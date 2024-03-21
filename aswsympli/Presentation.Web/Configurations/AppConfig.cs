using Application.Abstraction;

namespace Presentation.Web.Configurations
{
    public class AppConfig : IApplicationConfig
    {
        public const string SectionName = "AppConfig";

        public IEnumerable<string> SearchKeywords { get; set; }

        public string CompanyUrl { get; set; }

        public int SEORankDataRefreshFrequencyMinutes { get; set; }
    }
}