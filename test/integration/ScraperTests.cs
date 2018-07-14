using FluentAssertions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System;
using Xtr;

namespace Integration
{
    public class ScraperTests
    {
        [Test]
        public void ScrapeWebServer()
        {
            const string html = @"<html><head></head><body><a href=""foo"">bar</a></body></html>";
            using (WebHost.StartWith("http://localhost:10001", app =>
             {
                 app.Run(async context =>
                 {
                     await context.Response.WriteAsync(html);
                 });
             }))
            {
                var s = new Scraper(new Uri("http://localhost:10001"));
                var fetchedHtml = s.FetchHtml(true);
                fetchedHtml.Should().Be(html);
            }
        }

        [Test]
        public void ScrapeWebServerWithDynamicJS()
        {
            const string html = @"<html><head></head><body><div id=""div1"">{0}</div>
<script>
var p = document.createElement(""p"");
var text = document.createTextNode(""This is a test."");
p.appendChild(text);
var div1 = document.getElementById(""div1"");
div1.appendChild(p);
</script>
</body></html>";
            using (WebHost.StartWith("http://localhost:10002", app =>
             {
                 app.Run(async context =>
                 {
                     await context.Response.WriteAsync(string.Format(html, ""));
                 });
             }))
            {
                var s = new Scraper(new Uri("http://localhost:10002"));
                var fetchedHtml = s.FetchHtml(true);
                fetchedHtml.Should().Be(string.Format(html, "<p>This is a test.</p>"));
            }
        }
    }
}
