using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MsOwinDemo
{
    class Program
    {
        static string baseUrl = "http://localhost:8484";

        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>(baseUrl))
            {
                Console.WriteLine("WebApi started at " + baseUrl);
                Console.SetCursorPosition(0, Console.WindowHeight - 2);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Press ESC to quit.");
                Console.ResetColor();

                ConsoleKey ck;
                do
                {
                    ck = Console.ReadKey(true).Key;

                    if (ck.Equals(ConsoleKey.A))
                    {
                        GetTown().Wait();
                        
                        
                    }
                    else
                    {
                        Console.SetCursorPosition(0, Console.WindowHeight - 4);
                        Console.WriteLine("Command not recognized");
                    }

                    Console.SetCursorPosition(0, Console.WindowHeight - 2);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Press ESC to quit.");
                    Console.ResetColor();

                    //while (!Console.KeyAvailable) {}

                } while (ck != ConsoleKey.Escape);
            }
        }

        private static async Task GetTown()
        {
            string aa = await GetTownsStartWith("Amster");
            Console.SetCursorPosition(0, Console.WindowHeight - 4);
            Console.WriteLine(aa);
        }

        private static async Task<string> GetTownsStartWith(string townName)
        {
            using (var client = new HttpClient()) // { BaseAddress = new Uri($"http://localhost:{8484}") }
            {
                StringContent requestBody = new StringContent($"{{ \"town\": \"{townName}\" }}");

                requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync($"{baseUrl}/api/town/find", requestBody);

                return response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
