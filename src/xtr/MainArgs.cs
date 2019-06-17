using DocoptNet;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Xtr
{
    public class MainArgs
    {
        public const string usage = @"xtr - A tool to crawl and parse web pages

Usage:
  xtr [options] -
  xtr [options] <urlorcontent>

Options:
  --output=<file>, -o               File path where to save the output, if absent output is sent to stdout
  --use-browser, -b                 Use a browser to fetch the contents [default: false]
  --include-empty-links, -e         Include links without href or value [default: false]
  --include-hash-links, -#          Include links with # on href [default: false]
  --include-rel-links, -r           Include rel links in head [default: false]
  --include-js-links, -j            Include links with 'javascript:' on  href [default: false]
  --verbose                         Verbose install and run [default: false]
  --version, -v                     Show version number
  --help, -h                        Show help
";

        public MainArgs(string[] argv)
        {
            var fixedArgs = argv; //todo remove
            var version = typeof(MainArgs).Assembly.GetName().Version.ToString();
            var args = new Docopt().Apply(usage, fixedArgs, version: version);
            Verbose = args["--verbose"].IsTrue;
            UseBrowser = args["--use-browser"].IsTrue;
            IncludeEmptyLinks = args["--include-empty-links"].IsTrue;
            IncludeHashLinks = args["--include-hash-links"].IsTrue;
            IncludeRelLinks = args["--include-rel-links"].IsTrue;
            IncludeJavaScriptLinks = args["--include-js-links"].IsTrue;
            InputRedirected = args["-"].IsTrue;
            if (!string.IsNullOrWhiteSpace(args["--output"]?.Value as string))
            {
                Output = args["--output"].ToString();
                if (!Path.IsPathRooted(Output))
                    Output = Path.Combine(Environment.CurrentDirectory, Output);
            }
            UrlOrContent = args["<urlorcontent>"]?.ToString();
            IsUrl = UrlOrContent != null && Regex.IsMatch(UrlOrContent, "^https?://");
        }

        public string UrlOrContent { get; }
        public bool IsUrl { get; }
        public bool Verbose { get; }
        public string Output { get; }
        public bool UseBrowser { get; }
        public bool IncludeEmptyLinks { get; }
        public bool IncludeJavaScriptLinks { get; }
        public bool IncludeHashLinks { get; }
        public bool IncludeRelLinks { get; }
        public bool InputRedirected { get; }
    }
}
