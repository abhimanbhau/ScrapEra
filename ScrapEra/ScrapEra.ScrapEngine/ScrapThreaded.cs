using System.Collections.Generic;
using System.Threading;

namespace ScrapEra.ScrapEngine
{
    public class ScrapThreaded
    {
        public Dictionary<string, List<string>> StartNewScrap(string url)
        {
            var data = new Dictionary<string, List<string>>();
            var t = new Thread(() => data = DoScrap(url));
            t.Start();
            t.Join();
            return data;
        }

        private Dictionary<string, List<string>> DoScrap(string url)
        {
            var data = new Dictionary<string, List<string>>();
            var core = new ScrapCore(url);
            data.Add(url + "-links", core.GetAllLinks());
            data.Add(url + "-para", core.ParagraphText);
            return data;
        }
    }
}