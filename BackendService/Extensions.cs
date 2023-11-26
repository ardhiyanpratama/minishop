using BackendService.Application.Core.IRepositories;
using BackendService.Application.Core.Repositories;
using BackendService.Data;
using BackendService.Infrastructure.Services;
using BackendService.Settings;
using CustomLibrary.Services;
using Microsoft.EntityFrameworkCore;

namespace ManagementUserService
{
    public static class Extensions
    {
        public static IServiceCollection AddApplicationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
            //services.Configure<JwtAuthSetting>(configuration.GetSection(nameof(JwtAuthSetting)));

            return services;
        }
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(builderOptions =>
            {
                builderOptions.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), options =>
                {
                    options.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(60), errorNumbersToAdd: null);
                });
            });
            return services;
        }
        public static IServiceCollection AddRequiredService(this IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IPrivateUserIdService, PrivateUserIdService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            //services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();

            return services;
        }
    }
}
