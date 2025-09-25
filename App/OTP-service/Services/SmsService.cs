// SmsService.cs - Fixed configuration binding
using Microsoft.Extensions.Caching.Distributed;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OTP_service.Services;

[JsonSerializable(typeof(string[]))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(int))]
public partial class SmsServiceJsonContext : JsonSerializerContext
{
}

public interface ISmsService
{
    Task<bool> SendSmsAsync(string phoneNumber, string message);
}

public class SmsService : ISmsService
{
    private const string SPEEDSMS_API_URL = "https://api.speedsms.vn/index.php";
    private const int TYPE_QC = 1;
    private const int TYPE_CSKH = 2;
    private const int TYPE_BRANDNAME = 3;
    private const int TYPE_BRANDNAME_NOTIFY = 4;
    private const int TYPE_GATEWAY = 5;

    private readonly ILogger<SmsService> _logger;
    private readonly IConfiguration _configuration;
    private readonly string _accessToken;

    public SmsService(ILogger<SmsService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;

        // FIXED: Use consistent configuration approach
        _accessToken = _configuration["SpeedSMS:AccessToken"] ??
                      throw new InvalidOperationException("SpeedSMS access token is required");
    }

    public async Task<bool> SendSmsAsync(string phoneNumber, string message)
    {
        try
        {
            var formattedNumber = FormatPhoneNumber(phoneNumber);

            // FIXED: Use consistent configuration approach
            var smsType = _configuration.GetValue<int>("SpeedSMS:Type", TYPE_CSKH);
            var brandName = "";

            if (smsType == TYPE_BRANDNAME || smsType == TYPE_BRANDNAME_NOTIFY)
            {
                brandName = _configuration["SpeedSMS:BrandName"] ?? "OTP-Service";
            }

            var response = await SendSmsHttpRequest(formattedNumber, message, smsType, brandName);
            var success = IsResponseSuccessful(response);

            if (success)
            {
                _logger.LogInformation("SMS sent successfully to {PhoneNumber} via SpeedSMS",
                    MaskPhoneNumber(phoneNumber));
            }
            else
            {
                _logger.LogWarning("Failed to send SMS to {PhoneNumber} via SpeedSMS. Response: {Response}",
                    MaskPhoneNumber(phoneNumber), response);
            }

            return success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send SMS to {PhoneNumber} via SpeedSMS",
                MaskPhoneNumber(phoneNumber));
            return false;
        }
    }

    // FIXED: Use source-generated JSON serialization
    private async Task<string> SendSmsHttpRequest(string phoneNumber, string message, int type, string sender)
    {
        using var client = new HttpClient();

        var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_accessToken}::x"));
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

        var payload = new
        {
            to = new[] { phoneNumber },
            content = Uri.EscapeDataString(message),
            type = type,
            sender = sender
        };

        // Use source-generated context for better AOT performance
        var jsonContent = JsonSerializer.Serialize(payload, SmsServiceJsonContext.Default.Options);
        var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await client.PostAsync($"{SPEEDSMS_API_URL}/sms/send", stringContent);
        return await response.Content.ReadAsStringAsync();
    }

    private static string FormatPhoneNumber(string phoneNumber)
    {
        var digits = new string(phoneNumber.Where(char.IsDigit).ToArray());

        if (digits.StartsWith("84"))
        {
            digits = "0" + digits.Substring(2);
        }
        else if (!digits.StartsWith("0"))
        {
            digits = "0" + digits;
        }

        return digits;
    }

    private static bool IsResponseSuccessful(string response)
    {
        if (string.IsNullOrEmpty(response))
            return false;

        try
        {
            using var doc = JsonDocument.Parse(response);
            var root = doc.RootElement;

            // Check for success status
            if (root.TryGetProperty("status", out var statusElement))
            {
                var status = statusElement.GetString();
                return string.Equals(status, "success", StringComparison.OrdinalIgnoreCase);
            }

            // Check for error property
            if (root.TryGetProperty("error", out _))
            {
                return false;
            }

            // Check for message ID
            if (root.TryGetProperty("message_id", out var messageIdElement) ||
                root.TryGetProperty("messageId", out messageIdElement))
            {
                return !string.IsNullOrEmpty(messageIdElement.GetString());
            }

            return true;
        }
        catch (JsonException)
        {
            // Fallback to string checking
            var lowerResponse = response.ToLower();
            return lowerResponse.Contains("success") &&
                   !lowerResponse.Contains("error") &&
                   !lowerResponse.Contains("fail");
        }
    }

    private static string MaskPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length < 4)
            return phoneNumber;

        var start = phoneNumber.Substring(0, Math.Min(4, phoneNumber.Length));
        var end = phoneNumber.Length > 4 ? phoneNumber.Substring(phoneNumber.Length - 2) : "";
        var middle = new string('*', Math.Max(0, phoneNumber.Length - 6));

        return start + middle + end;
    }
}