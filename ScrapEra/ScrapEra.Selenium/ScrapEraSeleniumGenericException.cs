using System;

namespace ScrapEra.Selenium
{
    internal class ScrapEraSeleniumGenericException : Exception
    {
        public ScrapEraSeleniumGenericException()
        {
        }

        public ScrapEraSeleniumGenericException(string message)
            : base(message)
        {
        }
    }
}