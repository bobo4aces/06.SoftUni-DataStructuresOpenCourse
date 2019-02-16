using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01.Reverse_Numbers
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] inputArgs = Console.ReadLine()
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            Stack<int> numbers = new Stack<int>(inputArgs.Length);

            for (int i = 0; i < inputArgs.Length; i++)
            {
                numbers.Push(inputArgs[i]);
            }
            int[] output = new int[inputArgs.Length];

            for (int i = 0; i < inputArgs.Length; i++)
            {
                output[i] = numbers.Pop();
            }

            Console.WriteLine(string.Join(" ", output));
        }
    }
}
