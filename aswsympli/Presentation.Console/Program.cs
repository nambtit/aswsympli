// See https://aka.ms/new-console-template for more information
using System.IO;
using Domain.Services;

static HttpClient SetHttpClientHeaders()
{
    var httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.Accept.Clear();
    httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.8");
    httpClient.DefaultRequestHeaders.Add("Content-Security-Policy", "sandbox;");
    httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
    httpClient.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
    httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36");
    return httpClient;
}


var httpClient = SetHttpClientHeaders();

const string keyword = @"""e-settlements""";
const int maxResults = 100;

var searchUrl = $"https://www.google.com/search?q={keyword}&num={maxResults}&hl=en";
var bingSearchUrl = $"https://www.bing.com/search?q={keyword}&count=100";
var file = Path.Combine(@"D:\tmp", "input.txt");

var getData = false;
var getBingData = false;

if (getBingData)
{
    var sjson = await httpClient.GetStringAsync(bingSearchUrl);
    await File.WriteAllTextAsync(Path.Combine(@"D:\tmp", "bingorigin.txt"), sjson);

    return;
}

if (getData)
{
    var sjson = await httpClient.GetStringAsync(searchUrl);
    await File.WriteAllTextAsync(Path.Combine(@"D:\tmp", "origin.txt"), sjson);

    return;
}

//var ex = new GoogleRankExtractor();
////var ex = new BingRankExtractor();
//using var tmp = new StreamReader(file);
//var r = ex.Extract("https://www.sympli.com.au", tmp);
//Console.WriteLine($"Engine: {r.Engine}, RecordedAtUtc: {r.RecordedAtUtc}, Ranks: {string.Join(",", r.Ranks)}, Total: {r.TotalResults}");
//return;


await using (var searchResultStream = await httpClient.GetStreamAsync(bingSearchUrl))
{
    //var ex = new GoogleRankExtractor();
    var ex = new BingRankExtractor();
    using var tmp = new StreamReader(searchResultStream);
    var r = ex.Extract("https://www.sympli.com.au", tmp);

    Console.WriteLine($"Engine: {r.Engine}, RecordedAtUtc: {r.RecordedAtUtc}, Ranks: {string.Join(",", r.Ranks)}, Total: {r.TotalResults}");
}



return;

//var json = await httpClient.GetStringAsync(searchUrl);

//var file = Path.Combine(@"D:\tmp", "google.txt");

var json = await File.ReadAllTextAsync(file);

//<a href="/url?q=https://www.sympli.com.au/&amp;sa=U&amp;ved=2ahUKEwj3gJLLm_-EAxXNSfUHHRhaBK8QFnoECEQQAg&amp;usg=AOvVaw3uNns9PSopuj-fTCffEv_u"

//const string detectSectionStartPattern = "<a href=\"/url?q=";
const char detectSectionEndPattern = '"';
const string detectPattern = "<a href=\"/url?q=https://sympli.com.au";
var detectStartSectionIndex = detectPattern.LastIndexOf('=');

var detectPatternIndex = 0;
var sectionStart = false;
var sectionEnd = false;
var currentSectionIndex = -1;
var foundAtIndexes = new Queue<int>();
var tmpCharQ = new Queue<char>();

//using (var jsonStream = new StreamReader(file))
using (var searchResultStream = await httpClient.GetStreamAsync(searchUrl))
{
    using var jsonStream = new StreamReader(searchResultStream);

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



Console.WriteLine(string.Join(",", foundAtIndexes));
Console.ReadKey();

return;



//https://aicontentfy.com/en/blog/demystifying-google-search-url-parameters-and-how-to-use-them
//https://learn.microsoft.com/en-us/bing/search-apis/bing-web-search/reference/query-parameters
//https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy/sandbox