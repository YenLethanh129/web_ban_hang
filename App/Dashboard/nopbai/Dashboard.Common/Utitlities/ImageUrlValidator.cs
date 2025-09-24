using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Dashboard.Common.Utitlities
{
    public interface IImageUrlValidator
    {
        Task<bool> ValidateAsync(string imageUrl);
    }

    public class ImageUrlValidator : IImageUrlValidator
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ImageUrlValidator>? _logger;

        private static readonly string[] AllowedExtensions =
        {
            ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".svg"
        };

        private static readonly string[] TrustedDomains =
        {
            "media.istockphoto.com",
            "avatars.githubusercontent.com",
            "images.unsplash.com",
            "cdn.pixabay.com",
            "images.pexels.com",
        };

        public ImageUrlValidator(HttpClient httpClient, ILogger<ImageUrlValidator>? logger = null)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> ValidateAsync(string imageUrl)
        {
            try
            {
                // --- Case 1: Local file ---
                if (File.Exists(imageUrl))
                {
                    var ext = Path.GetExtension(imageUrl).ToLowerInvariant();
                    return AllowedExtensions.Contains(ext);
                }

                // --- Case 2: Remote URL ---
                if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri))
                    return false;

                if (uri.Scheme != Uri.UriSchemeHttps)
                    return false;

                // Optional: domain whitelist
                var host = uri.Host.ToLowerInvariant();
                var isTrustedDomain = TrustedDomains.Any(domain =>
                    host == domain || host.EndsWith("." + domain));
                // if (!isTrustedDomain) return false;

                var path = uri.AbsolutePath.ToLowerInvariant();
                var hasValidExtension = AllowedExtensions.Any(ext => path.EndsWith(ext));

                if (hasValidExtension)
                    return true;

                // --- Case 3: Không có extension → HEAD request ---
                try
                {
                    using var request = new HttpRequestMessage(HttpMethod.Head, uri);
                    request.Headers.Add("User-Agent", "Mozilla/5.0 (compatible; ImageValidator/1.0)");

                    using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                    if (response.IsSuccessStatusCode)
                    {
                        var contentType = response.Content.Headers.ContentType?.MediaType;
                        if (!string.IsNullOrEmpty(contentType) && contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning("HEAD request failed for URL {Url}. Error: {Error}", imageUrl, ex.Message);
                    return false;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error while validating image URL: {Url}", imageUrl);
                return false;
            }
        }
    }
}
