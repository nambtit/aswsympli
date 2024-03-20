using Application.Abstraction;
using Presentation.Web.Configurations;
using Presentation.Web.Services;

namespace Presentation.Web
{
    public static class DI
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IApplicationConfig, AppConfig>();
            services.AddSingleton<RankDataUpdateService>();

            return services;
        }
    }
}