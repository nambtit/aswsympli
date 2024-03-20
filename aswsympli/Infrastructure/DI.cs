using Application.Abstraction;
using Infrastructure.DB;
using Infrastructure.Factories;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DI
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IApplicationDb, InMemStorage>();
            services.AddScoped<ISearchHttpClientFactory, SearchHttpClientFactory>();

            services.AddScoped<IGoogleSearchDataService, GoogleSearchDataService>()
                    .AddScoped<IBingSearchDataService, BingSearchDataService>();

            return services;
        }
    }
}