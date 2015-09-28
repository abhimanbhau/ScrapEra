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
        }

        public void StartCrawl()
        {
            HtmlWeb docUrl = new HtmlWeb();
            _doc = docUrl.Load(Url);
        }
    }
}