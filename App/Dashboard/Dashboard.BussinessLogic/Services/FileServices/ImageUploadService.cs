using Amazon.S3;
using Amazon.S3.Model;
using Dashboard.Common.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dashboard.BussinessLogic.Services.FileServices;

public interface IImageUploadService
{
    Task<string> UploadImageAsync(string imageSource, string? fileName = null);
    Task<bool> DeleteImageAsync(string imagePath);
    Task<string> GetImageUrlAsync(string imagePath);
    bool IsValidImageFormat(string fileName);
}

public class ImageUploadService : IImageUploadService
{
    private readonly ImageUploadOptions _config;
    private readonly ILogger<ImageUploadService> _logger;
    private readonly IAmazonS3? _s3Client;
    private static readonly HttpClient _httpClient = new();

    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
    private readonly string[] _allowedContentTypes = [
        "image/jpeg", "image/jpg", "image/png", "image/gif",
        "image/bmp", "image/webp"
    ];

    public ImageUploadService(
        IOptions<ImageUploadOptions> config,
        ILogger<ImageUploadService> logger,
        IAmazonS3? s3Client = null)
    {
        _config = config.Value;
        _logger = logger;
        _s3Client = s3Client;
    }

    public async Task<string> UploadImageAsync(string imageSource, string? fileName = null)
    {
        try
        {
            if (string.IsNullOrEmpty(imageSource))
                throw new ArgumentException("Image source cannot be null or empty");

            var uniqueFileName = fileName ?? $"{Guid.NewGuid()}{GetExtensionFromSource(imageSource)}";

            if (_config.StorageType.Equals("S3", StringComparison.OrdinalIgnoreCase))
            {
                return await UploadToS3Async(imageSource, uniqueFileName);
            }
            else
            {
                return await UploadToLocalAsync(imageSource, uniqueFileName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload image from source: {ImageSource}", imageSource);
            throw;
        }
    }

    private async Task<string> UploadToS3Async(string imageSource, string fileName)
    {
        if (_s3Client == null)
            throw new InvalidOperationException("S3 client is not configured");

        using var imageStream = await GetImageStreamAsync(imageSource);
        var key = $"product-images/{fileName}";

        var request = new PutObjectRequest
        {
            BucketName = _config.S3.BucketName,
            Key = key,
            InputStream = imageStream,
            ContentType = GetContentTypeFromFileName(fileName),
            CannedACL = S3CannedACL.NoACL,
        };

        var response = await _s3Client.PutObjectAsync(request);

        if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new Exception($"Failed to upload to S3. Status: {response.HttpStatusCode}");
        }

        if (!string.IsNullOrEmpty(_config.S3.CloudFrontDomain))
        {
            return $"https://{_config.S3.CloudFrontDomain.TrimEnd('/')}/{fileName}";
        }

        return $"https://{_config.S3.BucketName}.s3.{_config.S3.Region}.amazonaws.com/{key}";
    }

    private async Task<string> UploadToLocalAsync(string imageSource, string fileName)
    {
        var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), _config.LocalUploadPath);
        Directory.CreateDirectory(uploadsDir);

        var filePath = Path.Combine(uploadsDir, fileName);

        using var imageStream = await GetImageStreamAsync(imageSource);
        using var fileStream = new FileStream(filePath, FileMode.Create);
        await imageStream.CopyToAsync(fileStream);

        return Path.Combine(_config.LocalUploadPath, fileName).Replace("\\", "/");
    }

    private async Task<Stream> GetImageStreamAsync(string imageSource)
    {
        if (Uri.TryCreate(imageSource, UriKind.Absolute, out var uri) &&
            (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
        {
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }

        if (File.Exists(imageSource))
        {
            return new FileStream(imageSource, FileMode.Open, FileAccess.Read);
        }

        throw new ArgumentException($"Invalid image source: {imageSource}");
    }

    public async Task<bool> DeleteImageAsync(string imagePath)
    {
        try
        {
            if (_config.StorageType.Equals("S3", StringComparison.OrdinalIgnoreCase))
            {
                return await DeleteFromS3Async(imagePath);
            }
            else
            {
                return DeleteFromLocal(imagePath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete image: {ImagePath}", imagePath);
            return false;
        }
    }

    private async Task<bool> DeleteFromS3Async(string imagePath)
    {
        if (_s3Client == null) return false;

        try
        {
            var key = ExtractS3KeyFromUrl(imagePath);
            if (string.IsNullOrEmpty(key)) return false;

            var request = new DeleteObjectRequest
            {
                BucketName = _config.S3.BucketName,
                Key = key
            };

            await _s3Client.DeleteObjectAsync(request);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete from S3: {ImagePath}", imagePath);
            return false;
        }
    }

    private bool DeleteFromLocal(string imagePath)
    {
        try
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), imagePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete local file: {ImagePath}", imagePath);
            return false;
        }
    }

    public Task<string> GetImageUrlAsync(string imagePath)
    {
        if (_config.StorageType.Equals("S3", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(imagePath);
        }
        else
        {
            return Task.FromResult(imagePath);
        }
    }

    public bool IsValidImageFormat(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return _allowedExtensions.Contains(extension);
    }

    private string GetExtensionFromSource(string imageSource)
    {
        if (Uri.TryCreate(imageSource, UriKind.Absolute, out var uri))
        {
            var extension = Path.GetExtension(uri.AbsolutePath);
            return string.IsNullOrEmpty(extension) ? ".jpg" : extension;
        }

        var localExtension = Path.GetExtension(imageSource);
        return string.IsNullOrEmpty(localExtension) ? ".jpg" : localExtension;
    }

    private string GetContentTypeFromFileName(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".webp" => "image/webp",
            _ => "image/jpeg"
        };
    }

    private string? ExtractS3KeyFromUrl(string url)
    {
        try
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                // Handle CloudFront URLs
                if (!string.IsNullOrEmpty(_config.S3.CloudFrontDomain) &&
                    uri.Host.Contains(_config.S3.CloudFrontDomain))
                {
                    return uri.AbsolutePath.TrimStart('/');
                }

                if (uri.Host.Contains("s3") && uri.Host.Contains(_config.S3.BucketName))
                {
                    return uri.AbsolutePath.TrimStart('/');
                }
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
}
