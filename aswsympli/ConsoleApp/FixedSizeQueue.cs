using System.Collections.Concurrent;

namespace ConsoleApp
{
    public class FixedSizedQueue<T>
    {
        public FixedSizedQueue(int size)
        {
            _size = size;
        }

        private readonly ConcurrentQueue<T> q = new ConcurrentQueue<T>();
        private object lockObject = new object();

        public int Limit => q.Count;
        private int _size;

        public void Enqueue(T obj)
        {
            q.Enqueue(obj);
            if (q.Count > _size)
            {
                lock (lockObject)
                {
                    while (q.Count > _size && q.TryDequeue(out _)) ;
                }
            }
        }

        public string GetString()
        {
            return string.Join(null, q);
        }
    }

    public static class Rules
    {
        public static readonly SearchExtractionRuleset StartSectionRule = new SearchExtractionRuleset(
            pattern: "<a href=\"/url?q=",
            acceptedChars: new HashSet<char>(),
            resetChars: new HashSet<char>(),
            ignoredChars: new HashSet<char>());

        public static readonly SearchExtractionRuleset SeoFoundRule = new SearchExtractionRuleset(
            pattern: "https://www.firsttitle.com.au",
            acceptedChars: new HashSet<char>(),
            resetChars: new HashSet<char>(),
            ignoredChars: new HashSet<char> { });

        public static readonly SearchExtractionRuleset EndSectionRule = new SearchExtractionRuleset(
            pattern: "</div></div></div></div></div></div></div>",
            acceptedChars: new HashSet<char>(),
            resetChars: new HashSet<char>(),
            ignoredChars: new HashSet<char> { });
    }
}

public class SearchExtractionRuleset
{
    public SearchExtractionRuleset(string pattern, HashSet<char> acceptedChars, HashSet<char> resetChars, HashSet<char> ignoredChars)
    {
        Pattern = pattern;
        AcceptedChars = acceptedChars;
        ResetChars = resetChars;
        IgnoredChars = ignoredChars;
    }

    public string Pattern { get; }

    public HashSet<char> AcceptedChars { get; }

    public HashSet<char> IgnoredChars { get; }

    public HashSet<char> ResetChars { get; }
}





//<a href="/url?q=https://www.firsttitle.com.au/blogs/4-things-you-should-know-about-electronic-settleme/&amp;sa=U&amp;ved=2ahUKEwjH6MGWmP6EAxVRna8BHXArDxYQFnoECF0QAg&amp;usg=AOvVaw2W5rxUL_GU_w7UK7rXvbJ3" data-ved="2ahUKEwjH6