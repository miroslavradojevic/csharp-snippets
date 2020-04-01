using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interaction
{
    class Program
    {
        static void Main(string[] args)
        {
            for (var input = args.FirstOrDefault(); input != "exit"; input = Console.ReadLine())
            {
                switch (input)
                {
                    case "Amsterdam":
                    case "Berlin":
                        Console.WriteLine(input);
                        break;
                    default:
                        Console.WriteLine(@"Enter one of the arguments:");
                        Console.WriteLine(@"Amsterdam");
                        Console.WriteLine(@"Berlin");
                        Console.WriteLine(@"exit" + "\n");
                        break;
                }
            }
        }
    }
}
