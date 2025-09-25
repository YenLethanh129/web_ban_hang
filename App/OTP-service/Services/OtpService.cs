using Microsoft.Extensions.Caching.Distributed;

namespace OTP_service.Services;

public interface IOtpService
{
    string GenerateOtp(int length = 6);
    Task StoreOtpAsync(string phoneNumber, string otp, TimeSpan expiration);
    Task<bool> VerifyOtpAsync(string phoneNumber, string otp);
    Task ClearOtpAsync(string phoneNumber);
    Task<bool> OtpExistsAsync(string phoneNumber);
}
public class OtpService : IOtpService
{
    private readonly IDistributedCache _cache;
    private readonly Random _random = new();

    public OtpService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public string GenerateOtp(int length = 6)
    {
        var min = (int)Math.Pow(10, length - 1);
        var max = (int)Math.Pow(10, length) - 1;
        return _random.Next(min, max + 1).ToString();
    }

    public async Task StoreOtpAsync(string phoneNumber, string otp, TimeSpan expiration)
    {
        var key = GetOtpKey(phoneNumber);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        await _cache.SetStringAsync(key, otp, options);
    }

    public async Task<bool> VerifyOtpAsync(string phoneNumber, string otp)
    {
        var key = GetOtpKey(phoneNumber);
        var storedOtp = await _cache.GetStringAsync(key);

        return !string.IsNullOrEmpty(storedOtp) && storedOtp == otp;
    }

    public async Task ClearOtpAsync(string phoneNumber)
    {
        var key = GetOtpKey(phoneNumber);
        await _cache.RemoveAsync(key);
    }

    public async Task<bool> OtpExistsAsync(string phoneNumber)
    {
        var key = GetOtpKey(phoneNumber);
        var storedOtp = await _cache.GetStringAsync(key);
        return !string.IsNullOrEmpty(storedOtp);
    }

    private static string GetOtpKey(string phoneNumber) => $"otp:{phoneNumber.Replace("+", "").Replace(" ", "")}";
}
