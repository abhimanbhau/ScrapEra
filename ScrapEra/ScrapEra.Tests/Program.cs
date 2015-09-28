using System;
using ScrapEra.ScrapEngine;

namespace ScrapEra.Tests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var engine = new ScrapCore("http://gre.magoosh.com/questions/1");
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