using Application.Models;
using Domain.Enums;

namespace Application.Abstraction
{
    public interface IApplicationStorage
    {
        Task<SearchRankData> GetRankDataByEngineAsync(SearchEngineEnum fromEngine);
    }
}
