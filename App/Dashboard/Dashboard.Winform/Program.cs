using Dashboard.BussinessLogic;
using Dashboard.Common.Options;
using Dashboard.DataAccess;
using Dashboard.Winform.Forms;
using Dashboard.Winform.Forms.CostFrms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;

namespace Dashboard.Winform;

internal static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        LoadEnvironmentVariables();

        var builder = Host.CreateApplicationBuilder(args);

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables(); 

        builder.Services.Configure<ImageUploadOptions>(options =>
        {
            var config = builder.Configuration.GetSection("ImageUpload");
            config.Bind(options);

            var storageType = Environment.GetEnvironmentVariable("STORAGE_TYPE");
            if (!string.IsNullOrEmpty(storageType))
            {
                options.StorageType = storageType;
            }

            var s3BucketName = Environment.GetEnvironmentVariable("S3_BUCKET_NAME");
            var s3Region = Environment.GetEnvironmentVariable("S3_REGION");
            var s3AccessKey = Environment.GetEnvironmentVariable("S3_ACCESS_KEY");
            var s3SecretKey = Environment.GetEnvironmentVariable("S3_SECRET_KEY");
            var s3CloudFront = Environment.GetEnvironmentVariable("S3_CLOUDFRONT_DOMAIN");

            if (!string.IsNullOrEmpty(s3BucketName))
                options.S3.BucketName = s3BucketName;
            if (!string.IsNullOrEmpty(s3Region))
                options.S3.Region = s3Region;
            if (!string.IsNullOrEmpty(s3AccessKey))
                options.S3.AccessKey = s3AccessKey;
            if (!string.IsNullOrEmpty(s3SecretKey))
                options.S3.SecretKey = s3SecretKey;
            if (!string.IsNullOrEmpty(s3CloudFront))
                options.S3.CloudFrontDomain = s3CloudFront;
        });

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(dispose: true);
        });

        builder.AddDataAccess();
        builder.AddBussinessLogicServices();
        builder.Services.AddWinformDependencies(builder.Configuration);

        var serviceProvider = builder.Services.BuildServiceProvider();
        FrmRecipeDetails.ServiceProviderHolder.Current = serviceProvider;

        using var host = builder.Build();
        ApplicationConfiguration.Initialize();

        //var mainForm = host.Services.GetRequiredService<FrmBaseMdiWithSidePanel>();
        var mainForm = host.Services.GetRequiredService<FrmCostStoreRunning>();
        Application.Run(mainForm);
    }

    private static void LoadEnvironmentVariables()
    {
        try
        {
            var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");

            if (File.Exists(envPath))
            {
                DotNetEnv.Env.Load(envPath);
                Console.WriteLine($"Loaded .env from: {envPath}");
            }
            else
            {
                var parentEnvPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");
                if (File.Exists(parentEnvPath))
                {
                    DotNetEnv.Env.Load(parentEnvPath);
                    Console.WriteLine($"Loaded .env from: {parentEnvPath}");
                }
                else
                {
                    Console.WriteLine("No .env file found. Using system environment variables only.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading .env file: {ex.Message}");
        }
    }
}