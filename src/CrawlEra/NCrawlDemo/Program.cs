using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using NCrawler.Demo;
using NCrawler.Interfaces;
using NCrawler.Services;

namespace NCrawlDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.MaxServicePoints = 999999;
            ServicePointManager.DefaultConnectionLimit = 999999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.CheckCertificateRevocationList = true;
            ServicePointManager.EnableDnsRoundRobin = true;

            // Run demo 1
            SimpleCrawlDemo.Run();

            // Run demo 2
            //CrawlUsingIsolatedStorage.Run();

            // Run demo 3
            //CrawlUsingDb4oStorage.Run();

            // Run demo 4
            //CrawlUsingEsentStorage.Run();

            // Run demo 5
            //CrawlUsingDbStorage.Run();

            // Run demo 6
            //IndexerDemo.Run();

            // Run demo 7
            //FindBrokenLinksDemo.Run();

            Console.Out.WriteLine("\nDone!");
        }

        public static IFilter[] ExtensionsToSkip = new[]
			{
				(RegexFilter)new Regex(@"(\.jpg|\.css|\.js|\.gif|\.jpeg|\.png|\.ico)",
					RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)
			};
    }
}
