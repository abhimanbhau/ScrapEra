using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Threading.Tasks;

namespace HAPDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            HtmlDocument doc = new HtmlDocument();
            HtmlWeb docUrl = new HtmlWeb();
            doc = docUrl.Load("http://gre.magoosh.com/questions/5");

            var stuff = doc.DocumentNode.SelectNodes("//div[@class='span8 offset0']");
            foreach (var stuffed in stuff)
            {
                Console.WriteLine(stuffed.InnerText);
            }
            if (stuff == null) return;
            Console.WriteLine("This is the shit i fetched -> \n" + stuff[0].InnerText);
        }
    }
}
