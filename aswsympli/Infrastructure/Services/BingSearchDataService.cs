using Application.Abstraction;

namespace Infrastructure.Services
{
    public class BingSearchDataService : IBingSearchDataService
    {
        private readonly HttpClient _httpClient;

        public BingSearchDataService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<Stream> GetSearchDataStreamAsync(string keyword, int pageSize, int skip, CancellationToken token)
        {
            var searchUrl = $"/search?q={keyword}&count={pageSize}&first={skip}&setLang=en-AU";
            return await _httpClient.GetStreamAsync(searchUrl, token);
        }
    }
}