// See https://aka.ms/new-console-template for more information
using System.IO;
using Domain.Services;

var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Accept.Clear();
httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.8");
httpClient.DefaultRequestHeaders.Add("Content-Security-Policy", "sandbox;");
httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
httpClient.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");

const string keyword = @"""e-settlements""";
const int maxResults = 10;
var searchUrl = $"https://www.google.com/search?q={keyword}&num={maxResults}&hl=en";

var getData = false;

if (getData)
{
    var sjson = await httpClient.GetStringAsync(searchUrl);
    var stream = await httpClient.GetStreamAsync(searchUrl);
    await File.WriteAllTextAsync(Path.Combine(@"D:\tmp", "origin.txt"), sjson);

    return;
}

 void ABC(StreamReader reader)
{

}

using (var searchResultStream = await httpClient.GetStreamAsync(searchUrl))
{
    var ex = new GoogleRankExtractor();
    using var tmp = new StreamReader(searchResultStream);
    var r = ex.Extract("https://www.sympli.com.au", tmp);
    foreach(var item in r)
    {
        Console.WriteLine(item);
    }
}

return;

//var json = await httpClient.GetStringAsync(searchUrl);
var file = Path.Combine(@"D:\tmp", "input.txt");
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
