using Microsoft.Extensions.Caching.Distributed;

namespace OTP_service.Services;

public interface IRateLimitService
{
    Task<bool> IsAllowedAsync(string phoneNumber);
    Task TrackRequestAsync(string phoneNumber);
    Task<int> GetRemainingRequestsAsync(string phoneNumber);
}

public class RateLimitService : IRateLimitService
{
    private readonly IDistributedCache _cache;
    private readonly int _maxRequestsPerHour;
    private readonly int _maxRequestsPerDay;

    public RateLimitService(IDistributedCache cache, IConfiguration configuration)
    {
        _cache = cache;

        // FIXED: Use consistent configuration approach with proper fallback
        _maxRequestsPerHour = configuration.GetValue<int>("RateLimit:MaxRequestsPerHour", 5);
        _maxRequestsPerDay = configuration.GetValue<int>("RateLimit:MaxRequestsPerDay", 20);
    }

    public async Task<bool> IsAllowedAsync(string phoneNumber)
    {
        var hourlyCount = await GetRequestCountAsync(phoneNumber, TimeSpan.FromHours(1));
        var dailyCount = await GetRequestCountAsync(phoneNumber, TimeSpan.FromDays(1));

        return hourlyCount < _maxRequestsPerHour && dailyCount < _maxRequestsPerDay;
    }

    public async Task TrackRequestAsync(string phoneNumber)
    {
        await IncrementCountAsync(phoneNumber, TimeSpan.FromHours(1));
        await IncrementCountAsync(phoneNumber, TimeSpan.FromDays(1));
    }

    public async Task<int> GetRemainingRequestsAsync(string phoneNumber)
    {
        var hourlyCount = await GetRequestCountAsync(phoneNumber, TimeSpan.FromHours(1));
        var dailyCount = await GetRequestCountAsync(phoneNumber, TimeSpan.FromDays(1));

        var remainingHourly = Math.Max(0, _maxRequestsPerHour - hourlyCount);
        var remainingDaily = Math.Max(0, _maxRequestsPerDay - dailyCount);

        return Math.Min(remainingHourly, remainingDaily);
    }

    private async Task<int> GetRequestCountAsync(string phoneNumber, TimeSpan timeSpan)
    {
        var key = GetRateLimitKey(phoneNumber, timeSpan);
        var countStr = await _cache.GetStringAsync(key);
        return string.IsNullOrEmpty(countStr) ? 0 : int.Parse(countStr);
    }

    // FIXED: More efficient increment logic
    private async Task IncrementCountAsync(string phoneNumber, TimeSpan timeSpan)
    {
        var key = GetRateLimitKey(phoneNumber, timeSpan);
        var currentCount = await GetRequestCountAsync(phoneNumber, timeSpan);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = timeSpan
        };

        await _cache.SetStringAsync(key, (currentCount + 1).ToString(), options);
    }

    private static string GetRateLimitKey(string phoneNumber, TimeSpan timeSpan)
    {
        var cleanPhone = phoneNumber.Replace("+", "").Replace(" ", "").Replace("-", "");
        var suffix = timeSpan.TotalHours <= 1 ? "hourly" : "daily";
        return $"rate_limit:{cleanPhone}:{suffix}";
    }
}