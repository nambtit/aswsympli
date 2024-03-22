using Application.Abstraction;
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

		public GetSEORankDataHandler(IApplicationDb applicationStorage)
		{
			_applicationStorage = applicationStorage;
		}

		public async Task<IEnumerable<SearchRankData>> HandleAsync()
		{
			var googleData = _applicationStorage.GetRankDataByEngineAsync(AppSearchEngineEnum.Google);

			var bingData = _applicationStorage.GetRankDataByEngineAsync(AppSearchEngineEnum.Bing);

			var data = await Task.WhenAll(googleData, bingData);
			return data.Where(e => e != null);
		}
	}
}
