using System;
using System.Collections.Generic;
using System.Linq;

namespace SweepAndPrune
{
    class Program
    {
        static void Main()
        {
            IList<Item> items = new List<Item>();
            IDictionary<string, Item> itemsById = new Dictionary<string, Item>();

            while (true)
            {
                string[] inputArgs = Console.ReadLine().Split(' ').ToArray();
                string command = inputArgs[0];
                switch (command)
                {
                    case "add":
                        AddItem(items, itemsById, inputArgs);
                        break;
                    case "start":
                        StartGame(items, itemsById);
                        return;
                }
            }
        }

        private static void AddItem(IList<Item> items, IDictionary<string, Item> itemsById, string[] inputArgs)
        {
            string name = inputArgs[1];
            int x = int.Parse(inputArgs[2]);
            int y = int.Parse(inputArgs[3]);

            Item item = new Item(x, y, name);

            items.Add(item);
            itemsById[name] = item;
        }

        private static void StartGame(IList<Item> items, IDictionary<string, Item> itemsById)
        {
            int ticks = 1;

            while (true)
            {
                string[] inputArgs = Console.ReadLine().Split(' ').ToArray();
                string command = inputArgs[0];

                if (command == "end")
                {
                    return;
                }

                if (command == "move")
                {
                    UpdateValues(itemsById, inputArgs);
                }

                InsertionSort(items);
                CheckIfIntersects(items, ticks++);
            }
        }

        private static void UpdateValues(IDictionary<string, Item> itemsById, string[] inputArgs)
        {
            string name = inputArgs[1];
            int x = int.Parse(inputArgs[2]);
            int y = int.Parse(inputArgs[3]);

            Item item = itemsById[name];

            item.X1 = x;
            item.Y1 = y;
        }

        private static void InsertionSort(IList<Item> items)
        {
            for (int i = 1; i < items.Count; i++)
            {
                int j = i - 1;
                while (j >= 0 && items[j].X1 > items[j + 1].X1)
                {
                    Item currentItem = items[j];
                    items[j] = items[j + 1];
                    items[j + 1] = currentItem;
                    j--;
                }
            }
        }

        private static void CheckIfIntersects(IList<Item> items, int ticks)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Item currentItem = items[i];
                for (int j = i + 1; j < items.Count; j++)
                {
                    Item candidate = items[j];
                    if (currentItem.X2 < candidate.X1)
                    {
                        break;
                    }
                    if (currentItem.Intersect(candidate))
                    {
                        Console.WriteLine($"({ticks}) {currentItem.Name} collides with {candidate.Name}");
                    }
                }
            }
        }
    }
}
