using Application.Abstraction;
using Presentation.Web.Configurations;
using Presentation.Web.Services;

namespace Presentation.Web
{
    public static class DI
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var appConfig = new AppConfig();
            configuration.GetSection(AppConfig.SectionName).Bind(appConfig);
            services.AddSingleton<IApplicationConfig>(sp => appConfig);

            services.AddHostedService<HostedRankDataUpdateService>();

            return services;
        }
    }
}