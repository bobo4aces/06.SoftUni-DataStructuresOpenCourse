using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06.SequenceN_M
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] inputArgs = Console.ReadLine()
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();
            int start = inputArgs[0];
            int end = inputArgs[1];

            Queue<Item<int>> queue = new Queue<Item<int>>();
            Item<int> startItem = new Item<int>(start);
            queue.Enqueue(startItem);

            while (queue.Count != 0)
            {
                Item<int> currentNumber = queue.Dequeue();
                if (currentNumber.Value < end)
                {
                    queue.Enqueue(new Item<int>(currentNumber.Value + 1, currentNumber));
                    queue.Enqueue(new Item<int>(currentNumber.Value + 2, currentNumber));
                    queue.Enqueue(new Item<int>(currentNumber.Value * 2, currentNumber));
                }
                if (currentNumber.Value == end)
                {
                    List<int> result = new List<int>();
                    while (currentNumber != null)
                    {
                        result.Add(currentNumber.Value);
                        currentNumber = currentNumber.PrevItem;
                    }
                    result.Reverse();
                    Console.WriteLine(string.Join(" -> ", result));
                    return;
                }
            }
        }
        private class Item<T>
        {
            public T Value { get; private set; }
            public Item<T> PrevItem { get; private set; }

            public Item(T value)
            {
                this.Value = value;
            }
            public Item(T value, Item<T> prevItem)
            {
                this.Value = value;
                this.PrevItem = prevItem;
            }
        }
    }
}
