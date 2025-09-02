using Dashboard.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Dashboard.API.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            int statusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                ValidationException => StatusCodes.Status400BadRequest,
                ConflictException => StatusCodes.Status409Conflict,
                UnauthorizedAccessException or UnauthorizeException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };
            if (statusCode == StatusCodes.Status500InternalServerError)
            {
                _logger.LogError(ex, $"Unhandle exception: {ex.Message}.");
            }
            await HandleGlobalExceptionAsync(context, ex, statusCode);
        }
    }

    private static Task HandleGlobalExceptionAsync(HttpContext context, Exception ex, int statusCode)
    {
        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitle(statusCode),
            Detail = ex.Message,
            Instance = context.Request.Path
        };

        if (ex is ValidationException validationException)
        {
            problem.Detail = "One or more validation errors occurred.";
            problem.Extensions["errors"] = validationException.Errors;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower,
            WriteIndented = true
        };
        return context.Response.WriteAsync(JsonSerializer.Serialize(problem, options));
    }

    private static string GetTitle(int statusCode) =>
        statusCode switch
        {
            StatusCodes.Status404NotFound => "Not Found",
            StatusCodes.Status400BadRequest => "Bad Request",
            StatusCodes.Status409Conflict => "Conflict",
            StatusCodes.Status401Unauthorized => "Unauthorized",
            StatusCodes.Status500InternalServerError => "Internal Server Error",
            _ => "Error"
        };
}
