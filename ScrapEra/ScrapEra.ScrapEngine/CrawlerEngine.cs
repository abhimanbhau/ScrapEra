using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            StartCrawl();
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
            ScrapLogger.Logger.LogI(Url + " -> GetAllParagraphText");
            return _doc.DocumentNode.SelectNodes("//p").Select(para => para.InnerHtml).ToList();
        }

        public List<String> GetAllLinks()
        {
            ScrapLogger.Logger.LogI(Url + " -> GetAllLinks");
            List<String> links = new List<string>();
            foreach (var link in _doc.DocumentNode.SelectNodes("//a"))
            {
                if (Regex.IsMatch(link.GetAttributeValue("href", link.OuterHtml),
                    @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$"))
                {
                    links.Add(link.GetAttributeValue("href", link.OuterHtml));
                }
            }
            return links;
        }

        public List<String> GetDivisionByClass(string className)
        {
            ScrapLogger.Logger.LogI(Url + " -> GetDivisionByClass => '" + className + "'");
            return _doc.DocumentNode.SelectNodes("//div[@class='" + className + "']")
                .Select(div => div.InnerHtml).ToList();
        }

        public List<String> GetDivisionById(string idName)
        {
            ScrapLogger.Logger.LogI(Url + " -> GetDivisionById => '" + idName + "'");
            return _doc.DocumentNode.SelectNodes("//div[@id='" + idName + "']")
                .Select(div => div.InnerHtml).ToList();
        }

        public List<String> GetElementsByXpath(string xpath)
        {
            ScrapLogger.Logger.LogI(Url + " -> GetElementsByXpath => " + xpath);
            return _doc.DocumentNode.SelectNodes(xpath).Select(elem => elem.InnerHtml).ToList();
        }
    }
}