using Application.Abstraction;
using Domain.Enums;
using Domain.Services;

namespace Application.Features.SEORank.Commands.UpdateSEORank
{
    public class UpdateSEORankData
    {
        private readonly IApplicationStorage _applicationStorage;
        private readonly ISearchDataRepository _searchDataRepository;
        private readonly ISEORankExtractor _seoRankExtractor;
        private readonly IApplicationConfig _applicationConfig;

        public UpdateSEORankData(IApplicationStorage applicationStorage,
            ISearchDataRepository searchDataRepository,
            ISEORankExtractor seoRankExtractor,
            IApplicationConfig applicationConfig)
        {
            _applicationStorage = applicationStorage;
            _searchDataRepository = searchDataRepository;
            _seoRankExtractor = seoRankExtractor;
            _applicationConfig = applicationConfig;
        }

        public async Task Handle()
        {
            //const string keyword = "\"e-settlements\"";
            //const string companyUrl = "https://www.sympli.com.au";

            var keywords = _applicationConfig.SearchKeywords;
            var companyUrl = _applicationConfig.CompanyUrl;

            foreach (var kw in keywords)
            {
                await using (var googleData = await _searchDataRepository.GetSearchDataStreamAsync(SearchEngineEnum.Google, kw))
                {
                    using var reader = new StreamReader(googleData);
                    var googleRankData = _seoRankExtractor.Extract(companyUrl, reader);

                    await _applicationStorage.UpdateRankDataByEngineAsync(SearchEngineEnum.Google, null);
                }

                await using (var bingData = await _searchDataRepository.GetSearchDataStreamAsync(SearchEngineEnum.Bing, kw))
                {
                    using var reader = new StreamReader(bingData);
                    var bingRankData = _seoRankExtractor.Extract(companyUrl, reader);

                    await _applicationStorage.UpdateRankDataByEngineAsync(SearchEngineEnum.Bing, null);
                }
            }
        }
    }
}
