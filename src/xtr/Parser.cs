using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Xtr
{
    public class Parser
    {
        private readonly string html;
        private HtmlDocument doc;
        private readonly Uri uri;

        public Parser(string html) => this.html = html;

        public Parser(Uri uri) => this.uri = uri;

        public IList<Link> GetLinks(bool includeEmpty, bool includeHashLink,
            bool includeJavaScriptLink)
        {
            return GetLinks(includeEmpty, includeHashLink, includeJavaScriptLink, false);
        }

        public IList<Link> GetLinks(bool includeEmpty, bool includeHashLink, bool includeJavaScriptLink, bool includeRelLinks)
        {
            if (doc == null || !doc.DocumentNode.ChildNodes.Any())
                return new List<Link>();
            HtmlNodeCollection linkNodes = doc.DocumentNode.SelectNodes("//a");
            if (includeRelLinks)
            {
                var linkHeadNodes = doc.DocumentNode.SelectNodes("//link");
                if (linkNodes == null && linkHeadNodes != null)
                {
                    //there aren't <a> nodes but there are <link> nodes
                    linkNodes = new HtmlNodeCollection(doc.DocumentNode);
                }
                foreach (var linkHeadNode in linkHeadNodes)
                {
                    linkNodes.Add(linkHeadNode);
                }
            }
            var links = from linkNode in linkNodes
                select new Link
                {
                    Href = linkNode.Attributes["href"]?.Value?.Trim() ?? "",
                    Value = linkNode.Name == "a" ?
                        WebUtility.HtmlDecode(linkNode.InnerText?.Trim() ?? "") :
                        linkNode.Attributes["rel"]?.Value?.Trim() ?? ""
                };

            if (!includeEmpty)
                links = links.Where(l => l.Href != "" && l.Value != "");
            if (!includeHashLink)
                links = links.Where(l => l.Href != "#");
            if (!includeJavaScriptLink)
                links = links.Where(l => !l.Href.StartsWith("javascript:"));
            return links.ToList();
        }

        public Parser Parse()
        {
            if (uri != null)
            {
                var web = new HtmlWeb();
                doc = web.Load(uri);
                return this;
            }
            if (!string.IsNullOrWhiteSpace(html))
            {
                doc = new HtmlDocument();
                doc.LoadHtml(html);
                return this;
            }
            return this;
        }
    }

    public class Link
    {
        public string Href { get; set; }
        public string Value { get; set; }
    }
}
