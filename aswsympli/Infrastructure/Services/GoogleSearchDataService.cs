using Application.Abstraction;

namespace Infrastructure.Services
{
    public class GoogleSearchDataService : IGoogleSearchDataService
    {
        private readonly HttpClient _httpClient;

        public GoogleSearchDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Stream> GetSearchDataStreamAsync(string keyword, int pageSize, int skip, CancellationToken token)
        {
            // https://aicontentfy.com/en/blog/demystifying-google-search-url-parameters-and-how-to-use-them
            var searchUrl = $"/search?q={keyword}&num={pageSize}&start={skip}&&hl=en";
            return await _httpClient.GetStreamAsync(searchUrl, token);
        }
    }
}