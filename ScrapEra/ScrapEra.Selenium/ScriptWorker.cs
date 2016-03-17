using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using OpenQA.Selenium.IE;
using ScrapEra.ScrapLogger;
using ScrapEra.OutputProcessor;

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
                if (line.StartsWith("#"))
                {
                    continue;
                }
                else if (line.ToUpper().Contains("INIT"))
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
                    switch (
                        tokens[1].Substring(0, tokens[1].Length - tokens[1].Substring(tokens[1].IndexOf(":")).Length)
                            .ToUpper())
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
                else if (line.ToUpper().Contains("SCRAPE_TEXT"))
                {
                    var tokens = line.Split(' ');
                    // Console.WriteLine(driver.FindElementByXPath(tokens[1].Substring(tokens[1].IndexOf(":") + 1)).Text);
                    PdfGenerator.GeneratePdfSingleDataType(Path.GetTempFileName().Replace(".tmp", ".pdf "), 
                        "ScriptWorker Demo", 
                        driver.FindElementByXPath(tokens[1].Substring(tokens[1].IndexOf(":") + 1)).Text);

                }
                else if (line.ToUpper().Contains("WAIT"))
                {
                    Thread.Sleep((int)double.Parse(line.Split(' ')[1]) * 1000);
                }
                else if (line.ToUpper().Contains("SCREENSHOT"))
                {
                    driver.GetScreenshot().SaveAsFile(Path.GetTempFileName().Replace(".tmp", ".png "), ImageFormat.Bmp);
                }
                else if (line.ToUpper().Contains("STOP"))
                {
                    driver.SafeCloseDriver();
                    //break;
                }
            }
        }

        public static void RunScript(ref InternetExplorerDriver driver, IEnumerable<string> scriptContent)
        {
            foreach (var line in scriptContent)
            {
                Logger.LogI("ScriptWorker -> " + line);
                if (line.StartsWith("#"))
                {
                    continue;
                }
                else if (line.ToUpper().Contains("INIT"))
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
                    switch (
                        tokens[1].Substring(0, tokens[1].Length - tokens[1].Substring(tokens[1].IndexOf(":")).Length)
                            .ToUpper())
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
                else if (line.ToUpper().Contains("SCRAPE_TEXT"))
                {
                    var tokens = line.Split(' ');
                    // Console.WriteLine(driver.FindElementByXPath(tokens[1].Substring(tokens[1].IndexOf(":") + 1)).Text);
                    PdfGenerator.GeneratePdfSingleDataType(Path.GetTempFileName().Replace(".tmp", ".pdf "), 
                        "ScriptWorker Demo", 
                        driver.FindElementByXPath(tokens[1].Substring(tokens[1].IndexOf(":") + 1)).Text);

                }
                else if (line.ToUpper().Contains("WAIT"))
                {
                    Thread.Sleep((int)double.Parse(line.Split(' ')[1]) * 1000);
                }
                else if (line.ToUpper().Contains("SCREENSHOT"))
                {
                    //driver.GetScreenshot().SaveAsFile(line.Split(' ')[1], ImageFormat.Png);
                    driver.DumpScreenshot(line.Split(' ')[1]);
                }
                else if (line.ToUpper().Contains("STOP"))
                {
                    driver.SafeCloseDriver();
                    //break;
                }
            }
        }

        private static string GetTextToType(string line)
        {
            return line.Split(' ')[2].Replace("\"", "").Replace("~", " ");
        }
    }
}