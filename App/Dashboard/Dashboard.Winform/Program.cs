using Dashboard.BussinessLogic;
using Dashboard.DataAccess;
using Dashboard.Winform.Forms;
using Dashboard.Winform.Mappings;
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
            builder.Services.AddAutoMapper(typeof(BranchViewModelMappingProfile));
            builder.Services.AddAutoMapper(typeof(ProductViewModelMappingProfile));

            builder.Services.AddTransient<ILandingDashboardPresenter, LandingDashboardPresenter>();
            builder.Services.AddTransient<IEmployeeDetailsPresenter, EmployeeDetailsPresenter>();
            builder.Services.AddTransient<IEmployeeManagementPresenter, EmployeeManagementPresenter>();
            //builder.Services.AddTransient<IProdu, ProductDetailsPresenter>();
            builder.Services.AddScoped<EmployeeManagementPresenter>();
            builder.Services.AddTransient<ProductManagementPresenter>();
            builder.Services.AddTransient<RecipeManagementPresenter>();

            // Register Forms
            builder.Services.AddTransient<frmLandingDashboard>();
            builder.Services.AddTransient<FrmEmployeeManagement>();
            builder.Services.AddTransient<FrmBaseMdiWithSidePanel>();
            builder.Services.AddTransient<FrmProductManagement>();
            builder.Services.AddTransient<FrmRecipeDetails>();
            builder.Services.AddTransient<FrmProductDetails>();
            builder.Services.AddTransient<FrmEmployeeDetails>();

            using var host = builder.Build();

            ApplicationConfiguration.Initialize();

            var testForm = host.Services.GetRequiredService<FrmBaseMdiWithSidePanel>();
            Application.Run(testForm);
        }
    }
}
