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

        public async Task<IEnumerable<SearchRankData>> GetAllSearchRankDataAsync()
        {
            var googleData = await _applicationStorage.GetRankDataByEngineAsync(SearchEngineEnum.Google);
            var bingData = await _applicationStorage.GetRankDataByEngineAsync(SearchEngineEnum.Bing);

            return [googleData, bingData];
        }
    }
}
