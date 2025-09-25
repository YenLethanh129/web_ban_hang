using System.Text.Json.Serialization;

namespace OTP_service.Models;

[JsonSerializable(typeof(SendOtpRequest))]
[JsonSerializable(typeof(VerifyOtpRequest))]
[JsonSerializable(typeof(SendOtpResponse))]
[JsonSerializable(typeof(VerifyOtpResponse))]
[JsonSerializable(typeof(ApiResponse<object>))]
[JsonSerializable(typeof(ApiResponse<SendOtpResponse>))]
[JsonSerializable(typeof(ApiResponse<VerifyOtpResponse>))]
[JsonSerializable(typeof(List<string>))]
[JsonSerializable(typeof(string[]))]
public partial class AppJsonSerializerContext : JsonSerializerContext
{
}
