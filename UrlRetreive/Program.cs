using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UrlRetreive
{
    internal static class Program
    {
        private static int counter;
        private static int countDownloaded;
        private static int countSkipped;
        private const string http_prefix = @"http://";
        private const string url = "http://people.csail.mit.edu/brussell/research/LabelMe/Images/05june05_static_street_boston/";
        private static string[] image_ext = { ".jpg", ".jpeg" };
        

        // https://stackoverflow.com/questions/124492/c-sharp-httpwebrequest-command-to-get-directory-listing
        // https://stackoverflow.com/questions/307688/how-to-download-a-file-from-a-url-in-c

        // http://people.csail.mit.edu/brussell/research/LabelMe/Images/05june05_static_street_boston/
        // http://people.csail.mit.edu/brussell/research/LabelMe/Images/05june05_static_street_boston/p1010737.jpg

        // http://people.csail.mit.edu/brussell/research/LabelMe/Annotations/
        // http://people.csail.mit.edu/brussell/research/LabelMe/Images/

        static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                counter = 0;
                countDownloaded = 0;
                countSkipped = 0;

                switch (args[0])
                {
                    case "--help":
                        PrintHelp();
                        break;
                    case "--get":
                        if (args.Length == 2)
                        {
                            if (url.StartsWith(http_prefix) && ValidUrl(url))
                            {
                                GetContents(args[1]);
                                Console.WriteLine("Finished");
                            }
                            else
                            {
                                Console.WriteLine("Argument is not a valid url");
                            }
                        }
                        else PrintHelp();
                        break;
                    default:
                        PrintHelp();
                        break;
                }
            }
            else
            {
                PrintHelp();
            }

            Console.WriteLine(string.Empty);

        }

        private static bool ValidUrl(string url)
        {
            return true;
        }

        private static void PrintHelp()
        {
            Console.WriteLine(@"Usage:");
            Console.WriteLine(@"UrlRetreive --get url");
            Console.WriteLine(@"UrlRetreive --help");
        }

        private static void GetContents(string url)
        {
            Console.WriteLine("\n" + url);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string html = reader.ReadToEnd();

                        Regex regex = new Regex(GetDirectoryListingRegexForUrl(url));

                        MatchCollection matches = regex.Matches(html);

                        if (matches.Count > 0)
                        {
                            foreach (Match match in matches)
                            {
                                if (match.Success)
                                {
                                    string hyperlinkElement = match.Groups["name"].ToString();

                                    if (hyperlinkElement.EndsWith(".jpg") || hyperlinkElement.EndsWith(".jpeg") || hyperlinkElement.EndsWith(".png") || hyperlinkElement.EndsWith(".tif") || hyperlinkElement.EndsWith(".tiff")) {

                                        string sourceUrl = url + ((url.EndsWith("/")) ? string.Empty : "/") + hyperlinkElement;

                                        Console.WriteLine(sourceUrl);

                                        string destDir = url.Replace(http_prefix, "");

                                        string destDirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), destDir);

                                        Directory.CreateDirectory(destDirPath);

                                        string destFilePath = Path.Combine(destDirPath, hyperlinkElement);

                                        destFilePath.Replace('/', Path.DirectorySeparatorChar);

                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine(destFilePath);
                                        Console.ResetColor();

                                        if (!File.Exists(destFilePath))
                                        {
                                            try
                                            {
                                                using (var client = new WebClient())
                                                {
                                                    client.DownloadFile(sourceUrl, destFilePath);
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine(e.Message);
                                                Console.ResetColor();
                                            }

                                            countDownloaded++;

                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine("downloaded, count = " + countDownloaded);
                                            Console.ResetColor();

                                        }
                                        else
                                        {
                                            countSkipped++;
                                            Console.WriteLine("skipped, count = " + countSkipped);
                                        }

                                        counter++;

                                        Console.WriteLine("found " + $"{counter} images");

                                    }
                                    else
                                    {
                                        GetContents(url + hyperlinkElement); // recursively call
                                    }


                                }
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Found no hyperlink references in retreived file.");
                            Console.ResetColor();
                        }
                    }
                }
            }
            catch (WebException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
            
        }

        private static string GetDirectoryListingRegexForUrl(string url)
        {
            return "<a href=\".*\">(?<name>.*)</a>";
        }

        public static bool EndsWithAny(this string str, params string[] search)
        {
            return search.Any(s => str.EndsWith(s));
        }

    }
}
