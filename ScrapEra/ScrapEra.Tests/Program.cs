using System.Collections.Generic;
using System.Text;
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

            /*
            var core = new
                ScrapCore("https://crunchprep.com/gre/2014/101-high-frequency-gre-words");
            PdfGenerator.GeneratePdfSingleDataType("101",
                "Crunchprep list", core.GetElementsByXpath("//p[not(@id) and not(@class)]"));
            */

            var core =
                new ScrapCore(
                    "https://docs.google.com/spreadsheets/d/15ZDmjlo4HEHrfR6N0TAVdP1_oxWyVxpIaPgdhV0880E/htmlview?pli=1&sle=true");
            var data = new List<string>();
            var table = core.GetNodes("//html/body/div[2]/div/div/table/tbody");
            foreach (var row in table)
            {
                var singleRow = new StringBuilder();
                foreach (var col in row.ChildNodes)
                {
                    var t = (col.InnerText.Length > 0) ? col.InnerText : "\t";
                    singleRow.Append(t + "|");
                }
                data.Add(singleRow.ToString());
            }

            PdfGenerator.GeneratePdfSingleDataType("AdmitReject", "Univlist", data);
        }
    }
}