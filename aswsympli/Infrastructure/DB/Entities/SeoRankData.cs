using Infrastructure.Enums;

namespace Infrastructure.DB.Entities
{
    public class SeoRankData
    {
        public int Id { get; set; }

        public ICollection<int> Ranks { get; set; }

        public string Keyword { get; set; }

        public string CompanyUrl { get; set; }

        public SearchEngineDbEnum Engine { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
