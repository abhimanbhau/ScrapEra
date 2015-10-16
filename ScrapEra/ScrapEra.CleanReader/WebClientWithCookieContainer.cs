using System;
using System.Net;

namespace ScrapEra.CleanReader
{
    internal class WebClientWithCookieContainer : WebClient
    {
        private readonly CookieContainer _cookieContainer;

        public WebClientWithCookieContainer()
        {
            _cookieContainer = new CookieContainer();
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var webRequest = base.GetWebRequest(address);
            var httpWebRequest = webRequest as HttpWebRequest;
            if (httpWebRequest != null)
            {
                httpWebRequest.CookieContainer = _cookieContainer;
            }
            return webRequest;
        }
    }
}