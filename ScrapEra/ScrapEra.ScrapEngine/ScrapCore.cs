using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ScrapEra.ScrapLogger;

namespace ScrapEra.ScrapEngine
{
    public class ScrapCore
    {
        private HtmlDocument _doc;

        public ScrapCore(string url)
        {
            Url = url;
            Logger.LogI(string.Format("ScrapCore -> Initialized crawler with URL='{0}'", url));
            Links = new List<string>();
            ParagraphText = new List<string>();
            StartScrape();
        }

        public List<string> Links { get; set; }
        public List<string> ParagraphText { get; set; }
        public string Url { get; set; }

        public void StartScrape()
        {
            try
            {
                Logger.LogI("StartScrape -> Initialized crawler with URL='" + Url + "'");
                var docUrl = new HtmlWeb();
                _doc = docUrl.Load(Url);
                ParagraphText = _doc.DocumentNode.SelectNodes("//p").Select(para => para.InnerHtml).ToList();
                foreach (var link in _doc.DocumentNode.SelectNodes("//a"))
                {
                    if (Regex.IsMatch(link.GetAttributeValue("href", link.OuterHtml),
                        @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$"))
                    {
                        Links.Add(link.GetAttributeValue("href", link.OuterHtml));
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogE("StartScrape -> " + e.StackTrace + Environment.NewLine
                            + e.Message);
            }
        }

        public List<string> GetDivisionByClass(string className)
        {
            Logger.LogI(Url + " -> GetDivisionByClass => '" + className + "'");
            return _doc.DocumentNode.SelectNodes("//div[@class='" + className + "']")
                .Select(div => div.InnerHtml).ToList();
        }

        public List<string> GetDivisionById(string idName)
        {
            Logger.LogI(Url + " -> GetDivisionById => '" + idName + "'");
            return _doc.DocumentNode.SelectNodes("//div[@id='" + idName + "']")
                .Select(div => div.InnerHtml).ToList();
        }

        public List<string> GetElementsByXpath(string xpath)
        {
            Logger.LogI(Url + " -> GetElementsByXpath => " + xpath);
            return _doc.DocumentNode.SelectNodes(xpath).Select(elem => elem.InnerHtml).ToList();
        }
    }
}