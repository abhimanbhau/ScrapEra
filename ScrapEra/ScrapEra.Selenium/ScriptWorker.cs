using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using OpenQA.Selenium.IE;
using ScrapEra.ScrapLogger;

namespace ScrapEra.Selenium
{
    public static class ScriptWorker
    {
        public static void RunScript(ref InternetExplorerDriver driver, string scriptPath)
        {
            Thread.Sleep(700);
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
                    var tokens = line.Split(' ');
                    switch (tokens[1].Substring(0, tokens[1].Length - tokens[1].Substring(tokens[1].IndexOf(":")).Length).ToUpper())
                    {
                        case "CSS":
                            driver.
                                FindElementByCssSelector(tokens[1].Substring(tokens[1].
                                        IndexOf(":") + 1)).SendKeys(GetTextToType(line));
                            break;
                        case "XPATH":
                            driver.
                                FindElementByXPath(tokens[1].Substring(tokens[1].
                                        IndexOf(":") + 1)).SendKeys(GetTextToType(line));
                            break;

                        default:
                            break;
                    }
                }
                else if (line.ToUpper().Contains("CLICK"))
                {
                    var tokens = line.Split(' ');
                    switch (
                        tokens[1].Substring(0, tokens[1].Length - tokens[1].Substring(tokens[1].IndexOf(":")).Length)
                            .ToUpper())
                    {
                        case "CSS":
                            driver.
                                FindElementByCssSelector(tokens[1].Substring(tokens[1].
                                    IndexOf(":") + 1)).Click();
                            break;
                        case "XPATH":
                            driver.
                                FindElementByXPath(tokens[1].Substring(tokens[1].
                                    IndexOf(":") + 1)).Click();
                            break;
                    }
                }
                else if (line.ToUpper().Contains("WAIT"))
                {
                    Thread.Sleep((int)double.Parse(line.Split(' ')[1]) * 1000);
                }
                else if (line.ToUpper().Contains("STOP"))
                {
                    driver.SafeCloseDriver();
                    break;
                }
            }
        }

        public static void RunScript(ref InternetExplorerDriver driver, IEnumerable<string> scriptContent)
        {
        }

        private static string GetTextToType(string line)
        {
            return line.Split(' ')[2].Replace("\"", "").Replace("~", " ");
        }
    }
}