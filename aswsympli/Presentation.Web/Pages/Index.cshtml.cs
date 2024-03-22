using Application.Features.SEORank.Enums;
using Application.Features.SEORank.Queries.GetSEORank;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages
{
	public class IndexModel : PageModel
	{
		private readonly IGetSEORankDataHandler _handler;

		public IndexModel(ILogger<IndexModel> logger, IGetSEORankDataHandler handler)
		{
			_handler = handler;
		}

		public async Task OnGet()
		{
			var rankData = await _handler.HandleAsync();

			if (rankData == null || !rankData.Any())
			{
				Data = Enumerable.Empty<RankDataViewModel>();
				return;
			}

			var tmp = new List<RankDataViewModel>();

			foreach (var e in rankData.GroupBy(e => e.Keyword))
			{
				var d = new RankDataViewModel
				{
					Keyword = e.Key,
					CompanyUrl = e.FirstOrDefault(e => !string.IsNullOrWhiteSpace(e.CompanyUrl)).CompanyUrl ?? string.Empty,
					BingRanks = e.FirstOrDefault(e => e.Engine == AppSearchEngineEnum.Bing)?.Ranks ?? new int[0].Order(),
					GoogleRanks = e.FirstOrDefault(e => e.Engine == AppSearchEngineEnum.Google)?.Ranks ?? new int[0].Order(),
					RecordedAtUTC = e.FirstOrDefault().RecordedAtUTC,
				};

				tmp.Add(d);
			}

			Data = tmp;
		}

		public IEnumerable<RankDataViewModel> Data { get; set; }
	}

	public class RankDataViewModel
	{
		public IOrderedEnumerable<int> GoogleRanks { get; set; }

		public IOrderedEnumerable<int> BingRanks { get; set; }

		public string Keyword { get; set; }

		public string CompanyUrl { get; set; }

		public DateTime RecordedAtUTC { get; set; }
	}
}

