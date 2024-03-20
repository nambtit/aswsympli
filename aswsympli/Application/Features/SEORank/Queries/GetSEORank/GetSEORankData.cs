using Application.Abstraction;
using Application.Models;
using Domain.Enums;

namespace Application.Features.SEORank.Queries.GetSEORank
{
    public class GetSEORankData
    {
        private readonly IApplicationStorage _applicationStorage;

        public GetSEORankData(IApplicationStorage applicationStorage)
        {
            _applicationStorage = applicationStorage;
        }

        public async Task<IEnumerable<SearchRankData>> Handle()
        {
            var googleData = _applicationStorage.GetRankDataByEngineAsync(SearchEngineEnum.Google);
            var bingData = _applicationStorage.GetRankDataByEngineAsync(SearchEngineEnum.Bing);

            return await Task.WhenAll(googleData, bingData);
        }
    }
}
