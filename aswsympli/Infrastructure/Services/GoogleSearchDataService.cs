using Application.Abstraction;
using Domain.Enums;
using Infrastructure.Factories;

namespace Infrastructure.Services
{
    public class GoogleSearchDataService : IGoogleSearchDataService
    {
        private readonly ISearchHttpClientFactory _httpClientFactory;

        public GoogleSearchDataService(ISearchHttpClientFactory factory)
        {
            _httpClientFactory = factory;
        }

        public Task<Stream> GetSearchDataStreamAsync(SearchEngineEnum searchEngine, string keyword)
        {
            throw new NotImplementedException();
        }

        public async Task<Stream> GetSearchDataStreamAsync(string keyword)
        {
            var httpClient = _httpClientFactory.CreateSearchHttpClient();

            const int maxResults = 10;
            var searchUrl = $"https://www.google.com/search?q={keyword}&num={maxResults}&hl=en";

            var sjson = await httpClient.GetStreamAsync(searchUrl);

            return sjson;
        }

        public async IAsyncEnumerable<Stream> GetSearchDataStreamChunkAsync(string keyword, CancellationToken token)
        {
            var httpClient = _httpClientFactory.CreateSearchHttpClient();

            const int maxResults = 10;
            var searchUrl = $"https://www.google.com/search?q={keyword}&num={maxResults}&hl=en";

            foreach (var a in new string[] { searchUrl, searchUrl })
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                var sjson = await httpClient.GetStreamAsync(a, token);
                yield return sjson;
            }
        }
    }
}
