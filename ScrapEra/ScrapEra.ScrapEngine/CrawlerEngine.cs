using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ScrapEra.ScrapEngine
{
    public class CrawlerEngine
    {
        public string Url { get; set; }

        private HtmlDocument _doc;

        public CrawlerEngine(string url)
        {
            Url = url;
            ScrapLogger.Logger.LogI("Initialized crawler with URL='" + url + "'");
        }

        public void StartCrawl()
        {
            try
            {
                ScrapLogger.Logger.LogI("Initialized crawler with URL='" + Url + "'");
                HtmlWeb docUrl = new HtmlWeb();
                _doc = docUrl.Load(Url);
            }
            catch (Exception e)
            {
                ScrapLogger.Logger.LogE("Fatal Error -> " + e.StackTrace + Environment.NewLine
                    + e.Message);
            }
        }

        public List<String> GetAllParagraphText()
        {
            return _doc.DocumentNode.SelectNodes("//p").Select(para => para.InnerHtml).ToList();
        }

        public List<String> GetAllLinks()
        {
            return _doc.DocumentNode.SelectNodes("//a").Select(link => link.InnerHtml).ToList();
        }
    }
}