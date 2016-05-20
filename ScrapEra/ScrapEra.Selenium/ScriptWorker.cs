using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using HtmlAgilityPack;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using ScrapEra.CleanReader;
using ScrapEra.OutputProcessor;
using ScrapEra.ScrapEngine;
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
                    Thread.Sleep((int)double.Parse(line.Split(' ')[1]) * 1000);
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
                    var service = PhantomJSDriverService.CreateDefaultService();
                    service.IgnoreSslErrors = true;
                    service.LoadImages = false;
                    service.ProxyType = "none";
                    service.SslProtocol = "tlsv1";
                    driver = new PhantomJSDriver(service);

                    driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(15));
                }
                else if (line.ToUpper().Contains("NAVIGATE"))
                {
                    driver.Navigate().GoToUrl(line.Split(' ')[1].Replace("\"", ""));
                    Thread.Sleep(5000);
                    Console.WriteLine(driver.PageSource);
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
                    Thread.Sleep((int)double.Parse(line.Split(' ')[1]) * 1000);
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

        public static void RunScriptHap(IEnumerable<string> scriptContent,
            string folderPath)
        {
            ScrapCore core = null;
            foreach (var line in scriptContent)
            {
                Logger.LogI("ScriptWorker -> " + line);
                if (line.StartsWith("#"))
                {
                }
                else
                {
                    if (line.ToUpper().Contains("INIT"))
                    {
                        core = new ScrapCore(line.Split(' ')[1]);
                    }
                    else if (line.ToUpper().Contains("NAVIGATE"))
                    {
                    }
                    else if (line.ToUpper().Contains("TYPE"))
                    {
                    }
                    else if (line.ToUpper().Contains("CLICK"))
                    {
                    }
                    else if (line.ToUpper().Contains("SCRAPE_TEXT"))
                    {
                        var tokens = line.Split(' ');
                        var elems = core.GetElemsByXpath(line.Split(' ')[1]);
                        var builder = new StringBuilder();
                        foreach (var elem in elems)
                        {
                            builder.Append(elem.InnerText);
                            builder.Append(Environment.NewLine);
                        }
                        PdfGenerator.GeneratePdfSingleDataType(folderPath + @"\" + tokens[2] + ".pdf",
                            "ScriptWorker Demo",
                            builder.ToString());
                    }
                    else if (line.ToUpper().Contains("WAIT"))
                    {
                        Thread.Sleep((int)double.Parse(line.Split(' ')[1]) * 1000);
                    }
                    else if (line.ToUpper().Contains("SCREENSHOT"))
                    {
                    }
                    else if (line.ToUpper().Contains("STOP"))
                    {
                    }
                    else
                    {
                        Logger.LogE(MethodBase.GetCurrentMethod().Name + " Invalid operation");
                    }
                }
            }
        }

        public static void RunMagicMode(IEnumerable<string> scriptContent, string folderPath)
        {
            foreach (var line in scriptContent)
            {
                Logger.LogI("ScriptWorker -> " + line);
                if (line.StartsWith("#"))
                {
                }
                else
                {
                    if (line.ToUpper().Contains("SEARCH_TERM"))
                    {
                        string term = null;
                        // get links
                        List<String> urls = new List<string>();
                        try
                        {
                            term = line.Split(' ')[1];
                            var sb = new StringBuilder();
                            var resultsBuffer = new byte[16300];
                            var searchResults = "http://google.com/search?q=" + term;
                            var request = (HttpWebRequest)WebRequest.Create(searchResults);
                            var response = (HttpWebResponse)request.GetResponse();

                            var resStream = response.GetResponseStream();
                            var count = 0;
                            do
                            {
                                if (resStream != null) count = resStream.Read(resultsBuffer, 0, resultsBuffer.Length);
                                if (count != 0)
                                {
                                    var tempString = Encoding.ASCII.GetString(resultsBuffer, 0, count);
                                    sb.Append(tempString);
                                }
                            } while (count > 0);
                            var sbb = sb.ToString();

                            var html = new HtmlDocument { OptionOutputAsXml = true };
                            html.LoadHtml(sbb);
                            var doc = html.DocumentNode;

                            urls.AddRange(from link in doc.SelectNodes("//a[@href]")
                                          select link.GetAttributeValue("href", string.Empty)
                                              into hrefValue
                                              where !hrefValue.ToUpper().Contains("GOOGLE")
                                                  && hrefValue.Contains("/url?q=")
                                                  && (hrefValue.ToUpper().Contains("HTTP://"))
                                                  || hrefValue.ToUpper().Contains("HTTPS://")
                                              let index = hrefValue.IndexOf("&")
                                              where index > 0
                                              select hrefValue.Substring(0, index)
                                                  into hrefValue
                                                  select hrefValue.Replace("/url?q=", ""));

                            foreach (var url in urls)
                            {
                                if (!url.Contains("http://") || !url.Contains("https://"))
                                {
                                    //urls.Remove(url);
                                }
                                Logger.LogI(MethodBase.GetCurrentMethod().Name + " Fetched Url " + url);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogE(MethodBase.GetCurrentMethod().Name + "\n" + ex.StackTrace);
                        }
                        var cleanPages = new List<string>();
                        foreach (var url in urls)
                        {
                            CleanReaderWeb cleanWeb = new CleanReaderWeb();
                            HtmlDocument doc = new HtmlDocument();
                            var html = cleanWeb.Transcode(url);
                            if (html == null) continue;
                            doc.LoadHtml(html);

                            try
                            {
                                var stuff = doc.DocumentNode.SelectNodes("//p").Select(para => para.InnerText);
                                var sb = new StringBuilder();
                                foreach (var str in stuff)
                                {
                                    sb.Append(str);
                                }
                                if (sb.Length != 0) cleanPages.Add(sb.ToString());

                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }
                        PdfGenerator.GeneratePdfSingleDataType(folderPath + @"\" + term + ".pdf",
                            "ScrapEra Magic Scraping - " + term,
                            cleanPages);
                    }
                    else
                    {
                        Logger.LogE(MethodBase.GetCurrentMethod().Name + " Invalid operation");
                    }
                }
            }
        }
    }
}