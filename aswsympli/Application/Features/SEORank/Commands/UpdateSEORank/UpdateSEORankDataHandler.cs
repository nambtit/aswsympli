using Application.Abstraction;
using Domain.Enums;
using Domain.Services;
using Microsoft.Extensions.Options;

namespace Application.Features.SEORank.Commands.UpdateSEORank
{
    public interface IUpdateSEORankDataHandler
    {
        Task HandleAsync();
    }

    public class UpdateSEORankDataHandler : IUpdateSEORankDataHandler
    {
        private readonly IApplicationDb _applicationStorage;
        private readonly IGoogleSearchDataService _googleSearchDataService;
        private readonly IBingSearchDataService _bingSearchDataService;
        private readonly IGoogleSEORankExtractor _googleSEORankExtractor;
        private readonly IBingSEORankExtractor _bingSEORankExtractor;
        private readonly IApplicationConfig _applicationConfig;

        public UpdateSEORankDataHandler(
            IApplicationDb applicationStorage,
            IGoogleSearchDataService googleSearchDataService,
            IBingSearchDataService bingSearchDataService,
            IGoogleSEORankExtractor googleSEORankExtractor,
            IBingSEORankExtractor bingSEORankExtractor,
            IApplicationConfig applicationConfig)
        {
            _applicationStorage = applicationStorage;
            _googleSearchDataService = googleSearchDataService;
            _bingSearchDataService = bingSearchDataService;
            _googleSEORankExtractor = googleSEORankExtractor;
            _bingSEORankExtractor = bingSEORankExtractor;
            _applicationConfig = applicationConfig;
        }

        public async Task HandleAsync()
        {
            //const string keyword = "\"e-settlements\"";
            //const string companyUrl = "https://www.sympli.com.au";

            var keywords = _applicationConfig.SearchKeywords;
            var companyUrl = _applicationConfig.CompanyUrl;

            foreach (var kw in keywords)
            {
                await using (var googleData = await _googleSearchDataService.GetSearchDataStreamAsync(kw))
                {
                    using var reader = new StreamReader(googleData);
                    var googleRankData = _googleSEORankExtractor.Extract(companyUrl, reader);

                    await _applicationStorage.UpdateRankDataByEngineAsync(SearchEngineEnum.Google, null);
                }

                await using (var bingData = await _bingSearchDataService.GetSearchDataStreamAsync(kw))
                {
                    using var reader = new StreamReader(bingData);
                    var bingRankData = _bingSEORankExtractor.Extract(companyUrl, reader);

                    await _applicationStorage.UpdateRankDataByEngineAsync(SearchEngineEnum.Bing, null);
                }
            }
        }
    }
}
