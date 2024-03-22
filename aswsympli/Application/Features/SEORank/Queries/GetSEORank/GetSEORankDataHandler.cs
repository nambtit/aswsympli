using Application.Abstraction;
using Application.Const;
using Application.Features.SEORank.Enums;
using Application.Models;

namespace Application.Features.SEORank.Queries.GetSEORank
{
    public interface IGetSEORankDataHandler
    {
        Task<IEnumerable<SearchRankData>> HandleAsync();
    }

    public class GetSEORankDataHandler : IGetSEORankDataHandler
    {
        private readonly IApplicationDb _applicationStorage;
        private readonly IAppDataCache _cache;

        public GetSEORankDataHandler(IApplicationDb applicationStorage, IAppDataCache cache)
        {
            _applicationStorage = applicationStorage;
            _cache = cache;
        }

        public async Task<IEnumerable<SearchRankData>> HandleAsync()
        {
            if (!_cache.TryGet<IEnumerable<SearchRankData>>(CacheKey.GoogleSeoRankData, out var googleData))
            {
                googleData = await _applicationStorage.GetLatestKeywordsRankDataByEngineAsync(AppSearchEngineEnum.Google);
                _cache.TrySet(CacheKey.GoogleSeoRankData, googleData);
            }

            if (!_cache.TryGet<IEnumerable<SearchRankData>>(CacheKey.BingSeoRankData, out var bingData))
            {
                bingData = await _applicationStorage.GetLatestKeywordsRankDataByEngineAsync(AppSearchEngineEnum.Bing);
                _cache.TrySet(CacheKey.BingSeoRankData, bingData);
            }

            return (googleData ?? []).Concat(bingData ?? []).Where(e => e != null);
        }
    }
}
