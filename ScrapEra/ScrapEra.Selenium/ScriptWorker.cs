using System.Collections.Generic;
using System.IO;
using OpenQA.Selenium.IE;
using ScrapEra.ScrapLogger;

namespace ScrapEra.Selenium
{
    public static class ScriptWorker
    {
        public static void RunScript(ref InternetExplorerDriver driver, string scriptPath)
        {
            var scriptContent = File.ReadAllLines(scriptPath);
            foreach (var line in scriptContent)
            {
                Logger.LogI("ScriptWorker -> " + line);
                if (line.ToUpper().Contains("INIT"))
                {
                    driver = SeleniumCoreIE.GetIeDriverInstance(line.Split(' ')[1].Replace("\"", ""));
                }
                else if (line.ToUpper().Contains("NAVIGATE"))
                {
                    driver.Navigate().GoToUrl(line.Split(' ')[1].Replace("\"", ""));
                }
                else if (line.ToUpper().Contains("TYPE"))
                {
                    switch (line.Split(' ')[1].Substring(0, line.IndexOf(":")).ToUpper())
                    {
                        case "CSS":
                            driver.
                                FindElementByCssSelector(line.
                                    Split(' ')[1].Substring(line.
                                        IndexOf(":"))).SendKeys(GetTextToType(line));
                            break;
                        case "XPATH":
                            driver.
                                FindElementByXPath(line.
                                    Split(' ')[1].Substring(line.
                                        IndexOf(":"))).SendKeys(GetTextToType(line));
                            break;
                    }
                }
                else if (line.ToUpper().Contains("CLICK"))
                {
                    switch (line.Split(' ')[1].Substring(0, line.IndexOf(":")).ToUpper())
                    {
                        case "CSS":
                            driver.
                                FindElementByCssSelector(line.
                                    Split(' ')[1].Substring(line.
                                        IndexOf(":"))).Click();
                            break;
                        case "XPATH":
                            driver.
                                FindElementByXPath(line.
                                    Split(' ')[1].Substring(line.
                                        IndexOf(":"))).Click();
                            break;
                    }
                }
            }
        }

        public static void RunScript(ref InternetExplorerDriver driver, IEnumerable<string> scriptContent)
        {

        }

        private static string GetTextToType(string line)
        {
            return line.Split(' ')[2].Replace("\"", "");
        }
    }
}