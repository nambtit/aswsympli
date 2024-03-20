using Application.Features.SEORank.Queries.GetSEORank;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IGetSEORankDataHandler _handler;

        public IndexModel(ILogger<IndexModel> logger, IGetSEORankDataHandler handler)
        {
            _logger = logger;
            _handler = handler;
        }

        public async Task OnGet()
        {
            var data = await _handler.HandleAsync();
            foreach (var item in data)
            {
                var s = item.ToString();
            }

            GoogleRanks = data.FirstOrDefault().Ranks;
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

