using Application.Features.SEORank.Enums;
using Application.Models;

namespace Application.Abstraction
{
    public interface IApplicationDb
    {
        Task<SearchRankData> GetRankDataByEngineAsync(AppSearchEngineEnum fromEngine);

        Task UpdateRankDataByEngineAsync(AppSearchEngineEnum engine, SearchRankData data);
    }
}
