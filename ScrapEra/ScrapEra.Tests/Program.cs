using ScrapEra.OutputProcessor;
using ScrapEra.ScrapEngine;

namespace ScrapEra.Tests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var threads = new ScrapThreaded();
            //for (var i = 1; i < 11; ++i)
            //{
            //    var url = string.Format("http://gre.magoosh.com/questions/{0}", i);
            //    var core = new ScrapCore(url);
            //    Console.WriteLine(PageLanguageDetection.
            //        GetContentLanguage(string.Join(" ", core.ParagraphText.ToArray())));
            //}
            var core = new
                ScrapCore("https://crunchprep.com/gre/2014/101-high-frequency-gre-words");
            PdfGenerator.GeneratePdfSingleDataType("101",
                "Crunchprep list", core.GetElementsByXpath("//p[not(@id) and not(@class)]"));
        }
    }
}