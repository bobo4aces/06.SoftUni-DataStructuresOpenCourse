using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.Calculate_Sequence
{
    class Program
    {
        static void Main(string[] args)
        {
            int number = int.Parse(Console.ReadLine());
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(number);
            int index = 0;
            int[] result = new int[50];
            while (queue.Count > 0)
            {
                int current = queue.Dequeue();
                result[index++] = current;
                if (index == 50)
                {
                    break;
                }

                queue.Enqueue(current + 1);
                queue.Enqueue(2 * current + 1);
                queue.Enqueue(current + 2);
            }
            Console.WriteLine(string.Join(", ", result));
        }
    }
}
