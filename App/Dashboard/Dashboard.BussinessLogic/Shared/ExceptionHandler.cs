using Dashboard.Common.Exceptions;
using Microsoft.Extensions.Logging;

namespace Dashboard.BussinessLogic.Shared;

public static class ExceptionHandler
{
    public static ExceptionResult Handle(Exception ex, ILogger? logger = null)
    {
        var result = new ExceptionResult
        {
            StatusCode = ex switch
            {
                NotFoundException => 404,
                BadRequestException => 400,
                ValidationException => 400,
                ConflictException => 409,
                UnauthorizedAccessException or UnauthorizeException => 401,
                _ => 500
            },
            Title = GetTitle(ex),
            Message = ex.Message,
            Errors = (ex as ValidationException)?.Errors
        };

        if (logger != null && result.StatusCode == 500)
        {
            logger.LogError(ex, $"Unhandled exception occurred: {ex.Message}");
        }

        return result;
    }

    private static string GetTitle(Exception ex) =>
        ex switch
        {
            NotFoundException => "Not Found",
            BadRequestException => "Bad Request",
            ValidationException => "Validation Failed",
            ConflictException => "Conflict",
            UnauthorizedAccessException or UnauthorizeException => "Unauthorized",
            _ => "Internal Server Error"
        };
}

public class ExceptionResult
{
    public int StatusCode { get; set; }
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public object? Errors { get; set; }
}
