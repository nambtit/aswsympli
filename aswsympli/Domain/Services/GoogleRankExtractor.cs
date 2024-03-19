using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Services
{
    public class GoogleRankExtractor : ISeoRankExtractor
    {
        public IEnumerable<SeoRecord> Extract(string companyUrl, StreamReader resultStream)
        {
            const char detectSectionEndPattern = '"';
            var simpliedUrl = companyUrl
                .Replace("https://", string.Empty)
                .Replace("http://", string.Empty)
                .Replace("www.", ".");

            var detectPattern = $"<a href=\"/url?q={simpliedUrl}";
            var detectStartSectionIndex = detectPattern.LastIndexOf('=');

            var detectPatternIndex = 0;
            var sectionStart = false;
            var sectionEnd = false;
            var currentSectionIndex = -1;
            var foundAtIndexes = new Queue<int>();

            var result = new List<SeoRecord>();

            while (!resultStream.EndOfStream)
            {
                var c = (char)resultStream.Read();

                if (c == detectPattern[detectPatternIndex])
                {
                    if (!sectionEnd && !sectionStart && detectPatternIndex == detectStartSectionIndex)
                    {
                        sectionStart = true;
                        currentSectionIndex++;
                    }

                    detectPatternIndex++;
                }
                else
                {
                    if (!sectionStart)
                    {
                        detectPatternIndex = 0;
                    }

                    if (sectionStart && c == detectSectionEndPattern)
                    {
                        sectionEnd = true;
                    }
                }

                if (sectionStart && detectPatternIndex == detectPattern.Length - 1)
                {
                    foundAtIndexes.Enqueue(currentSectionIndex);
                    sectionEnd = true;
                }

                if (sectionEnd)
                {
                    sectionStart = false;
                    sectionEnd = false;
                    detectPatternIndex = 0;
                }
            }

            if (!foundAtIndexes.Any())
            {
                return Enumerable.Empty<SeoRecord>();
            }

            var nowUTC = DateTime.UtcNow;
            return foundAtIndexes.Select(i => new SeoRecord
            {
                CompanyUrl = companyUrl,
                Rank = i,
                RecordedAtUtc = nowUTC,
                SearchEngine = SearchEngineEnum.Google
            });
        }
    }
}