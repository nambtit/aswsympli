using Application.Features.SEORank.Enums;

namespace Application.Models
{
    public class SearchRankData
    {
        public SearchRankData(AppSearchEngineEnum engine)
        {
            Engine = engine;
            Ranks = Enumerable.Empty<int>().Order();
        }

        public AppSearchEngineEnum Engine { get; init; }

        public IOrderedEnumerable<int> Ranks { get; set; }

        public DateTime RecordedAtUTC { get; set; }
    }
}
