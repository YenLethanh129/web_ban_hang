using Dashboard.BussinessLogic;
using Dashboard.BussinessLogic.Mappings;
using Dashboard.DataAccess;
using Dashboard.Winform.Presenters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dashboard.Winform
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            builder.AddDataAccess();
            builder.AddBussinessLogicServices();

            builder.Services.AddAutoMapper(typeof(ReportMappingProfile));

            builder.Services.AddScoped<IDashboardPresenter, DashboardPresenter>();
            builder.Services.AddTransient<MainDashboardForm>();

            using var host = builder.Build();

            ApplicationConfiguration.Initialize();

            var mainForm = host.Services.GetRequiredService<MainDashboardForm>();
            Application.Run(mainForm);
        }
    }
}
