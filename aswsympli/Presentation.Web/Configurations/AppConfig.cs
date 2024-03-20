using Application.Abstraction;

namespace Presentation.Web.Configurations
{
    public class AppConfig : IApplicationConfig
    {
        public IEnumerable<string> SearchKeywords => new[] { "e-settlements" };

        public string CompanyUrl => "https://www.sympli.com.au";
    }
}
