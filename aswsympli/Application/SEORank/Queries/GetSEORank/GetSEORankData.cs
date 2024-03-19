using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SEORank.Queries.GetSEORank
{
    public class GetSEORankData
    {
        public void ABC()
        {
            const char detectSectionEndPattern = '"';
            const string detectPattern = "<a href=\"/url?q=https://sympli.com.au";
            var detectStartSectionIndex = detectPattern.LastIndexOf('=');

            var detectPatternIndex = 0;
            var sectionStart = false;
            var sectionEnd = false;
            var currentSectionIndex = -1;
            var foundAtIndexes = new Queue<int>();
            var tmpCharQ = new Queue<char>();
            var file = string.Empty;

            using (var jsonStream = new StreamReader(file))
            {
                while (!jsonStream.EndOfStream)
                {
                    var c = (char)jsonStream.Read();

                    // For debug.
                    tmpCharQ.Enqueue(c);
                    var tmps = string.Join(null, tmpCharQ);

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



                    if (currentSectionIndex > -1)
                    {
                        Console.WriteLine($"Section {currentSectionIndex}");
                    }








                }
            }
        }
    }
}
