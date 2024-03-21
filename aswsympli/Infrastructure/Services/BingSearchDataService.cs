﻿using Application.Abstraction;
using Domain.Enums;

namespace Infrastructure.Services
{
    public class BingSearchDataService : IBingSearchDataService
    {
        private readonly HttpClient _httpClient;

        public BingSearchDataService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public Task<Stream> GetSearchDataStreamAsync(SearchEngineEnum searchEngine, string keyword)
        {
            throw new NotImplementedException();
        }

        public async Task<Stream> GetSearchDataStreamAsync(string keyword)
        {
            const int maxResults = 10;
            var searchUrl = $"https://www.google.com/search?q={keyword}&num={maxResults}&hl=en";

            var sjson = await _httpClient.GetStreamAsync(searchUrl);

            return sjson;
        }

        public async IAsyncEnumerable<Stream> GetSearchDataStreamChunkAsync(string keyword, CancellationToken token)
        {
            const int maxResults = 10;
            var searchUrl = $"https://www.google.com/search?q={keyword}&num={maxResults}&hl=en";

            foreach (var a in new string[] { searchUrl, searchUrl })
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                var sjson = await _httpClient.GetStreamAsync(a, token);
                yield return sjson;
            }
        }
    }
}
