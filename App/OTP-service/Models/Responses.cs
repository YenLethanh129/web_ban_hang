namespace OTP_service.Models;

public class SendOtpResponse
{
    public string RequestId { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
}

public class VerifyOtpResponse
{
    public bool Verified { get; set; }
    public DateTime? VerifiedAt { get; set; }
}

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}
