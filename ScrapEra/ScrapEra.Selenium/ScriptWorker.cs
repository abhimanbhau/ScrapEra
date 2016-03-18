using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Reflection;
using System.Threading;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using ScrapEra.OutputProcessor;
using ScrapEra.ScrapLogger;

namespace ScrapEra.Selenium
{
    public static class ScriptWorker
    {
        public static void RunScript(ref InternetExplorerDriver driver, IEnumerable<string> scriptContent,
            string folderPath)
        {
            foreach (var line in scriptContent)
            {
                Logger.LogI("ScriptWorker -> " + line);
                if (line.StartsWith("#"))
                {
                }
                else if (line.ToUpper().Contains("INIT"))
                {
                    driver = SeleniumCoreInternetExplorer.GetIeDriverInstance(line.Split(' ')[1].Replace("\"", ""));
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
                    PdfGenerator.GeneratePdfSingleDataType(folderPath + @"\" + tokens[2] + ".pdf",
                        "ScriptWorker Demo",
                        driver.FindElementByXPath(tokens[1].Substring(tokens[1].IndexOf(":") + 1)).Text);
                }
                else if (line.ToUpper().Contains("WAIT"))
                {
                    Thread.Sleep((int) double.Parse(line.Split(' ')[1])*1000);
                }
                else if (line.ToUpper().Contains("SCREENSHOT"))
                {
                    driver.GetScreenshot().SaveAsFile(folderPath + @"/" + line.Split(' ')[1] + ".png", ImageFormat.Png);
                    //driver.DumpScreenshot(line.Split(' ')[1]);
                }
                else if (line.ToUpper().Contains("STOP"))
                {
                    driver.SafeCloseDriver();
                    //break;
                }
                else
                {
                    Logger.LogE(MethodBase.GetCurrentMethod().Name + " Invalid operation");
                }
            }
        }

        public static void RunScriptPhantom(ref PhantomJSDriver driver, IEnumerable<string> scriptContent,
            string folderPath)
        {
            foreach (var line in scriptContent)
            {
                Logger.LogI("ScriptWorker -> " + line);
                if (line.StartsWith("#"))
                {
                }
                else if (line.ToUpper().Contains("INIT"))
                {
                    driver = new PhantomJSDriver(PhantomJSDriverService.CreateDefaultService());
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
                    PdfGenerator.GeneratePdfSingleDataType(folderPath + @"\" + tokens[2] + ".pdf",
                        "ScriptWorker Demo",
                        driver.FindElementByXPath(tokens[1].Substring(tokens[1].IndexOf(":") + 1)).Text);
                }
                else if (line.ToUpper().Contains("WAIT"))
                {
                    Thread.Sleep((int) double.Parse(line.Split(' ')[1])*1000);
                }
                else if (line.ToUpper().Contains("SCREENSHOT"))
                {
                    driver.GetScreenshot().SaveAsFile(folderPath + @"/" + line.Split(' ')[1] + ".png", ImageFormat.Png);
                    //driver.DumpScreenshot(line.Split(' ')[1]);
                }
                else if (line.ToUpper().Contains("STOP"))
                {
                    driver.Close();
                    //break;
                }
                else
                {
                    Logger.LogE(MethodBase.GetCurrentMethod().Name + " Invalid operation");
                }
            }
        }

        private static string GetTextToType(string line)
        {
            return line.Split(' ')[2].Replace("\"", "").Replace("~", " ");
        }
    }
}