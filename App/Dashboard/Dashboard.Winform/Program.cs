using Dashboard.BussinessLogic;
using Dashboard.BussinessLogic.Security;
using Dashboard.Common.Options;
using Dashboard.DataAccess;
using Dashboard.DataAccess.Helpers;
using Dashboard.Winform.Forms;
using Dashboard.Winform.Mappings;
using Dashboard.Winform.Presenters;
using Dashboard.Winform.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            builder.Services.AddAutoMapper(typeof(ReportMappingProfile));
            builder.Services.AddAutoMapper(typeof(RBACViewModelMappingProfile));

            builder.Services.AddTransient<ILandingDashboardPresenter, LandingDashboardPresenter>();
            builder.Services.AddTransient<IEmployeeDetailsPresenter, EmployeeDetailsPresenter>();
            builder.Services.AddTransient<IEmployeeManagementPresenter, EmployeeManagementPresenter>();
            builder.Services.AddTransient<IUserManagementPresenter, UserManagementPresenter>();
            builder.Services.AddTransient<IRolePermissionManagementPresenter, RolePermissionManagementPresenter>();
            builder.Services.AddTransient<IScheduleManagementPresenter, ScheduleManagementPresenter>();

            builder.Services.AddScoped<EmployeeManagementPresenter>();
            builder.Services.AddTransient<ProductManagementPresenter>();
            builder.Services.AddTransient<RecipeManagementPresenter>();
            builder.Services.AddTransient<IngredientManagementPresenter>();

            // Register Token Cleanup Service
            builder.Services.AddScoped<ITokenCleanupService, TokenCleanupService>();
            builder.Services.AddHostedService<TokenCleanupBackgroundService>();


            // Register Forms
            builder.Services.AddTransient<FrmLandingDashboard>();
            builder.Services.AddTransient<FrmEmployeeManagement>();
            builder.Services.AddTransient<FrmBaseMdiWithSidePanel>();
            builder.Services.AddTransient<FrmProductManagement>();
            builder.Services.AddTransient<FrmRecipeDetails>();
            builder.Services.AddTransient<FrmProductDetails>();
            builder.Services.AddTransient<FrmEmployeeDetails>();
            builder.Services.AddTransient<FrmIngredientDetails>();
            builder.Services.AddTransient<FrmIngredientManagement>();
            builder.Services.AddTransient<FrmUserManagement>();
            builder.Services.AddTransient<FrmRolePermissionManagement>();
            builder.Services.AddTransient<FrmScheduleEditor>();



            var securitySection = builder.Configuration.GetSection("Security");
            builder.Services.Configure<SecurityOptions>(securitySection);
            builder.Services.Configure<JwtSettings>(securitySection.GetSection("Jwt"));

            // Register security services and helpers
            builder.Services.AddSingleton<JwtTokenService>(sp =>
            {
                var jwtOptions = sp.GetRequiredService<IOptions<JwtSettings>>().Value;
                return new JwtTokenService(jwtOptions);
            });
            builder.Services.AddSingleton<DataEncryptionHelper>();

            using var host = builder.Build();

            ApplicationConfiguration.Initialize();

            var testForm = host.Services.GetRequiredService<FrmBaseMdiWithSidePanel>();
            Application.Run(testForm);
        }
    }
}
