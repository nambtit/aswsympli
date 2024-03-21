using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Services
{
    public class PatternBasedExtractOptions
    {
        public string DetectPattern { get; set; }
        public string DetectSectionEndPattern { get; set; }
        public int SectionStartIndex { get; set; }
    }

    public abstract class PatternBasedRankExtractor
    {
        public RankExtractResult ExtractWithOptions(StreamReader resultStream, Action<PatternBasedExtractOptions> optionBuilder)
        {
            var options = new PatternBasedExtractOptions();
            optionBuilder(options);

            // A section for a result listed will be marked-up with this pattern. Used for filtering the incoming stream of characters.
            var detectPattern = options.DetectPattern;

            // If the stream reach this far in the filter, we're in a section.
            var detectStartSectionIndex = options.SectionStartIndex;

            // A section can be considered end with this tag.
            var sdetectSectionEndPattern = options.DetectSectionEndPattern;

            var detectPatternIndex = 0;
            var detectSectionEndIndex = 0;

            var sectionStarted = false;
            var sectionEnded = false;
            var currentSectionIndex = -1;
            var foundAtIndexes = new Queue<int>();

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

            var result = new RankExtractResult();
            result.RecordedAtUtc = DateTime.UtcNow;
            result.Ranks = foundAtIndexes.Order();
            result.TotalResults = currentSectionIndex + 1;

            return result;
        }

        public IEnumerable<SEORecord> ExtractWithOptionsMany(StreamReader resultStream, Action<PatternBasedExtractOptions> optionBuilder)
        {
            var options = new PatternBasedExtractOptions();
            optionBuilder(options);

            // A section for a result listed will be marked-up with this pattern. Used for filtering the incoming stream of characters.
            var detectPattern = options.DetectPattern;

            // If the stream reach this far in the filter, we're in a section.
            var detectStartSectionIndex = options.SectionStartIndex;

            // A section can be considered end with this tag.
            var sdetectSectionEndPattern = options.DetectSectionEndPattern;

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
