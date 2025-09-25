using DotNetEnv;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using OTP_service.Models;
using OTP_service.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Collections.Generic;

Env.Load();

var builder = WebApplication.CreateSlimBuilder(args);

// Configure services
builder.Services.AddRouting();
builder.Services.Configure<RouteOptions>(options =>
{
    options.SetParameterPolicy<RegexInlineRouteConstraint>("regex");
});

// Configure JSON options
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

// FIXED: Add configuration sources in correct order (JSON first, then env vars override)
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables(); // Environment variables should override JSON settings

// Configure services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "OTP Service API", Version = "v1" });
});
builder.Services.AddOpenApi();

// FIXED: Redis Configuration - use consistent approach
var redisConnectionString = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;
    options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
    {
        EndPoints = { redisConnectionString },
        AbortOnConnectFail = false,
        ConnectRetry = 5,
        ConnectTimeout = 10000,
        SyncTimeout = 10000
    };
});

// Register services
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<ISmsService, SmsService>();
builder.Services.AddScoped<IRateLimitService, RateLimitService>();

// Health checks
builder.Services.AddHealthChecks()
    .AddRedis(redisConnectionString, name: "redis", tags: ["ready"]);

// FIXED: CORS Configuration
var allowedOrigins = builder.Configuration["CORS:AllowedOrigins"]?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? ["*"];

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        if (allowedOrigins.Length == 1 && allowedOrigins[0] == "*")
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
        else
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
    });
});

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.MapHealthChecks("/health");

// OTP API Endpoints
var otpGroup = app.MapGroup("/api/otp").WithTags("OTP Management");

otpGroup.MapPost("/send", async (
    SendOtpRequest request,
    IOtpService otpService,
    ISmsService smsService,
    IRateLimitService rateLimitService,
    IConfiguration config) =>
{
    // Validate request
    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(request);
    var isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

    if (!isValid)
    {
        var errors = validationResults
            .Where(v => !string.IsNullOrEmpty(v.ErrorMessage))
            .Select(v => v.ErrorMessage!)
            .ToList();

        return Results.BadRequest(new ApiResponse<object>
        {
            Success = false,
            Message = "Validation failed",
            Errors = errors
        });
    }

    // Rate limiting check
    if (!await rateLimitService.IsAllowedAsync(request.PhoneNumber))
    {
        var payload = new ApiResponse<object>
        {
            Success = false,
            Message = "Too many OTP requests. Please try again later."
        };
        var jsonTypeInfo = GetJsonTypeInfo(typeof(ApiResponse<object>));
        return Results.Json(payload, jsonTypeInfo, statusCode: 429);
    }

    try
    {
        // FIXED: Use consistent configuration reading
        var otpLength = config.GetValue<int>("OTP:DefaultLength", 6);
        var defaultExpiration = config.GetValue<int>("OTP:DefaultExpirationMinutes", 5);
        var maxExpiration = config.GetValue<int>("OTP:MaxExpirationMinutes", 30);

        // Generate OTP
        var otp = otpService.GenerateOtp(otpLength);
        var expirationMinutes = Math.Min(request.ExpirationMinutes ?? defaultExpiration, maxExpiration);

        // Store OTP in cache
        await otpService.StoreOtpAsync(request.PhoneNumber, otp, TimeSpan.FromMinutes(expirationMinutes));

        // FIXED: Use consistent configuration for SMS message
        var defaultMessage = config["SMS:DefaultMessage"] ?? "Your verification code is: {otp}. Valid for {expiration} minutes.";

        // Send SMS
        var message = request.CustomMessage ?? defaultMessage;
        message = message.Replace("{otp}", otp).Replace("{expiration}", expirationMinutes.ToString());

        var success = await smsService.SendSmsAsync(request.PhoneNumber, message);

        if (!success)
        {
            var payload = new ApiResponse<object>
            {
                Success = false,
                Message = "Failed to send SMS"
            };
            var jsonTypeInfo = GetJsonTypeInfo(typeof(ApiResponse<object>));
            return Results.Json(payload, jsonTypeInfo, statusCode: 500);
        }

        await rateLimitService.TrackRequestAsync(request.PhoneNumber);
        await rateLimitService.TrackRequestAsync(request.PhoneNumber);

        return Results.Ok(new ApiResponse<SendOtpResponse>
        {
            Success = true,
            Message = "OTP sent successfully",
            Data = new SendOtpResponse
            {
                RequestId = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes),
                PhoneNumber = MaskPhoneNumber(request.PhoneNumber)
            }
        });
    }
    catch (Exception ex)
    {
        var payload = new ApiResponse<object>
        {
            Success = false,
            Message = "Internal server error",
            Errors = [ex.Message]
        };
        var jsonTypeInfo = GetJsonTypeInfo(typeof(ApiResponse<object>));
        return Results.Json(payload, jsonTypeInfo, statusCode: 500);
    }
})
.WithName("SendOtp")
.WithSummary("Send OTP via SMS");

app.Run();

static string MaskPhoneNumber(string phoneNumber)
{
    if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length < 4)
        return phoneNumber;

    var start = phoneNumber[..Math.Min(4, phoneNumber.Length)];
    var end = phoneNumber.Length > 4 ? phoneNumber.Substring(phoneNumber.Length - 2) : "";
    var middle = new string('*', Math.Max(0, phoneNumber.Length - 6));

    return start + middle + end;
}

static JsonTypeInfo GetJsonTypeInfo(Type t)
{
    var info = AppJsonSerializerContext.Default?.GetTypeInfo(t);
    if (info is null)
        throw new InvalidOperationException($"Missing generated JsonTypeInfo for type '{t.FullName}'. Ensure AppJsonSerializerContext has a JsonSerializable attribute for the type.");
    return info;
}