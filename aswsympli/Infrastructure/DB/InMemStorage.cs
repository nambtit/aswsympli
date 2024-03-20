using Application.Abstraction;
using Application.Models;
using Domain.Enums;

namespace Infrastructure.DB
{
    public class InMemStorage : IApplicationDb
    {
        public Task<SearchRankData> GetRankDataByEngineAsync(SearchEngineEnum fromEngine)
        {
            return Task.FromResult(new SearchRankData(DateTime.UtcNow, 1, 2, 3, 5, 7));
        }

        public Task UpdateRankDataByEngineAsync(SearchEngineEnum engine, SearchRankData data)
        {
            throw new NotImplementedException();
        }
    }
}
