using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Functional.CSharp;
using Utilities;

namespace WebCrawler
{
    public class TheWebCrawler
    {
        public static IEnumerable<string> WebCrawler(string url) //#A
        {
            string content = GetWebContent(url);
            yield return content;

            foreach (string item in AnalyzeHtmlContent(content))
                yield return GetWebContent(item);
        }

        private static string GetWebContent(string url)
        {
            using (var wc = new WebClient())
            {
                try
                {
                    return wc.DownloadString(new Uri(url));
                }
                catch (WebException)
                {
                    // Swallow Web Exceptions since WebClient throws 4xx and 5xx response codes as exceptions
                    return string.Empty;
                }
            }
        }

        private static readonly Regex regexLink = new Regex(@"(?<=href=('|""))https?://.*?(?=\1)");

        private static IEnumerable<string> AnalyzeHtmlContent(string text)
        {
            foreach (var url in regexLink.Matches(text))
                yield return url.ToString();
        }

        private static readonly Regex regexTitle = new Regex("<title>(?<title>.*?)<\\/title>", RegexOptions.Compiled);

        public static string ExtractWebPageTitle(string textPage) //#D
        {
            if (regexTitle.IsMatch(textPage))
                return regexTitle.Match(textPage).Groups["title"].Value;
            return "No Page Title Found!";
        }

        // Listing 2.18 Web crawler execution using memoization
        public static Func<string, IEnumerable<string>> WebCrawlerMemoized =
            Memoization.Memoize<string, IEnumerable<string>>(WebCrawler); // #A

        // Listing 2.20 Thread-safe memoization function
        public static Func<string, IEnumerable<string>> WebCrawlerMemoizedThreadSafe =
            Memoization.MemoizeThreadSafe<string, IEnumerable<string>>(WebCrawler);


        public static void RunDemo()
        {
            List<string> urls = new List<string> { //#A
                @"http://www.google.com",
                @"http://www.microsoft.com",
                @"http://www.bing.com",
                @"http://www.google.com"
            };

            Demo.TimeTask("Listing 2.17 Web crawler execution", () =>
            {
                var webPageTitles = from url in urls //#B
                                    from pageContent in WebCrawler(url)
                                    select ExtractWebPageTitle(pageContent);

                Console.WriteLine($"Crawled {webPageTitles.Count()} page titles");
            });


            Demo.TimeTask("Listing 2.18 Web crawler execution using memoization", () =>
            {
                var webPageTitles = from url in urls //#B
                                    from pageContent in WebCrawlerMemoized(url)
                                    select ExtractWebPageTitle(pageContent);

                Console.WriteLine($"Crawled {webPageTitles.Count()} page titles");
            });

            Demo.TimeTask("Listing 2.19 Web crawler query using PLINQ", () =>
            {
                var webPageTitles = from url in urls.AsParallel() //#A
                                    from pageContent in WebCrawlerMemoized(url)
                                    select ExtractWebPageTitle(pageContent);

                Console.WriteLine($"Crawled {webPageTitles.Count()} page titles");
            });

            Demo.TimeTask("Listing 2.20 Thread-safe memoization function", () =>
            {
                var webPageTitles = from url in urls.AsParallel()
                                    from pageContent in WebCrawlerMemoizedThreadSafe(url) //#B
                                    select ExtractWebPageTitle(pageContent);  //#C

                Console.WriteLine($"Crawled {webPageTitles.Count()} page titles");
            });
        }
    }
}



