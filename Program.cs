using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseUrl = "http://localhost:9090"; // webserver running on port 9090
            using (WebApp.Start<Startup>(baseUrl))
            {
                Console.WriteLine(baseUrl);
                Process.Start(baseUrl); // open the page from the application
                Console.WriteLine("Press Enter to quit.");
                Console.ReadKey();
            }
        }
    }
}

