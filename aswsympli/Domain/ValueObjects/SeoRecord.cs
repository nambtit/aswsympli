using Domain.Enums;

namespace Domain.ValueObjects
{
    public record SEORecord
    {
        public SearchEngineEnum SearchEngine { get; set; }

        public int Rank { get; set; }

        public DateTime RecordedAtUtc { get; set; }
    }

    public record RankExtractResult
    {
        /// <summary>
        /// Total number of results found from the input stream. It can be other websites or the input one.
        /// </summary>
        public int TotalResults { get; set; }

        public SearchEngineEnum Engine { get; set; }

        public DateTime RecordedAtUtc { get; set; }

        public IOrderedEnumerable<int> Ranks { get; set; }
    }
}
