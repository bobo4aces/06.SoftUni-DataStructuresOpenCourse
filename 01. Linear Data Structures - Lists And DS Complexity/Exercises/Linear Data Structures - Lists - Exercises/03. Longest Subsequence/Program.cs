using System;
using System.Collections.Generic;
using System.Linq;

namespace _03._Longest_Subsequence
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> list = Console.ReadLine()
                .Split(' ')
                .Select(int.Parse)
                .ToList();

            int startIndex = 0;
            int bestLength = 1;
            int currentLength = 1;

            for (int i = 1; i < list.Count; i++)
            {
                if (list[i-1] == list[i])
                {
                    currentLength++;
                }
                else
                {
                    currentLength = 1;
                    
                }
                if (currentLength > bestLength)
                {
                    bestLength = currentLength;
                    startIndex = i - bestLength + 1;
                }
            }

            List<int> result = new List<int>();

            for (int i = startIndex; i < startIndex + bestLength; i++)
            {
                result.Add(list[i]);
            }
            Console.WriteLine(string.Join(" ",result));
        }
    }
}
