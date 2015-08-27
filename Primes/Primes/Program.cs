using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Primes
{
    class Program
    {
        public static List<long> Primes { get; set; }

        static void Main(string[] args)
        {
            Primes = new List<long>();
            //PrimeList einlesen
            foreach (string line in File.ReadAllLines(@"C://users/julian/desktop/primes.txt"))
            {
                if (!String.IsNullOrEmpty(line))
                    Primes.Add(Convert.ToInt64(line));
            }
            Primes.Sort();
            long last = Primes[Primes.Count - 1] + 1;
            while (true)
            {
                bool isPrime = true;
                foreach (long prime in Primes)
                {
                    if (last % prime == 0)
                    {
                        isPrime = false;
                        break;
                    }
                    if (isPrime)
                    {
                        Console.WriteLine(last);
                        Primes.Add(last);
                        using (StreamWriter sw = new StreamWriter(@"C://users/julian/desktop/primes.txt", true))
                        {
                            sw.WriteLine("\n" + last.ToString());
                        }
                        break;
                    }
                }
                last++;
            }
        }
    }
}
