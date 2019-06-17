using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using Xtr;

namespace Unit
{
    public class ParserTests
    {

        [Test]
        public void ParseLinksForEmptyString()
        {
            var html = @"";
            var p = new Parser(html);
            var links = p.Parse().GetLinks(includeEmpty: true, includeHashLink: true, includeJavaScriptLink: true);
            links.Should().BeEmpty();
        }

        [Test]
        public void ParseLinksForString()
        {
            var html = @"<html><body><a href=""foo"">bar</a></body></html>";
            var p = new Parser(html);
            var links = p.Parse().GetLinks(includeEmpty: true, includeHashLink: true, includeJavaScriptLink: true);
            var link = links.Single();
            link.Href.Should().Be("foo");
            link.Value.Should().Be("bar");
        }

        [Test]
        public void IgnoreEmptyValueLink()
        {
            var html = @"<html><body><a href=""baz""></a><a href=""foo"">bar</a></body></html>";
            var p = new Parser(html);
            var links = p.Parse().GetLinks(includeEmpty: false, includeHashLink: true, includeJavaScriptLink: true);
            var link = links.Single();
            link.Href.Should().Be("foo");
            link.Value.Should().Be("bar");
        }

        [Test]
        public void IgnoreEmptyHrefLink()
        {
            var html = @"<html><body><a>baz</a><a href=""foo"">bar</a></body></html>";
            var p = new Parser(html);
            var links = p.Parse().GetLinks(includeEmpty: false, includeHashLink: true, includeJavaScriptLink: true);
            var link = links.Single();
            link.Href.Should().Be("foo");
            link.Value.Should().Be("bar");
        }

        [Test]
        public void DoesNotIgnoreEmptyValueLinkWhenIncludeEmpty()
        {
            var html = @"<html><body><a href=""foo""></a></body></html>";
            var p = new Parser(html);
            var links = p.Parse().GetLinks(includeEmpty: true, includeHashLink: true, includeJavaScriptLink: true);
            var link = links.Single();
            link.Href.Should().Be("foo");
            link.Value.Should().Be("");
        }

        [Test]
        public void DoesIgnoreEmptyHrefLinkWhenIncludeEmpty()
        {
            var html = @"<html><body><a>foo</a></body></html>";
            var p = new Parser(html);
            var links = p.Parse().GetLinks(includeEmpty: true, includeHashLink: true, includeJavaScriptLink: true);
            var link = links.Single();
            link.Href.Should().Be("");
            link.Value.Should().Be("foo");
        }

        [Test]
        public void IgnoreHashLink()
        {
            var html = @"<html><body><a href=""#"">baz</a><a href=""foo"">bar</a></body></html>";
            var p = new Parser(html);
            var links = p.Parse().GetLinks(includeEmpty: false, includeHashLink: false, includeJavaScriptLink: true);
            var link = links.Single();
            link.Href.Should().Be("foo");
            link.Value.Should().Be("bar");
        }

        [Test]
        public void DoesNotIgnoreHashLinkWhenIncludeHashLinks()
        {
            var html = @"<html><body><a href=""#"">foo</a></body></html>";
            var p = new Parser(html);
            var links = p.Parse().GetLinks(includeEmpty: false, includeHashLink: true, includeJavaScriptLink: true);
            var link = links.Single();
            link.Href.Should().Be("#");
            link.Value.Should().Be("foo");
        }

        [Test]
        public void IgnoreJavaScriptLink()
        {
            var html = @"<html><body><a href=""javascript:alert(1)"">baz</a><a href=""foo"">bar</a></body></html>";
            var p = new Parser(html);
            var links = p.Parse().GetLinks(includeEmpty: false, includeHashLink: true, includeJavaScriptLink: false);
            var link = links.Single();
            link.Href.Should().Be("foo");
            link.Value.Should().Be("bar");
        }

        [Test]
        public void DoesNotIgnoreJavaScriptLinkWhenToldSo()
        {
            var html = @"<html><body><a href=""javascript:alert(1)"">foo</a></body></html>";
            var p = new Parser(html);
            var links = p.Parse().GetLinks(includeEmpty: false, includeHashLink: true, includeJavaScriptLink: true);
            var link = links.Single();
            link.Href.Should().Be("javascript:alert(1)");
            link.Value.Should().Be("foo");
        }

        [Test]
        public void HtmlDecodes()
        {
            var html = @"<html><body><a href=""foo"">a&atilde;b</a></body></html>";
            var p = new Parser(html);
            var links = p.Parse().GetLinks(includeEmpty: false, includeHashLink: false, includeJavaScriptLink: false);
            var link = links.Single();
            link.Href.Should().Be("foo");
            link.Value.Should().Be("aãb");
        }

        [Test]
        public void CaptureRelLinks()
        {
            var html = @"<html><head><link rel=""foo"" href=""http://example.com""/></head><body></body></html>";
            var p = new Parser(html);
            var links = p.Parse().GetLinks(includeEmpty: true, includeHashLink: true, includeJavaScriptLink: true, true);
            var link = links.Single();
            link.Href.Should().Be("http://example.com");
            link.Value.Should().Be("foo");
        }

        [Test]
        public void CaptureHrefAndRelLinks()
        {
            var html = @"<html><head><link rel=""foo"" href=""http://example.com""/></head><body><a href=""bar"">baz</a></body></html>";
            var p = new Parser(html);
            var links = p.Parse().GetLinks(includeEmpty: true, includeHashLink: true, includeJavaScriptLink: true, true);
            var link = links[0];
            link.Href.Should().Be("bar");
            link.Value.Should().Be("baz");
            link = links[1];
            link.Href.Should().Be("http://example.com");
            link.Value.Should().Be("foo");
        }
    }
}
