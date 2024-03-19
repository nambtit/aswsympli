namespace Application.Models
{
    public class SearchRankData
    {
        public SearchRankData(DateTime recordedAtUtc, params int[] ranks)
        {
            Ranks = ranks;
            RecordedAtUTC = recordedAtUtc;
        }

        public IEnumerable<int> Ranks { get; init; }

        public DateTime RecordedAtUTC { get; init; }
    }
}
