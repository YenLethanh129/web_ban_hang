using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Common.Options
{
    public class ImageUploadOptions
    {
        public string StorageType { get; set; } = "Local";
        public string LocalUploadPath { get; set; } = "Resources/Uploads";
        public S3Options S3 { get; set; } = new();
    }

    public class S3Options
    {
        public string BucketName { get; set; } = string.Empty;
        public string Region { get; set; } = "ap-southeast-1";
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string? CloudFrontDomain { get; set; }
    }
}