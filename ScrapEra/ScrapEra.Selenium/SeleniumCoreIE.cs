using System;
using OpenQA.Selenium.IE;
using ScrapEra.OutputProcessor;

namespace ScrapEra.Selenium
{
    public static class SeleniumCoreInternetExplorer
    {
        public static InternetExplorerDriver GetIeDriverInstance(string baseUrl)
        {
            return new InternetExplorerDriver(new InternetExplorerOptions
            {
                InitialBrowserUrl = baseUrl,
                IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                IgnoreZoomLevel = true,
                EnableNativeEvents = false
            });
        }

        public static void SafeCloseDriver(this InternetExplorerDriver driver)
        {
            if (driver == null) return;
            try
            {
                driver.Close();
                driver.Quit();
            }
            catch (Exception ex)
            {
                throw new ScrapEraSeleniumGenericException(ex.ToString());
            }
        }

        public static void SetDefaultElementSearchTimeout(this InternetExplorerDriver driver, int seconds)
        {
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(seconds));
        }

        public static void DumpScreenshot(this InternetExplorerDriver driver, string path)
        {
            if (driver == null)
            {
                throw new ScrapEraSeleniumGenericException("driver is null");
            }
            ImageOutput.WriteImageToFile(driver.GetScreenshot().AsBase64EncodedString, path);
        }

        public static void StartTimer(this InternetExplorerDriver driver, ref long startTime)
        {
            startTime = DateTime.Now.Ticks;
        }

        public static int GetRunningTime(this InternetExplorerDriver driver, ref long startTime)
        {
            return TimeSpan.FromTicks(DateTime.Now.Ticks - startTime).Seconds;
        }
    }
}