// 
using System;
using System.Threading;

namespace Chapter1
{
    public static class Listing_1_1
    {
        public static void ThreadMethod()
        {
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine("ThreadProc: {0} ", i);
                Thread.Sleep(0); // signal windows that this thread is finished, signal that it is possible to switch to another thread
            }
        }
        public static void Main() // string[] args
        {
            Thread t = new Thread(new ThreadStart(ThreadMethod));
            t.Start();
                
            for (int i = 0; i < 100; i++)
            {
                Console.Write("Main thread: {0} ", i);
                Thread.Sleep(0);
            }

            t.Join();

            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }
    }
}
