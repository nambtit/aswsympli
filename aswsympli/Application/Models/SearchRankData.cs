using Application.Features.SEORank.Enums;

namespace Application.Models
{
    public class SearchRankData
    {
        public SearchRankData(AppSearchEngineEnum engine, string keyword, string companyUrl)
        {
            Engine = engine;
            Ranks = Enumerable.Empty<int>().Order();
            Keyword = keyword;
            CompanyUrl = companyUrl;
        }

        public string Keyword { get; set; }

        public string CompanyUrl { get; set; }

        public AppSearchEngineEnum Engine { get; init; }

        public IOrderedEnumerable<int> Ranks { get; set; }

        public DateTime RecordedAtUTC { get; set; }
    }
}
