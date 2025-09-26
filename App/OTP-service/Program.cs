using DotNetEnv;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.OpenApi.Models;
using OTP_service.Models;
using OTP_service.Services;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon;

// Load environment variables from .env file first
Env.Load();

var builder = WebApplication.CreateSlimBuilder(args);

// Configure services
builder.Services.AddRouting();

// Configure JSON options
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.WriteIndented = true;
});

// Add configuration sources - .env variables will override appsettings
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "OTP Service API", Version = "v1" });
});
builder.Services.AddOpenApi();

// Redis Configuration - prioritize environment variable
var redisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING") 
    ?? builder.Configuration.GetConnectionString("Redis") 
    ?? "localhost:6379";

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

// AWS SNS Configuration
var awsAccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
var awsSecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
var awsRegion = Environment.GetEnvironmentVariable("AWS_REGION") ?? builder.Configuration["AWS:Region"] ?? "ap-southeast-1";

if (!string.IsNullOrEmpty(awsAccessKey) && !string.IsNullOrEmpty(awsSecretKey))
{
    // Use explicit credentials from environment variables
    builder.Services.AddSingleton<IAmazonSimpleNotificationService>(_ =>
    {
        var config = new AmazonSimpleNotificationServiceConfig
        {
            RegionEndpoint = RegionEndpoint.GetBySystemName(awsRegion)
        };
        return new AmazonSimpleNotificationServiceClient(awsAccessKey, awsSecretKey, config);
    });
}
else
{
    // Use default credential chain (IAM roles, profiles, etc.)
    builder.Services.AddSingleton<IAmazonSimpleNotificationService>(_ =>
    {
        var config = new AmazonSimpleNotificationServiceConfig
        {
            RegionEndpoint = RegionEndpoint.GetBySystemName(awsRegion)
        };
        return new AmazonSimpleNotificationServiceClient(config);
    });
}

// Register services
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<IRateLimitService, RateLimitService>();
builder.Services.AddScoped<ISmsService, SmsService>();

// Health checks
builder.Services.AddHealthChecks()
    .AddRedis(redisConnectionString, name: "redis", tags: ["ready"]);

// CORS Configuration
var allowedOrigins = Environment.GetEnvironmentVariable("CORS_ALLOWED_ORIGINS")?.Split(',', StringSplitOptions.RemoveEmptyEntries) 
    ?? builder.Configuration["CORS:AllowedOrigins"]?.Split(',', StringSplitOptions.RemoveEmptyEntries) 
    ?? ["*"];

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

// Send OTP endpoint
otpGroup.MapPost("/send", async (SendOtpRequest request, IOtpService otpService, ISmsService smsService, IRateLimitService rateLimitService) =>
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

    try
    {
        // Check rate limiting
        if (!await rateLimitService.IsAllowedAsync(request.PhoneNumber))
        {
            return Results.Json(
                new ApiResponse<object>
                {
                    Success = false,
                    Message = "Too many requests. Please try again later.",
                    Errors = ["Rate limit exceeded"]
                },
                statusCode: 429
            );
        }

        // Generate OTP
        var otpLength = Math.Max(4, Math.Min(8, request.Length));
        var otp = otpService.GenerateOtp(otpLength);
        
        // Get expiration time from config
        var expirationMinutes = int.TryParse(Environment.GetEnvironmentVariable("OTP_EXPIRATION_MINUTES"), out var expMin) 
            ? expMin 
            : 5;
        var expiration = TimeSpan.FromMinutes(expirationMinutes);

        // Store OTP
        await otpService.StoreOtpAsync(request.PhoneNumber, otp, expiration);

        // Send SMS
        var smsSent = await smsService.SendOtpAsync(request.PhoneNumber, otp);
        
        if (smsSent)
        {
            // Track request for rate limiting
            await rateLimitService.TrackRequestAsync(request.PhoneNumber);

            return Results.Ok(new ApiResponse<SendOtpResponse>
            {
                Success = true,
                Message = "OTP sent successfully",
                Data = new SendOtpResponse
                {
                    Success = true,
                    Message = "OTP has been sent to your phone number",
                    ExpiresAt = DateTime.UtcNow.Add(expiration),
                    RemainingAttempts = await rateLimitService.GetRemainingRequestsAsync(request.PhoneNumber),
                    MaskedPhoneNumber = MaskPhoneNumber(request.PhoneNumber)
                }
            });
        }
        else
        {
            // Clear stored OTP if SMS sending failed
            await otpService.ClearOtpAsync(request.PhoneNumber);
            
            return Results.Problem(
                detail: "Unable to send OTP at this time. Please try again later.",
                statusCode: 500,
                title: "SMS Sending Failed");
        }
    }
    catch (Exception ex)
    {
        return Results.Problem(
            detail: ex.Message,
            statusCode: 500,
            title: "Internal Server Error");
    }
})
.WithName("SendOtp")
.WithSummary("Send OTP to phone number");

