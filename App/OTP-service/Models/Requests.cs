using System.ComponentModel.DataAnnotations;

namespace OTP_service.Models;
public class SendOtpRequest
{
    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Range(1, 30, ErrorMessage = "Expiration must be between 1 and 30 minutes")]
    public int? ExpirationMinutes { get; set; } = 5;

    public string? CustomMessage { get; set; }
}

public class VerifyOtpRequest
{
    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "OTP is required")]
    [RegularExpression(@"^\d{4,8}$", ErrorMessage = "OTP must be 4-8 digits")]
    public string Otp { get; set; } = string.Empty;

    public bool KeepOtp { get; set; } = false;
}
