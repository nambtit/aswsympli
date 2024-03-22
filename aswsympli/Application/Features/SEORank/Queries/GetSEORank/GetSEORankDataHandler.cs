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
			if (!_cache.TryGet<SearchRankData>(CacheKey.GoogleSeoRankData, out var googleData))
			{
				googleData = await _applicationStorage.GetRankDataByEngineAsync(AppSearchEngineEnum.Google);
				_cache.TrySet<SearchRankData>(CacheKey.GoogleSeoRankData, googleData);
			}

			if (!_cache.TryGet<SearchRankData>(CacheKey.BingSeoRankData, out var bingData))
			{
				bingData = await _applicationStorage.GetRankDataByEngineAsync(AppSearchEngineEnum.Bing);
				_cache.TrySet<SearchRankData>(CacheKey.BingSeoRankData, bingData);
			}

			return new[] { googleData, bingData }.Where(e => e != null);
		}
	}
}