// Verify OTP endpoint
otpGroup.MapPost("/verify", async (VerifyOtpRequest request, IOtpService otpService, IRateLimitService rateLimitService) =>
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

    try
    {
        // Check if OTP exists
        if (!await otpService.OtpExistsAsync(request.PhoneNumber))
        {
            return Results.BadRequest(new ApiResponse<VerifyOtpResponse>
            {
                Success = false,
                Message = "No OTP found for this phone number or OTP has expired",
                Data = new VerifyOtpResponse
                {
                    IsValid = false,
                    Message = "OTP not found or expired",
                    RemainingAttempts = await rateLimitService.GetRemainingRequestsAsync(request.PhoneNumber)
                }
            });
        }

        // Verify OTP
        var isValidOtp = await otpService.VerifyOtpAsync(request.PhoneNumber, request.Otp);
        var remainingAttempts = await otpService.GetVerificationAttemptsAsync(request.PhoneNumber);
        var maxAttempts = int.TryParse(Environment.GetEnvironmentVariable("OTP_MAX_VERIFICATION_ATTEMPTS"), out var max) ? max : 3;

        if (isValidOtp)
        {
            // Clear OTP if verification successful and requested
            if (request.ClearAfterVerification)
            {
                await otpService.ClearOtpAsync(request.PhoneNumber);
            }

            return Results.Ok(new ApiResponse<VerifyOtpResponse>
            {
                Success = true,
                Message = "OTP verified successfully",
                Data = new VerifyOtpResponse
                {
                    IsValid = true,
                    Message = "Verification successful",
                    VerifiedAt = DateTime.UtcNow,
                    RemainingAttempts = maxAttempts - remainingAttempts
                }
            });
        }
        else
        {
            var attemptsLeft = Math.Max(0, maxAttempts - remainingAttempts);
            var message = attemptsLeft > 0
                ? $"Invalid OTP. {attemptsLeft} attempts remaining."
                : "Maximum verification attempts exceeded. Please request a new OTP.";

            return Results.BadRequest(new ApiResponse<VerifyOtpResponse>
            {
                Success = false,
                Message = message,
                Data = new VerifyOtpResponse
                {
                    IsValid = false,
                    Message = message,
                    RemainingAttempts = attemptsLeft
                }
            });
        }
    }
    catch (Exception ex)
    {
        return Results.Problem(
            detail: ex.Message,
            statusCode: 500,
            title: "Verification Error");
    }
})
.WithName("VerifyOtp")
.WithSummary("Verify OTP code");

// Get OTP status endpoint
otpGroup.MapGet("/status/{phoneNumber}", async (string phoneNumber, IOtpService otpService, IRateLimitService rateLimitService) =>
{
    try
    {
        var exists = await otpService.OtpExistsAsync(phoneNumber);
        var expiration = await otpService.GetOtpExpirationAsync(phoneNumber);
        var remainingAttempts = await rateLimitService.GetRemainingRequestsAsync(phoneNumber);

        return Results.Ok(new ApiResponse<OtpStatusResponse>
        {
            Success = true,
            Message = exists ? "OTP status retrieved successfully" : "No active OTP found",
            Data = new OtpStatusResponse
            {
                Exists = exists,
                ExpiresAt = expiration,
                RemainingAttempts = remainingAttempts,
                PhoneNumber = MaskPhoneNumber(phoneNumber)
            }
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(
            detail: ex.Message,
            statusCode: 500,
            title: "Status Retrieval Error");
    }
})
.WithName("GetOtpStatus")
.WithSummary("Check OTP status for a phone number");

// Clear OTP endpoint
otpGroup.MapDelete("/{phoneNumber}", async (string phoneNumber, IOtpService otpService) =>
{
    try
    {
        await otpService.ClearOtpAsync(phoneNumber);

        return Results.Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "OTP cleared successfully"
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(
            detail: ex.Message,
            statusCode: 500,
            title: "OTP Clear Error");
    }
})
.WithName("ClearOtp")
.WithSummary("Clear OTP for a phone number");

app.Run();

static string MaskPhoneNumber(string phoneNumber)
{
    if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length < 4)
        return phoneNumber;

    var start = phoneNumber[..Math.Min(4, phoneNumber.Length)];
    string phoneNumber1 = phoneNumber;
    var end = phoneNumber.Length > 4 ? phoneNumber[(phoneNumber1.Length - 2)..] : "";
    var middle = new string('*', Math.Max(0, phoneNumber.Length - 6));

    return start + middle + end;
}