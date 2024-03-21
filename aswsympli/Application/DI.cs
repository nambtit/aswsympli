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

            services.AddScoped<IGoogleSEORankExtractor, GoogleRankExtractor>();
            services.AddScoped<IBingSEORankExtractor, BingRankExtractor>();

            return services;
        }
    }
}
