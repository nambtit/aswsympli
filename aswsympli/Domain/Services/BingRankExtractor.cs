using CoreUtils.DateTime;
using Domain.ValueObjects;

namespace Domain.Services
{
    public class BingRankExtractor : PatternBasedRankExtractor, IBingSEORankExtractor
    {
        public BingRankExtractor(IDateTimeService dateTimeService) : base(dateTimeService)
        {
        }

        public RankExtractResult Extract(string companyUrl, StreamReader resultStream)
        {
            var simpliedUrl = new Uri(companyUrl).Host.Replace("www.", string.Empty);

            // A section for a result listed will be marked-up with this pattern. Used for filtering the incoming stream of characters.
            var detectPattern = $"<div class=\"b_attribution\"><cite>{simpliedUrl}</cite>";

            // If the stream reach this far in the filter, we're in a section.
            var detectStartSectionIndex = detectPattern.IndexOf("\"><cite");

            // A section can be considered end with this tag.
            var sdetectSectionEndPattern = "</cite>";

            var result = ExtractWithOptions(resultStream, options =>
            {
                options.DetectPattern = detectPattern;
                options.DetectSectionEndPattern = sdetectSectionEndPattern;
                options.SectionStartIndex = detectStartSectionIndex;
            });

            result.Engine = Enums.SearchEngineEnum.Bing;

            return result;
        }
    }
}
