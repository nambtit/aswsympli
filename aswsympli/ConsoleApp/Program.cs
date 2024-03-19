// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;
using System.Xml.Linq;
using ConsoleApp;

var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Accept.Clear();
httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.8");
httpClient.DefaultRequestHeaders.Add("Content-Security-Policy", "sandbox;");
httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
httpClient.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");




const string keyword = @"""e-settlements""";
const int maxResults = 100;


var searchUrl = $"https://www.google.com/search?q={keyword}&num={maxResults}&hl=en";

var sjson = await httpClient.GetStringAsync(searchUrl);
await File.WriteAllTextAsync(Path.Combine(@"D:\tmp", "origin.txt"), sjson);

return;

//var json = await httpClient.GetStringAsync(searchUrl);
var file = Path.Combine(@"D:\tmp", "input.txt");
//var file = Path.Combine(@"D:\tmp", "google.txt");
var json = await File.ReadAllTextAsync(file);
const int size = 1000;
var trackingQueue = new FixedSizedQueue<char>(size);

var acceptedChars = new HashSet<char>
{
    '<', '>', 'd', 'i', 'v', 's', 'p', 'a', 'n', 'h', 'r', 'e', 'f', '=', 's', 'y', 'm', 'p', 'l', 'i'
};

//var pattern = "<cite class=\"tjvcx GvPZzd cHaqb\" role=\"text\">https://www.sympli.com.au</cite>"

var acceptedCharOrder = new Dictionary<char, HashSet<char>>
{
    {'<', new HashSet<char>{ '*' }},
    {'>', new HashSet<char>{ '*' }},

    {'d', new HashSet<char>{ '<' }},
    {'i', new HashSet<char>{ 'd' }},
    {'v', new HashSet<char>{ 'i' }},

    {'s', new HashSet<char>{ 'd' }},
    {'p', new HashSet<char>{ 'd' }},
    {'a', new HashSet<char>{ 'd' }},
    {'n', new HashSet<char>{ 'd' }},

    {'h', new HashSet<char>{ '=' }},
    {'r', new HashSet<char>{ 'h' }},
    {'e', new HashSet<char>{ 'r' }},
    {'f', new HashSet<char>{ 'e' }},
};

//<divclassnottranslatecitesympli.com.au
//<divclassnottranslate => count index
//<divclassnottranslatecitesympli.com.au

const string resultSectionToken = "<div class=\"notranslate\"";
const string seoListedToken = "role=\"text\">https://www.sympli.com.au</cite>";

var sectionDetectArr = resultSectionToken.ToCharArray();
var sectionTrackArr = new char[sectionDetectArr.Length];
var sectionClearTokens = new HashSet<char> { '/' };
var searchIndex = -1;
var resultSectionTraceIndex = 0;
var seoTrackIndex = 0;
var withinSection = false;
var foundAtIndexes = new Queue<int>();

using (var jsonStream = new StreamReader(file))
{
    while (!jsonStream.EndOfStream)
    {
        var c = (char)jsonStream.Read();

        if (withinSection)
        {
            if (seoTrackIndex == seoListedToken.Length - 1)
            {
                foundAtIndexes.Enqueue(searchIndex);
                seoTrackIndex = 0;
                withinSection = false;
            }
            else
            {
                if (c == seoListedToken[seoTrackIndex])
                {
                    seoTrackIndex++;
                }
                else
                {
                    seoTrackIndex = 0;
                }
            }
        }
        else
        {
            if (c == resultSectionToken[resultSectionTraceIndex])
            {
                resultSectionTraceIndex++;
            }
            else
            {
                // We are under a section.
                if (resultSectionTraceIndex == sectionDetectArr.Length - 1)
                {
                    searchIndex++;
                    withinSection = true;
                }

                resultSectionTraceIndex = 0;
            }
        }





        //Console.WriteLine(string.Join(null, sectionTrackArr.Select(e => Array.IndexOf(sectionTrackArr, e) <= trackIndex)));


        //if (!acceptedChars.Contains(c))
        //{
        //    continue;
        //}

        //trackingQueue.Enqueue(c);

        //if (char.Equals(c, '>'))
        //{
        //    if (trackingQueue.Limit == size)
        //    {
        //        Console.WriteLine(trackingQueue.GetString());
        //    }
        //}

        //if (trackingQueue.GetString().Contains("sympli"))
        //{
        //    Console.WriteLine(trackingQueue.GetString());
        //}
    }
}

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