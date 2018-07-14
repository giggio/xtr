using OpenQA.Selenium;
using System;

namespace Xtr
{
    public class Scraper
    {
        private readonly Uri uri;
        public Scraper(Uri uri) => this.uri = uri;

        public string FetchHtml(bool quiet)
        {
            using (var driver = DriverManager.CreateChromeDriver(quiet))
            {
                driver.Navigate().GoToUrl(uri);
                var htmlElement = driver.FindElement(By.TagName("html"));
                return htmlElement.GetAttribute("outerHTML");
            }
        }
    }
}
