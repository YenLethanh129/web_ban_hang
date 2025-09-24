using Dashboard.BussinessLogic;
using Dashboard.DataAccess;
using Dashboard.Winform.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace Dashboard.Winform
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            builder.Services.AddLogging(config => config.AddConsole());

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

            // Test Service Locator It's kinda new thing I just know 'bout it today =)))) 
            var serviceProvider = builder.Services.BuildServiceProvider();
            FrmRecipeDetails.ServiceProviderHolder.Current = serviceProvider;


            using var host = builder.Build();

            ApplicationConfiguration.Initialize();
            var mainForm = host.Services.GetRequiredService<FrmBaseMdiWithSidePanel>();
            Application.Run(mainForm);
        }
    }
}
