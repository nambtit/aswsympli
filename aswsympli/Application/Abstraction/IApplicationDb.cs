using Application.Models;
using Domain.Enums;

namespace Application.Abstraction
{
    public interface IApplicationDb
    {
        Task<SearchRankData> GetRankDataByEngineAsync(SearchEngineEnum fromEngine);
        Task UpdateRankDataByEngineAsync(SearchEngineEnum engine, SearchRankData data);
    }
}
