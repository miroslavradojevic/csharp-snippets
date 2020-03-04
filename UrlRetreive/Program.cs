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
        private const string url = "http://people.csail.mit.edu/brussell/research/LabelMe/Images/05june05_static_street_boston/p1010737.jpg";
        // http://people.csail.mit.edu/brussell/research/LabelMe/Images/
        // Hints:
        // https://stackoverflow.com/questions/124492/c-sharp-httpwebrequest-command-to-get-directory-listing
        // https://stackoverflow.com/questions/307688/how-to-download-a-file-from-a-url-in-c
        // http://people.csail.mit.edu/brussell/research/LabelMe/Images/05june05_static_street_boston/p1010737.jpg
        // http://www.ibiblio.org/pub
        // http://people.csail.mit.edu/brussell/research/LabelMe/Annotations/
        // http://people.csail.mit.edu/brussell/research/LabelMe/Images/

        static void Main(string[] args)
        {
            for (var input = args.FirstOrDefault(); input != "exit"; input = Console.ReadLine())
            {
                switch (input)
                {
                    case "dir":
                        var items = GetHyperlinks(url);
                        break;
                    case "grab":
                        Console.WriteLine("grab them!");
                        break;
                    default:
                        Console.WriteLine(@"Enter one of following arguments:");
                        Console.WriteLine(@"dir: List directories");
                        Console.WriteLine(@"grab: Download all files encountered in root directory");
                        Console.WriteLine(@"exit: Exit application");
                        break;
                }
            }

            //string[] names = { "Hartono ahahah", "Tommy", "Adams, Terry",
            //"Andersen, Henriette Thaulow",
            //"Hedlund, Magnus", "Ito, Shu" };
            //string firstLongName = names.FirstOrDefault(name => name.Length > len);
            //Console.WriteLine("Name longer than {0} characters: {1}", len, string.IsNullOrEmpty(firstLongName) ? "not found" : firstLongName);

            Console.WriteLine(string.Empty);
            //Console.ReadKey();

        }

        private static object GetHyperlinks(string url)
        {
            //url = url + "test";

            var hyperlinkList = new List<string>();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string html = reader.ReadToEnd();

                    //Console.WriteLine(html);

                    Regex regex = new Regex(GetDirectoryListingRegexForUrl(url));

                    MatchCollection matches = regex.Matches(html);

                    if (matches.Count > 0)
                    {
                        foreach (Match match in matches)
                        {
                            if (match.Success)
                            {
                                //Console.WriteLine(match.Groups["name"]);
                                var tt = match.Groups["name"];
                                hyperlinkList.Add(match.Groups["name"].ToString());
                            }
                        }
                    }
                }
            }


            return hyperlinkList;
        }

        private static string GetDirectoryListingRegexForUrl(string url)
        {
            return "<a href=\".*\">(?<name>.*)</a>";
        }
    }
}
