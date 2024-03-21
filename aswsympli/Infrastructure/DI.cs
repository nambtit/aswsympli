using System.Net.Http;
using System.Net.Http.Headers;
using Application.Abstraction;
using Infrastructure.DB;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DI
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IGoogleSearchDataService, GoogleSearchDataService>(client =>
            {
                client.BaseAddress = new Uri("https://www.google.com");
                SetHttpClientHeaders(client);
            });

            services.AddHttpClient<IBingSearchDataService, BingSearchDataService>(client =>
            {
                client.BaseAddress = new Uri("https://www.bing.com");
                SetHttpClientHeaders(client);
            });

            services.AddSingleton<IApplicationDb, InMemStorage>();

            return services;
        }

        private static void SetHttpClientHeaders(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true
            };

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.8");
            httpClient.DefaultRequestHeaders.Add("Content-Security-Policy", "sandbox;");
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");

            //httpClient.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36");
        }
    }
}