using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bitte geb eine Zahl zwischen 0 und 1000000 ein");
            int num = int.Parse(Console.ReadLine());
            Stopwatch s = new Stopwatch();
            s.Start();
            for (int i = 0; i < 1000000; i++)
            {
                if (i == num)
                {
                    s.Stop();
                    Console.WriteLine(s.Elapsed);
                    break;
                }
            }
            Console.ReadKey();
        }
    }
}
