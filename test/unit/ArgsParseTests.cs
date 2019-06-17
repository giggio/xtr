using DocoptNet;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using Xtr;

namespace Unit
{
    public class ArgsParseTests
    {
        [Test]
        public void ParsePageOrContent()
        {
            const string url = "https://google.com";
            var args = new[] { url };
            var arguments = new MainArgs(args);
            arguments.UrlOrContent.Should().Be(url);
        }

        [Test]
        public void ParseUrl()
        {
            var args = new[] { "http://google.com" };
            var arguments = new MainArgs(args);
            arguments.IsUrl.Should().BeTrue();
        }

        [Test]
        public void ParseUrlWithHttps()
        {
            var args = new[] { "https://google.com" };
            var arguments = new MainArgs(args);
            arguments.IsUrl.Should().BeTrue();
        }

        [Test]
        public void ParseNonUrl()
        {
            var args = new[] { "<html></html>" };
            var arguments = new MainArgs(args);
            arguments.IsUrl.Should().BeFalse();
        }

        [Test]
        public void ParseOptionsAvailableBefore()
        {
            const string output = "output";
            var args = new[] { "--use-browser", "--include-empty-links", "--include-hash-links", "--include-rel-links", "--include-js-links", "--output", output, "--verbose", "<html></html>" };
            var arguments = new MainArgs(args);
            arguments.Output.Should().Be(Path.Combine(Environment.CurrentDirectory, output));
            arguments.Verbose.Should().BeTrue();
            arguments.UseBrowser.Should().BeTrue();
            arguments.IncludeEmptyLinks.Should().BeTrue();
            arguments.IncludeHashLinks.Should().BeTrue();
            arguments.IncludeRelLinks.Should().BeTrue();
            arguments.IncludeJavaScriptLinks.Should().BeTrue();
        }

        [Test]
        public void ParseOptionsAvailableAfter()
        {
            const string output = "output";
            var args = new[] { "<html></html>", "--output", output, "--verbose", "--use-browser", "--include-empty-links", "--include-hash-links", "--include-rel-links", "--include-js-links" };
            var arguments = new MainArgs(args);
            arguments.Output.Should().Be(Path.Combine(Environment.CurrentDirectory, output));
            arguments.Verbose.Should().BeTrue();
            arguments.UseBrowser.Should().BeTrue();
            arguments.IncludeEmptyLinks.Should().BeTrue();
            arguments.IncludeHashLinks.Should().BeTrue();
            arguments.IncludeRelLinks.Should().BeTrue();
            arguments.IncludeJavaScriptLinks.Should().BeTrue();
        }

        [Test]
        public void InputRedirectedWorks()
        {
            var args = new[] { "-" };
            var arguments = new MainArgs(args);
            arguments.InputRedirected.Should().BeTrue();
        }

        [Test]
        public void ParseOptionsUnavailable()
        {
            var args = new[] { "<html></html>" };
            var arguments = new MainArgs(args);
            arguments.Output.Should().BeNull();
            arguments.Verbose.Should().BeFalse();
            arguments.UseBrowser.Should().BeFalse();
            arguments.IncludeEmptyLinks.Should().BeFalse();
            arguments.IncludeHashLinks.Should().BeFalse();
            arguments.IncludeRelLinks.Should().BeFalse();
            arguments.IncludeJavaScriptLinks.Should().BeFalse();
        }

        [Test]
        public void ParseOptionsAvailableShort()
        {
            const string output = "output";
            var args = new[] { "<html></html>", "-o", output, "-b", "-e", "-#", "-j", "-r" };
            var arguments = new MainArgs(args);
            arguments.Output.Should().Be(Path.Combine(Environment.CurrentDirectory, output));
            arguments.UseBrowser.Should().BeTrue();
            arguments.IncludeEmptyLinks.Should().BeTrue();
            arguments.IncludeHashLinks.Should().BeTrue();
            arguments.IncludeRelLinks.Should().BeTrue();
            arguments.IncludeJavaScriptLinks.Should().BeTrue();
        }

        [Test]
        public void ParseOptionsAvailableAbsolute()
        {
            var output = Path.DirectorySeparatorChar + "foo";
            var args = new[] { "<html></html>", "-o", output };
            var arguments = new MainArgs(args);
            arguments.Output.Should().Be(output);
        }

        [Test]
        public void ParseVersion()
        {
            try
            {
                var arguments = new MainArgs((string[])(new[] { "--version" }));
                Assert.Fail("Should have failed on version.");
            }
            catch (DocoptExitException ex)
            {
                ex.Message.Should().MatchRegex(@"\d\.\d\.\d\.\d");
            }
        }

        [Test]
        public void ParseHelp()
        {
            try
            {
                var arguments = new MainArgs((string[])(new[] { "--help" }));
                Assert.Fail("Should have failed on version.");
            }
            catch (DocoptExitException ex)
            {
                ex.Message.Should().Be(MainArgs.usage);
            }
        }

    }
}
