using DocoptNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Xtr
{
    class Program
    {
        private static ConsoleColor defaultConsoleColor;

        static int Main(string[] args)
        {
            Initialize();
            MainArgs arguments;
            try
            {
                arguments = new MainArgs(args);
            }
            catch (DocoptExitException ex)
            {
                WriteLine(ex.Message);
                return 0;
            }
            catch (DocoptInputErrorException ex)
            {
                WriteErrorLine(ex.Message);
                return 1;
            }
            var parser = CreateParser(arguments);
            var links = parser.Parse().GetLinks(
                includeEmpty: arguments.IncludeEmptyLinks,
                includeHashLink: arguments.IncludeHashLinks,
                includeJavaScriptLink: arguments.IncludeJavaScriptLinks,
                includeRelLinks: arguments.IncludeRelLinks);
            var linksText = GetLinksText(links);
            if (!string.IsNullOrWhiteSpace(arguments.Output))
                File.WriteAllText(arguments.Output, linksText);
            else
                WriteLine($"Links:\n{linksText}");
            return 0;
        }

        private static Parser CreateParser(MainArgs arguments)
        {
            if (arguments.InputRedirected && Console.IsInputRedirected)
            {
                using (var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding))
                {
                    var stdin = reader.ReadToEnd();
                    return new Parser(stdin);
                }
            }
            if (!arguments.IsUrl)
                return new Parser(arguments.UrlOrContent);
            var url = new Uri(arguments.UrlOrContent);
            if (!arguments.UseBrowser)
                return new Parser(url);
            return new Parser(new Scraper(url).FetchHtml(!arguments.Verbose));
        }

        private static string GetLinksText(IList<Link> links)
        {
            var sb = new StringBuilder();
            if (!links.Any())
            {
                WriteLine("No links found.");
                return null;
            }
            foreach (var link in links)
                sb.AppendLine($"{link.Value},{link.Href}");
            return sb.ToString();
        }

        private static void Initialize() => defaultConsoleColor = Console.ForegroundColor;

        private static readonly object sync = new object();

        private static void WriteLine(string message = "")
        {
            if (message == null)
                return;
            lock (sync)
                Console.WriteLine(message);
        }

        private static void Write(string message = "")
        {
            if (message == null)
                return;
            lock (sync)
                Console.Write(message);
        }

        private static void WriteErrorLine(string message)
        {
            if (message == null)
                return;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && message.StartsWith("INFO:"))
                return;
            lock (sync)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Error.WriteLine(message);
                Console.ForegroundColor = defaultConsoleColor;
            }
        }
    }
}
