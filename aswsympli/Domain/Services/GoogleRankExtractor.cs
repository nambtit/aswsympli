using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Services
{
    public class GoogleRankExtractor : IGoogleSEORankExtractor
    {
        public IEnumerable<SEORecord> Extract(string companyUrl, StreamReader resultStream)
        {
            var simpliedUrl = new Uri(companyUrl).Host.Replace("www.", string.Empty);

            // A section for a result listed will be marked-up with this pattern. Used for filtering the incoming stream of characters.
            var detectPattern = $"<div class=\"notranslate\"><cite class=\"\" role=\"text\">{simpliedUrl}</cite>";

            // If the stream reach this far in the filter, we're in a section.
            var detectStartSectionIndex = detectPattern.IndexOf("\"><cite");

            // A section can be considered end with this tag.
            var sdetectSectionEndPattern = "</cite>";

            var detectPatternIndex = 0;
            var detectSectionEndIndex = 0;

            var sectionStarted = false;
            var sectionEnded = false;
            var currentSectionIndex = -1;
            var foundAtIndexes = new Queue<int>();

            var result = new List<SEORecord>();

            while (!resultStream.EndOfStream)
            {
                var c = (char)resultStream.Read();

                // When within a section, track if there is a closing tag of the section for flagging it.
                if (sectionStarted)
                {
                    if (c == sdetectSectionEndPattern[detectSectionEndIndex])
                    {
                        detectSectionEndIndex++;

                        if (detectSectionEndIndex == sdetectSectionEndPattern.Length - 1)
                        {
                            sectionEnded = true;
                        }
                    }
                    else
                    {
                        detectSectionEndIndex = 0;
                    }
                }

                // The stream of characters keep coming thru our filtering pattern. Only increase the matching index
                // when it matches, else reset the index IF we're not currently within a section (because when inside a section
                // there are noisy markups that can be ignored and keep the tracking).
                if (!sectionEnded && !sectionStarted && detectPatternIndex == detectStartSectionIndex)
                {
                    sectionStarted = true;
                    currentSectionIndex++;
                }

                if (c == detectPattern[detectPatternIndex])
                {
                    detectPatternIndex++;
                }
                else if (!sectionStarted)
                {
                    detectPatternIndex = 0;
                }

                if (sectionStarted && detectPatternIndex == detectPattern.Length - 1)
                {
                    foundAtIndexes.Enqueue(currentSectionIndex);
                    sectionEnded = true;
                }

                if (sectionEnded)
                {
                    sectionStarted = false;
                    sectionEnded = false;
                    detectPatternIndex = 0;
                    detectSectionEndIndex = 0;
                }
            }

            var nowUTC = DateTime.UtcNow;
            return foundAtIndexes.Select(i => new SEORecord
            {
                Rank = i,
                RecordedAtUtc = nowUTC,
                SearchEngine = SearchEngineEnum.Google
            });
        }
    }
}