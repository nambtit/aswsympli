using Application.Abstraction;
using Application.Models;
using Domain.Enums;

namespace Infrastructure.DB
{
    public class InMemStorage : IApplicationDb
    {
        private readonly Dictionary<SearchEngineEnum, SearchRankData> _storage;

        public InMemStorage()
        {
            _storage = new Dictionary<SearchEngineEnum, SearchRankData>();
        }

        public Task<SearchRankData> GetRankDataByEngineAsync(SearchEngineEnum fromEngine)
        {
            switch (fromEngine)
            {
                case SearchEngineEnum.Bing:
                    {
                        return Task.FromResult(new SearchRankData(Application.Features.SEORank.Enums.SearchEngineEnum.Bing, DateTime.UtcNow, 1, 2, 3, 5, 7));
                    }
                case SearchEngineEnum.Google:
                    {
                        return Task.FromResult(new SearchRankData(Application.Features.SEORank.Enums.SearchEngineEnum.Google, DateTime.UtcNow, 0, 9, 11));
                    }

                default:
                    {
                        throw new Exception($"Not supported engine {fromEngine}");
                    }
            }
        }

        public Task UpdateRankDataByEngineAsync(SearchEngineEnum engine, SearchRankData data)
        {
            _storage.TryAdd(engine, data);
            return Task.CompletedTask;
        }
    }
}
