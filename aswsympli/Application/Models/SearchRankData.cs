using Application.Features.SEORank.Enums;

namespace Application.Models
{
    public class SearchRankData
    {
        public SearchRankData(AppSearchEngineEnum engine, DateTime recordedAtUtc, params int[] ranks)
        {
            Engine = engine;
            Ranks = ranks;
            RecordedAtUTC = recordedAtUtc;
        }

        public AppSearchEngineEnum Engine { get; init; }

        public IEnumerable<int> Ranks { get; init; }

        public DateTime RecordedAtUTC { get; init; }
    }
}
