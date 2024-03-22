// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;
using System.Xml.Linq;

var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Accept.Clear();
httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.8");
httpClient.DefaultRequestHeaders.Add("Content-Security-Policy", "sandbox;");
httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
httpClient.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");

const string keyword = @"""e-settlements""";
const int maxResults = 100;

var getData = false;

if (getData)
{
    var searchUrl = $"https://www.google.com/search?q={keyword}&num={maxResults}&hl=en";
    var sjson = await httpClient.GetStringAsync(searchUrl);
    await File.WriteAllTextAsync(Path.Combine(@"D:\tmp", "origin.txt"), sjson);

    return;
}


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

        //if (!sectionEnd && c == detectPattern[detectPatternIndex])
        //{
        //    if (!sectionStart && detectPatternIndex == detectStartSectionIndex)
        //    {
        //        sectionStart = true;
        //        currentSectionIndex++;
        //    }

        //    detectPatternIndex++;
        //}
        //else
        //{
        //    if (sectionStart && c == detectSectionEndPattern)
        //    {
        //        sectionEnd = true;
        //    }
        //}

        if (currentSectionIndex > -1)
        {
            Console.WriteLine($"Section {currentSectionIndex}");
        }

        






    }
}

Console.WriteLine(string.Join(",", foundAtIndexes));
Console.ReadKey();

return;

var regex = new Regex(
   "(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)",
   RegexOptions.Singleline | RegexOptions.IgnoreCase
);

string ouput = regex.Replace(json, "");
await File.WriteAllTextAsync(Path.Combine(@"D:\tmp", "google.txt"), ouput);

//await File.WriteAllTextAsync(Path.Combine(@"D:\tmp", "google.txt"), json);
//var cts = new CancellationTokenSource();

//var resultDoc = await XDocument.LoadAsync(await httpClient.GetStreamAsync(searchUrl), LoadOptions.None, cts.Token);
//var resultDoc = XDocument.Parse(json);
//foreach (var item in resultDoc.Elements())
//{
//    Console.WriteLine(item.ToString());
//}

ouput = ouput.Replace("doctype", "DOCTYPE");

var resultDoc = XDocument.Parse(ouput);
foreach (var item in resultDoc.Descendants())
{
    Console.WriteLine(item);
}

//var readerSettings = new XmlReaderSettings
//{
//    //Async = true,
//    DtdProcessing = DtdProcessing.Ignore,
//    ValidationFlags = System.Xml.Schema.XmlSchemaValidationFlags.AllowXmlAttributes,
//    ValidationType = ValidationType.None,
//};

//var jsonStream = new FileStream(path: file, FileMode.Open);


//using (var reader = XmlReader.Create(jsonStream, readerSettings))
//{
//    while (reader.Read())
//    {
//        Console.WriteLine(reader.NodeType);
//        Console.WriteLine(reader.ValueType);

//        if (reader.HasValue)
//        {
//            try
//            {
//                //Console.WriteLine(reader.Value);
//                Console.WriteLine(reader.ReadContentAsString());
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.ToString());
//            }
//        }
//    }
//}

//XmlReader.Create()