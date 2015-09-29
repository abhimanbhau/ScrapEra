using System;
using ScrapEra.ScrapEngine;

namespace ScrapEra.Tests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var threads = new ScrapThreaded();
            for (var i = 1; i < 11; ++i)
            {
                var url = string.Format("http://gre.magoosh.com/questions/{0}", i);
                var stuff = threads.StartNewScrap(url);
                foreach (var data in stuff)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(data.Key);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    foreach (var str in data.Value)
                    {
                        Console.WriteLine(str);
                    }
                }
            }
        }
    }
}