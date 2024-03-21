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
            var topNResults = _applicationConfig.TotalSearchResultsToLookup;
            var relativePageSize = _applicationConfig.MaxResultPerPage;

            foreach (var kw in keywords)
            {
                var totalResults = 300;
                var pageIndex = 0;
                var googleRankData = new SearchRankData(AppSearchEngineEnum.Google);
                var bingRankData = new SearchRankData(AppSearchEngineEnum.Bing);

                var cts = new CancellationTokenSource();

                while (totalResults < topNResults)
                {
                    var pageSize = (topNResults - totalResults) > relativePageSize ? relativePageSize : topNResults - totalResults;
                    await using (var googleData = await _googleSearchDataService.GetSearchDataStreamAsync(kw, pageSize, totalResults, cts.Token))
                    {
                        using var reader = new StreamReader(googleData);
                        var rankData = _googleSEORankExtractor.Extract(companyUrl, reader);
                        var currentIndex = totalResults > 0 ? totalResults - 1 : 0;

                        MergeSearchRankData(googleRankData, rankData, currentIndex);
                        totalResults += rankData.TotalResults;
                    }

                    pageIndex++;
                }

                CleaningRankData(googleRankData, topNResults - 1);
                await _applicationStorage.UpdateRankDataByEngineAsync(AppSearchEngineEnum.Google, googleRankData);
                pageIndex = 0;
                totalResults = 0;
                cts = new CancellationTokenSource();

                while (totalResults < topNResults)
                {
                    var pageSize = (topNResults - totalResults) > relativePageSize ? relativePageSize : topNResults - totalResults;
                    await using (var bingData = await _bingSearchDataService.GetSearchDataStreamAsync(kw, pageSize, totalResults, cts.Token))
                    {
                        using var reader = new StreamReader(bingData);
                        var rankData = _bingSEORankExtractor.Extract(companyUrl, reader);
                        var currentIndex = totalResults > 0 ? totalResults - 1 : 0;

                        MergeSearchRankData(bingRankData, rankData, currentIndex);
                        totalResults += rankData.TotalResults;
                    }

                    pageIndex++;
                }

                CleaningRankData(bingRankData, topNResults - 1);
                await _applicationStorage.UpdateRankDataByEngineAsync(AppSearchEngineEnum.Bing, bingRankData);
            }
        }

        private void MergeSearchRankData(SearchRankData origin, RankExtractResult result, int currentIndex)
        {
            if (result == null || result.Ranks == null || !result.Ranks.Any())
            {
                return;
            }

            var ranks = result.Ranks.Select(e => e + currentIndex);

            origin.Ranks = origin.Ranks.Concat(ranks).Order();
            origin.RecordedAtUTC = result.RecordedAtUtc;
        }

        private void CleaningRankData(SearchRankData data, int maxResultIndex)
        {
            data.Ranks = data.Ranks.Distinct().Where(e => e <= maxResultIndex).Order();
        }
    }
}
