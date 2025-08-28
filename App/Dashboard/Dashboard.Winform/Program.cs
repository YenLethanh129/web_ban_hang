using Dashboard.BussinessLogic;
using Dashboard.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dashboard.Winform
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            var builder = Host.CreateApplicationBuilder();

            // Load appsettings.json
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // Register services
            builder.AddDataAccess();
            builder.AddBussinessLogicServices();
            
            builder.Services.AddTransient<MainDashboardForm>();

            // Build
            using var host = builder.Build();

            ApplicationConfiguration.Initialize();
            var mainForm = host.Services.GetRequiredService<MainDashboardForm>();

            Application.Run(mainForm);
        }
    }
}