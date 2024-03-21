using System.Collections.Concurrent;
using Application.Abstraction;
using Application.Features.SEORank.Enums;
using Application.Models;

namespace Infrastructure.DB
{
    public class InMemStorage : IApplicationDb
    {
        private readonly ConcurrentDictionary<AppSearchEngineEnum, SearchRankData> _storage;

        public InMemStorage()
        {
            _storage = new ConcurrentDictionary<AppSearchEngineEnum, SearchRankData>();
        }

        public Task<SearchRankData> GetRankDataByEngineAsync(AppSearchEngineEnum fromEngine)
        {
            if (!_storage.TryGetValue(fromEngine, out var rankData))
            {
                return Task.FromResult(new SearchRankData(fromEngine, DateTime.UtcNow, Enumerable.Empty<int>().Order()));
            }

            return Task.FromResult(rankData);
        }

        public Task UpdateRankDataByEngineAsync(AppSearchEngineEnum engine, SearchRankData data)
        {
            _storage.AddOrUpdate(engine, data, (engine, data) => data);

            return Task.CompletedTask;
        }
    }
}
