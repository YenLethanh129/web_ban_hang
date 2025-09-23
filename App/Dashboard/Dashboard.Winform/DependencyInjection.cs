using Dashboard.BussinessLogic.Security;
using Dashboard.BussinessLogic.Services.ProductServices;
using Dashboard.Common.Options;
using Dashboard.DataAccess.Helpers;
using Dashboard.Winform.Forms;
using Dashboard.Winform.Forms.SupplierFrm;
using Dashboard.Winform.Mappings;
using Dashboard.Winform.Presenters;
using Dashboard.Winform.Presenters.EmployeePresenter;
using Dashboard.Winform.Presenters.IngredientPresenters;
using Dashboard.Winform.Presenters.ProductPresenters;
using Dashboard.Winform.Presenters.RecipePresenters;
using Dashboard.Winform.Presenters.SupplierPresenters;
using Dashboard.Winform.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Dashboard.Winform
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddWinformDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(ReportMappingProfile));
            services.AddAutoMapper(typeof(BranchViewModelMappingProfile));
            services.AddAutoMapper(typeof(ProductViewModelMappingProfile));
            services.AddAutoMapper(typeof(RBACViewModelMappingProfile));

            services.AddTransient<ILandingDashboardPresenter, LandingDashboardPresenter>();
            services.AddTransient<IEmployeeDetailsPresenter, EmployeeDetailsPresenter>();
            services.AddTransient<IEmployeeManagementPresenter, EmployeeManagementPresenter>();
            services.AddTransient<IUserManagementPresenter, UserManagementPresenter>();
            services.AddTransient<IRolePermissionManagementPresenter, RolePermissionManagementPresenter>();
            services.AddTransient<IScheduleManagementPresenter, ScheduleManagementPresenter>();
            services.AddTransient<IProductDetailPresenter, ProductDetailPresenter>();
            services.AddTransient<IProductManagementPresenter, ProductManagementPresenter>();
            services.AddTransient<IIngredientDetailPresenter, IngredientDetailPresenter>();
            services.AddTransient<IIngredientManagementPresenter, IngredientManagementPresenter>();
            services.AddTransient<IRecipeDetailPresenter, RecipeDetailPresenter>();
            services.AddTransient<ISupplierManagementPresenter, SupplierManagementPresenter>();
            services.AddTransient<ISupplierDetailPresenter, SupplierDetailPresenter>();

            services.AddTransient<EmployeeManagementPresenter>();

            services.AddTransient<ProductManagementPresenter>();
            services.AddTransient<RecipeManagementPresenter>();
            services.AddTransient<IngredientManagementPresenter>();
            services.AddTransient<ProductDetailPresenter>();
            services.AddTransient<IngredientDetailPresenter>();
            services.AddTransient<SupplierManagementPresenter>();
            services.AddTransient<SupplierDetailPresenter>();

            services.AddScoped<ITokenCleanupService, TokenCleanupService>();
            services.AddHostedService<TokenCleanupBackgroundService>();

            services.AddTransient<FrmLandingDashboard>();
            services.AddTransient<FrmEmployeeManagement>();
            services.AddTransient<FrmBaseMdiWithSidePanel>();
            services.AddTransient<FrmProductManagement>();
            services.AddTransient<FrmRecipeDetails>();
            services.AddTransient<FrmProductDetails>();
            services.AddTransient<FrmEmployeeDetails>();
            services.AddTransient<FrmIngredientDetails>();
            services.AddTransient<FrmIngredientManagement>();
            services.AddTransient<FrmUserManagement>();
            services.AddTransient<FrmRolePermissionManagement>();
            services.AddTransient<FrmScheduleEditor>();
            services.AddTransient<FrmLogin>();
            services.AddTransient<FrmRecipeDetails>();
            services.AddTransient<FrmSupplierManagement>();
            services.AddTransient<FrmSupplierDetails>();

            var securitySection = configuration.GetSection("Security");
            services.Configure<SecurityOptions>(securitySection);
            services.Configure<JwtSettings>(securitySection.GetSection("Jwt"));

            services.AddSingleton(sp =>
            {
                var jwtOptions = sp.GetRequiredService<IOptions<JwtSettings>>().Value;
                return new JwtTokenService(jwtOptions);
            });

            services.AddSingleton<DataEncryptionHelper>();

            services.AddHttpClient<ProductService>();
            return services;
        }
    }
}
