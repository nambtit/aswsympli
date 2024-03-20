using Application.Abstraction;
using Application.Features.SEORank.Commands.UpdateSEORank;
using Application.Features.SEORank.Queries.GetSEORank;
using Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DI
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IGetSEORankDataHandler, GetSEORankDataHandler>()
                    .AddScoped<IUpdateSEORankDataHandler, UpdateSEORankDataHandler>();

            services.AddScoped<GoogleRankExtractor>();
            services.AddScoped<BingRankExtractor>();
            services.AddScoped<IGoogleSEORankExtractor>(sp => (IGoogleSEORankExtractor)sp.GetService<GoogleRankExtractor>());
            services.AddScoped<IBingSEORankExtractor>(sp => (IBingSEORankExtractor)sp.GetService<GoogleRankExtractor>());

            return services;
        }
    }
}
