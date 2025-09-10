using Dashboard.BussinessLogic;
using Dashboard.BussinessLogic.Mappings;
using Dashboard.DataAccess;
using Dashboard.Winform.Presenters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Dashboard.Winform
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

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

            builder.Services.AddAutoMapper(typeof(ReportMappingProfile));

            builder.Services.AddScoped<ILandingDashboardPresenter, LandingDashboardPresenter>();
            builder.Services.AddTransient<frmLandingDashboard>();
            builder.Services.AddTransient<FrmBaseMdiWithSidePanel>();

            using var host = builder.Build();

            ApplicationConfiguration.Initialize();

            var testForm = host.Services.GetRequiredService<FrmBaseMdiWithSidePanel>();
            Application.Run(testForm);
        }
    }
}
