using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System.Text.Json;

namespace OTP_service.Services;

public interface ISmsService
{
    Task<bool> SendOtpAsync(string phoneNumber, string otpCode);
}

public class SmsService : ISmsService
{
    private readonly IAmazonSimpleNotificationService _snsClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SmsService> _logger;
    private readonly string _sandboxPhoneNumber;

    public SmsService(
        IAmazonSimpleNotificationService snsClient,
        IConfiguration configuration,
        ILogger<SmsService> logger)
    {
        _snsClient = snsClient;
        _configuration = configuration;
        _logger = logger;
        
        // Get sandbox phone number from configuration
        _sandboxPhoneNumber = _configuration["SMS:SandboxPhoneNumber"] 
                            ?? Environment.GetEnvironmentVariable("SMS_SANDBOX_PHONE_NUMBER")
                            ?? throw new InvalidOperationException("SMS sandbox phone number not configured");
    }

    public async Task<bool> SendOtpAsync(string phoneNumber, string otpCode)
    {
        try
        {
            // Tạo nội dung tin nhắn với format JSON như yêu cầu
            var messageContent = new
            {
                otp_code = otpCode,
                target_number = phoneNumber
            };

            var message = $"OTP Code: {otpCode}\nTarget: {phoneNumber}\nExpires in 5 minutes.";
            
            _logger.LogInformation("Sending OTP to sandbox number {SandboxNumber} with data: {MessageData}", 
                _sandboxPhoneNumber, JsonSerializer.Serialize(messageContent));

            // Tạo request với SMS Type là Promotional
            var publishRequest = new PublishRequest
            {
                PhoneNumber = _sandboxPhoneNumber, // Gửi tới số đã verify trong sandbox
                Message = message,
                MessageAttributes = new Dictionary<string, MessageAttributeValue>
                {
                    {
                        "AWS.SNS.SMS.SMSType",
                        new MessageAttributeValue
                        {
                            StringValue = "Promotional", // Set SMS type as Promotional
                            DataType = "String"
                        }
                    },
                    {
                        "AWS.SNS.SMS.MaxPrice",
                        new MessageAttributeValue
                        {
                            StringValue = "0.50", // Set max price per SMS (USD)
                            DataType = "Number"
                        }
                    }
                }
            };

            var response = await _snsClient.PublishAsync(publishRequest);
            
            if (!string.IsNullOrEmpty(response.MessageId))
            {
                _logger.LogInformation("Promotional SMS sent successfully. MessageId: {MessageId}, TargetPhone: {TargetPhone}", 
                    response.MessageId, phoneNumber);
                return true;
            }
            
            _logger.LogWarning("SMS sending returned no MessageId for phone: {PhoneNumber}", phoneNumber);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send promotional SMS to phone: {PhoneNumber}", phoneNumber);
            return false;
        }
    }
}