using Application.Abstraction;
using Application.Models;
using Domain.Enums;

namespace Infrastructure.Storage
{
    public class InMemStorage : IApplicationStorage
    {
        public Task<SearchRankData> GetRankDataByEngineAsync(SearchEngineEnum fromEngine)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRankDataByEngineAsync(SearchEngineEnum engine, SearchRankData data)
        {
            throw new NotImplementedException();
        }
    }
}
