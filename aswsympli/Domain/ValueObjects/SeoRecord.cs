using Domain.Enums;

namespace Domain.ValueObjects
{
    public record SEORecord
    {
        public SearchEngineEnum SearchEngine { get; set; }

        public int Rank { get; set; }

        public DateTime RecordedAtUtc { get; set; }
    }
}
