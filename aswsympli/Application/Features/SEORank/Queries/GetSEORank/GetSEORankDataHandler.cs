using Application.Abstraction;
using Application.Models;
using Domain.Enums;

namespace Application.Features.SEORank.Queries.GetSEORank
{
    public interface IGetSEORankDataHandler
    {
        Task<IEnumerable<SearchRankData>> HandleAsync();
    }

    public class GetSEORankDataHandler: IGetSEORankDataHandler
    {
        private readonly IApplicationDb _applicationStorage;

        public GetSEORankDataHandler(IApplicationDb applicationStorage)
        {
            _applicationStorage = applicationStorage;
        }

        public async Task<IEnumerable<SearchRankData>> HandleAsync()
        {
            var googleData = _applicationStorage.GetRankDataByEngineAsync(SearchEngineEnum.Google);

            var bingData = _applicationStorage.GetRankDataByEngineAsync(SearchEngineEnum.Bing);

            return await Task.WhenAll(googleData, bingData);
        }
    }
}
