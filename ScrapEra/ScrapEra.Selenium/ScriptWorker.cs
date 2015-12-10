using System.Collections.Generic;
using System.IO;
using OpenQA.Selenium.IE;
using ScrapEra.Selenium;

namespace ScrapEra.Selenium
{
    public static class ScriptWorker
    {
        public static void RunScript(ref InternetExplorerDriver driver, string scriptPath)
        {
            var scriptContent = File.ReadAllLines(scriptPath);
            foreach (var line in scriptContent)
            {
                if (line.ToUpper().Contains("INIT"))
                {
                    driver = SeleniumCoreIE.GetIeDriverInstance(line.Split('')[1].Replace("\"", ""));
                }
                else if (line.ToUpper().Contains("NAVIGATE"))
                {
                    driver.Navigate().GoToUrl(line.Split(' ')[1].Replace("\"", ""));
                }
                else if (line.ToUpper().Contains("TYPE"))
                {
                    
                }
                else if (line.ToUpper().Contains("CLICK"))
                {
                    
                }
            }
        }

        public static void RunScript(ref InternetExplorerDriver driver, IEnumerable<string> scriptContent)
        {

        }
    }
}