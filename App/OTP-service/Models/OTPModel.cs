using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OTP_service.Models;

public class SendOtpRequest
{
    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^(\+84|0)[0-9]{9,10}$", ErrorMessage = "Invalid Vietnamese phone number format")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Range(4, 8, ErrorMessage = "OTP length must be between 4 and 8 digits")]
    public int Length { get; set; } = 6;
}

public class VerifyOtpRequest
{
    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^(\+84|0)[0-9]{9,10}$", ErrorMessage = "Invalid Vietnamese phone number format")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "OTP is required")]
    [RegularExpression(@"^[0-9]{4,8}$", ErrorMessage = "OTP must be 4-8 digits")]
    public string Otp { get; set; } = string.Empty;

    public bool ClearAfterVerification { get; set; } = true;
}

public class SendOtpResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime? ExpiresAt { get; set; }
    public int RemainingAttempts { get; set; }
    public string MaskedPhoneNumber { get; set; } = string.Empty;
}

public class VerifyOtpResponse
{
    public bool IsValid { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime? VerifiedAt { get; set; }
    public int RemainingAttempts { get; set; }
}

public class OtpStatusResponse
{
    public bool Exists { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public int RemainingAttempts { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
}

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = [];
}