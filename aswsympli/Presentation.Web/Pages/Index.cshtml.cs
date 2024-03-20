using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            GoogleRanks = new[] { 2, 8, 9 };
            BingRanks = new[] { 0, 1, 3, 5, 6 };
            Keyword = "\"e-settlements\"";
            CompanyUrl = "https://www.sympli.com.au";
        }

        public IEnumerable<int> GoogleRanks { get; set; }
        public IEnumerable<int> BingRanks { get; set; }
        public string Keyword { get; set; }
        public string CompanyUrl { get; set; }
    }
}

