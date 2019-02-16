using System;
using System.Collections.Generic;
using System.Linq;

namespace _01._Sum_and_Average
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> list = Console.ReadLine()
                .Split(' ')
                .Select(int.Parse)
                .ToList();

            if (list.Count != 0)
            {
                Console.WriteLine($"Sum={list.Sum()}; Average={list.Average():f2}");
            }
            else
            {
                Console.WriteLine($"Sum=0; Average=0.00");
            }
            
        }
    }
}
