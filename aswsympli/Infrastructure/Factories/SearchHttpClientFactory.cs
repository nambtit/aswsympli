namespace Infrastructure.Factories
{
    public class SearchHttpClientFactory : ISearchHttpClientFactory
    {
        public HttpClient CreateSearchHttpClient()
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.8");
            httpClient.DefaultRequestHeaders.Add("Content-Security-Policy", "sandbox;");
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            httpClient.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");

            return httpClient;
        }
    }
}
