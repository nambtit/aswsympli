using Application.Features.SEORank.Enums;

namespace Application.Models
{
    public class SearchRankData
    {
        public SearchRankData(AppSearchEngineEnum engine, DateTime recordedAtUtc, IOrderedEnumerable<int> ranks)
        {
            Engine = engine;
            Ranks = ranks;
            RecordedAtUTC = recordedAtUtc;
        }

        public AppSearchEngineEnum Engine { get; init; }

        public IOrderedEnumerable<int> Ranks { get; init; }

        public DateTime RecordedAtUTC { get; init; }
    }
}
