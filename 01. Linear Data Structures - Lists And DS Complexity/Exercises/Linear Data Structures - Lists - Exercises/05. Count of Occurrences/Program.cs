using System;
using System.Collections.Generic;
using System.Linq;

namespace _05._Count_of_Occurrences
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> list = Console.ReadLine()
                .Split(' ')
                .Select(int.Parse)
                .ToList();

            Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();

            for (int i = 0; i < list.Count; i++)
            {
                if (!keyValuePairs.ContainsKey(list[i]))
                {
                    keyValuePairs.Add(list[i], 0);
                }
                keyValuePairs[list[i]]++;
            }

            foreach (var keyValuePair in keyValuePairs.OrderBy(n=>n.Key))
            {
                Console.WriteLine($"{keyValuePair.Key} -> {keyValuePair.Value} times");
            }
        }
    }
}
