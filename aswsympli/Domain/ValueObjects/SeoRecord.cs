using Domain.Enums;

namespace Domain.ValueObjects
{
    public record SeoRecord
    {
        public SearchEngineEnum SearchEngine { get; set; }

        public string CompanyUrl { get; set; }

        public string Keyword { get; set; }

        public int Rank { get; set; }

        public DateTime RecordedAtUtc { get; set; }
    }
}
