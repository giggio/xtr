using FluentAssertions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System;
using System.Linq;
using Xtr;

namespace Integration
{
    public class ParserWithUriTests
    {
        [Test]
        public void ParseLinksForUri()
        {
            using (WebHost.StartWith("http://localhost:9999", app =>
             {
                 app.Run(async context =>
                 {
                     const string html = @"<html><body><a href=""foo"">bar</a></body></html>";
                     await context.Response.WriteAsync(html);
                 });
             }))
            {
                var p = new Parser(new Uri("http://localhost:9999"));
                var links = p.Parse().GetLinks(includeEmpty: false, includeHashLink: false, includeJavaScriptLink: false);
                var link = links.First();
                link.Href.Should().Be("foo");
                link.Value.Should().Be("bar");
            }
        }
    }
}
