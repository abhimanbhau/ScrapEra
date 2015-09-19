using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace HAPDemo
{
    class Demo
    {
        public static void Run()
        {
            HtmlWeb docUrl = new HtmlWeb();
            
            for (int i = 0; i < 10; ++i)
            {
                var doc = docUrl.Load(String.Format("http://gre.magoosh.com/questions/{0}", i));

                var stuff = doc.DocumentNode.SelectNodes("//div[@class='span8 offset0']");
                if (stuff == null) continue;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Out.WriteLine("Fetching GRE Questions -> \n\n" + stuff[0].InnerText);
            }
        }
    }
}
