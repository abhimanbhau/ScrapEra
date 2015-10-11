using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ScrapEra.ScrapLogger;

namespace ScrapEra.ScrapEngine
{
    public class ScrapCore
    {
        private readonly List<string> _links;
        private HtmlDocument _doc;

        public ScrapCore(string url)
        {
            Url = url;
            Logger.LogI(string.Format("ScrapCore -> Initialized crawler with URL='{0}'", url));
            _links = new List<string>();
            ParagraphText = new List<string>();
            StartScrape();
        }

        public List<string> ParagraphText { get; set; }
        public string Url { get; set; }

        private void StartScrape()
        {
            try
            {
                Logger.LogI("StartScrape -> Initialized crawler with URL='" + Url + "'");
                var docUrl = new HtmlWeb
                {
                    UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64; rv:42.0) Gecko/20100101 Firefox/42.0"
                };
                _doc = docUrl.Load(Url);
                if (docUrl.StatusCode != HttpStatusCode.OK)
                {
                    Logger.LogE("StartScrape -> " + Url + " Errorcode -> " + docUrl.StatusCode);
                    return;
                }
                ParagraphText = _doc.DocumentNode.SelectNodes("//p").Select(para => para.InnerHtml).ToList();
                //foreach (var link in _doc.DocumentNode.SelectNodes("//a"))
                //{
                //    try
                //    {
                //        if (Regex.IsMatch(link.GetAttributeValue("href", link.OuterHtml),
                //            @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$"))
                //        {
                //            Links.Add(link.GetAttributeValue("href", link.OuterHtml));
                //        }
                //    }
                //    catch
                //    {
                //    }
                //}
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

        public List<string> GetAllLinks()
        {
            var linkCount = _doc.DocumentNode.SelectNodes("//a").Count;
            var links = _doc.DocumentNode.SelectNodes("//a");
            for (var i = 0; i < linkCount; ++i)
            {
                if (Regex.IsMatch(links[i].GetAttributeValue("href", links[i].OuterHtml),
                    @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$"))
                {
                    _links.Add(links[i].GetAttributeValue("href", links[i].OuterHtml));
                }
            }
            return _links;
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

        public IEnumerable<HtmlNode> GetNodes(string xpath)
        {
            var children = _doc.DocumentNode.SelectNodes(xpath);
            var nodes = new List<HtmlNode>();
            foreach (var stuff in children)
            {
                nodes.AddRange(stuff.ChildNodes);
            }
            return nodes;
        }
    }
}