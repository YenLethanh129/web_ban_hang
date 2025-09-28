using Microsoft.Extensions.Caching.Distributed;

namespace OTP_service.Services;

public interface IOtpService
{
    string GenerateOtp(int length = 6);
    Task StoreOtpAsync(string phoneNumber, string otp, TimeSpan expiration);
    Task<bool> VerifyOtpAsync(string phoneNumber, string otp);
    Task ClearOtpAsync(string phoneNumber);
    Task<bool> OtpExistsAsync(string phoneNumber);
    Task<DateTime?> GetOtpExpirationAsync(string phoneNumber);
    Task<int> GetVerificationAttemptsAsync(string phoneNumber);
    Task IncrementVerificationAttemptsAsync(string phoneNumber);
    Task ClearVerificationAttemptsAsync(string phoneNumber);
}

public class OtpService : IOtpService
{
    private readonly IDistributedCache _cache;
    private readonly Random _random = new();
    private readonly IConfiguration _configuration;
    private readonly int _maxVerificationAttempts;

    public OtpService(IDistributedCache cache, IConfiguration configuration)
    {
        _cache = cache;
        _configuration = configuration;
        _maxVerificationAttempts = _configuration.GetValue<int>("OTP:MaxVerificationAttempts", 3);
    }

    public string GenerateOtp(int length = 6)
    {
        var min = (int)Math.Pow(10, length - 1);
        var max = (int)Math.Pow(10, length) - 1;
        return _random.Next(min, max + 1).ToString();
    }

    public async Task StoreOtpAsync(string phoneNumber, string otp, TimeSpan expiration)
    {
        var otpKey = GetOtpKey(phoneNumber);
        var expirationKey = GetOtpExpirationKey(phoneNumber);
        
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        // Store OTP
        await _cache.SetStringAsync(otpKey, otp, options);
        
        // Store expiration timestamp
        var expirationTime = DateTime.UtcNow.Add(expiration);
        await _cache.SetStringAsync(expirationKey, expirationTime.ToString("O"), options);
        
        // Reset verification attempts when new OTP is generated
        await ClearVerificationAttemptsAsync(phoneNumber);
    }

    public async Task<bool> VerifyOtpAsync(string phoneNumber, string otp)
    {
        var key = GetOtpKey(phoneNumber);
        var storedOtp = await _cache.GetStringAsync(key);

        var isValid = !string.IsNullOrEmpty(storedOtp) && storedOtp == otp;
        
        if (!isValid)
        {
            await IncrementVerificationAttemptsAsync(phoneNumber);
        }

        return isValid;
    }

    public async Task ClearOtpAsync(string phoneNumber)
    {
        var otpKey = GetOtpKey(phoneNumber);
        var expirationKey = GetOtpExpirationKey(phoneNumber);
        
        await _cache.RemoveAsync(otpKey);
        await _cache.RemoveAsync(expirationKey);
        await ClearVerificationAttemptsAsync(phoneNumber);
    }

    public async Task<bool> OtpExistsAsync(string phoneNumber)
    {
        var key = GetOtpKey(phoneNumber);
        var storedOtp = await _cache.GetStringAsync(key);
        return !string.IsNullOrEmpty(storedOtp);
    }

    public async Task<DateTime?> GetOtpExpirationAsync(string phoneNumber)
    {
        var key = GetOtpExpirationKey(phoneNumber);
        var expirationStr = await _cache.GetStringAsync(key);
        
        if (string.IsNullOrEmpty(expirationStr))
            return null;
            
        if (DateTime.TryParse(expirationStr, out var expiration))
            return expiration;
            
        return null;
    }

    public async Task<int> GetVerificationAttemptsAsync(string phoneNumber)
    {
        var key = GetVerificationAttemptsKey(phoneNumber);
        var attemptsStr = await _cache.GetStringAsync(key);
        return string.IsNullOrEmpty(attemptsStr) ? 0 : int.Parse(attemptsStr);
    }

    public async Task IncrementVerificationAttemptsAsync(string phoneNumber)
    {
        var key = GetVerificationAttemptsKey(phoneNumber);
        var currentAttempts = await GetVerificationAttemptsAsync(phoneNumber);
        var newAttempts = currentAttempts + 1;

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        };

        await _cache.SetStringAsync(key, newAttempts.ToString(), options);

        // If max attempts reached, clear the OTP
        if (newAttempts >= _maxVerificationAttempts)
        {
            await ClearOtpAsync(phoneNumber);
        }
    }

    public async Task ClearVerificationAttemptsAsync(string phoneNumber)
    {
        var key = GetVerificationAttemptsKey(phoneNumber);
        await _cache.RemoveAsync(key);
    }

    private static string GetOtpKey(string phoneNumber) => 
        $"otp:{phoneNumber.Replace("+", "").Replace(" ", "").Replace("-", "")}";
    
    private static string GetOtpExpirationKey(string phoneNumber) => 
        $"otp_exp:{phoneNumber.Replace("+", "").Replace(" ", "").Replace("-", "")}";
    
    private static string GetVerificationAttemptsKey(string phoneNumber) => 
        $"otp_attempts:{phoneNumber.Replace("+", "").Replace(" ", "").Replace("-", "")}";
}

