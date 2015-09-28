using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapEra.ScrapEngine;

namespace ScrapEra.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            CrawlerEngine engine = new CrawlerEngine("http://gre.magoosh.com/questions/1");
            foreach (var links in engine.GetAllLinks())
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(links);
            }

            foreach (var paras in engine.GetAllParagraphText())
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(paras);
            }
        }
    }
}
