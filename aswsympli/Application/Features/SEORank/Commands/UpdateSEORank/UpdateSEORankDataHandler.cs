using Application.Abstraction;
using Application.Features.SEORank.Enums;
using Application.Models;
using Domain.Services;
using Domain.ValueObjects;

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
            var keywords = _applicationConfig.SearchKeywords;
            var companyUrl = _applicationConfig.CompanyUrl;

            foreach (var kw in keywords)
            {
                await using (var googleData = await _googleSearchDataService.GetSearchDataStreamAsync(kw))
                {
                    using var reader = new StreamReader(googleData);
                    var googleRankData = _googleSEORankExtractor.Extract(companyUrl, reader);

                    var data = ToSearchRankData(googleRankData);
                    await _applicationStorage.UpdateRankDataByEngineAsync(AppSearchEngineEnum.Google, data);
                }

                await using (var bingData = await _bingSearchDataService.GetSearchDataStreamAsync(kw))
                {
                    using var reader = new StreamReader(bingData);
                    var bingRankData = _bingSEORankExtractor.Extract(companyUrl, reader);

                    var data = ToSearchRankData(bingRankData);
                    await _applicationStorage.UpdateRankDataByEngineAsync(AppSearchEngineEnum.Bing, data);
                }
            }
        }

        private SearchRankData ToSearchRankData(IEnumerable<SEORecord> records)
        {
            if (records == null || !records.Any())
            {
                return null;
            }

            var d = records.First();
            var ranks = records.Select(e => e.Rank);
            var eng = d.SearchEngine == Domain.Enums.SearchEngineEnum.Google ? AppSearchEngineEnum.Google : AppSearchEngineEnum.Bing;

            return new SearchRankData(eng, d.RecordedAtUtc, ranks.ToArray());
        }
    }
}
