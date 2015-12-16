﻿using OpenQA.Selenium.IE;
using ScrapEra.Selenium;

namespace ScrapEra.Tests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var threads = new ScrapThreaded();
            //for (var i = 1; i < 11; ++i)
            //{
            //    var url = string.Format("http://gre.magoosh.com/questions/{0}", i);
            //    var core = new ScrapCore(url);
            //    Console.WriteLine(PageLanguageDetection.
            //        GetContentLanguage(string.Join(" ", core.ParagraphText.ToArray())));
            //}

            /*
            var core = new
                ScrapCore("https://crunchprep.com/gre/2014/101-high-frequency-gre-words");
            PdfGenerator.GeneratePdfSingleDataType("101",
                "Crunchprep list", core.GetElementsByXpath("//p[not(@id) and not(@class)]"));
            */

            //var core =
            //    new ScrapCore(
            //        "https://docs.google.com/spreadsheets/d/15ZDmjlo4HEHrfR6N0TAVdP1_oxWyVxpIaPgdhV0880E/htmlview?pli=1&sle=true");
            //var data = new List<string>();
            //var table = core.GetNodes("//html/body/div[2]/div/div/table/tbody");
            //foreach (var row in table)
            //{
            //    var singleRow = new StringBuilder();
            //    foreach (var col in row.ChildNodes)
            //    {
            //        var t = (col.InnerText.Length > 0) ? col.InnerText : "\t";
            //        singleRow.Append(t + "|");
            //    }
            //    data.Add(singleRow.ToString());
            //}

            //PdfGenerator.GeneratePdfSingleDataType("AdmitReject", "Univlist", data);


            /*
            var termsToSearch = new List<string>
            {
                //"Abhiman Kolte",
                //"Sachin Tendulkar",
                //"Is Pluto a planet?",
                //"Roshan Karande",
                //"Shivam Gupta",
                //"Shivaji Maharaj",
                //"India",
                "Carnegie Mellon University",
                "NCSU",
                "UPitt",
                "UNCC",
                "IITK",
                "VIT Pune"
                //"Latest Movies",
                //"Facebook news",
                //"Latest mobiles",
                //"latest news",
                //"hot topics"
            };
            const string url = "http://www.google.com";
            long timerStart = 0;
            var driver = SeleniumCoreIE.GetIeDriverInstance(url);
            driver.StartTimer(ref timerStart);
            driver.SetDefaultElementSearchTimeout(10);
            var dataMined = new List<string>();
            foreach (var search in termsToSearch)
            {
                driver.Navigate().GoToUrl(url);
                driver.FindElementByClassName("gsfi").SendKeys(search);
                driver.FindElementByName("btnK").Click();
                dataMined.Add(driver.FindElementById("ires").Text);
            }
            PdfGenerator.GeneratePdfSingleDataType("google.pdf", "miner", dataMined);
            Console.WriteLine(driver.GetRunningTime(ref timerStart));
            driver.SafeCloseDriver();
             * 
             */
            InternetExplorerDriver driver = null;
            ScriptWorker.RunScript(ref driver, @"C:\Users\abhim\Desktop\ScrapEra\FB_Login_Script.txt");
        }
    }
}