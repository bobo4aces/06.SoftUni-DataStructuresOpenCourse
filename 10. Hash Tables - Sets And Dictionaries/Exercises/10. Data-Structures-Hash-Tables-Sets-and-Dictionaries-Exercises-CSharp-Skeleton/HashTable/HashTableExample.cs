using System;
using System.Collections.Generic;
using System.Linq;

class Example
{
    static void Main()
    {
        string[] input = Console.ReadLine().Split('-').ToArray();

        HashTable<string, string> phonebook = new HashTable<string, string>();

        while (input[0] != "search")
        {
            string name = input[0];
            string number = input[1];
            if (!phonebook.ContainsKey(name))
            {
                phonebook.Add(name, number);
            }
            else
            {
                phonebook[name] = number;
            }
            input = Console.ReadLine().Split('-').ToArray();
        }
        input = Console.ReadLine().Split('-').ToArray();
        while (input[0] != "end")
        {
            string name = input[0];
            if (phonebook.ContainsKey(name))
            {
                Console.WriteLine($"{name} -> {phonebook[name]}");
            }
            else
            {
                Console.WriteLine($"Contact {name} does not exist.");
            }
            input = Console.ReadLine().Split('-').ToArray();
        }
        //char[] input = Console.ReadLine().ToCharArray();
        //HashTable<char, int> keyValues = new HashTable<char, int>();
        //foreach (var character in input)
        //{
        //    if (!keyValues.ContainsKey(character))
        //    {
        //        keyValues.Add(character, 0);
        //    }
        //    keyValues[character]++;
        //}
        //List<char> sortedKeys = new List<char>();
        //foreach (var element in keyValues)
        //{
        //    sortedKeys.Add(element.Key);
        //}
        //sortedKeys.Sort();
        //foreach (var sortedKey in sortedKeys)
        //{
        //    Console.WriteLine($"{sortedKey}: {keyValues[sortedKey]} time/s");
        //}

        //HashTable<string, int> grades = new HashTable<string, int>();
        //
        //Console.WriteLine("Grades:" + string.Join(",", grades));
        //Console.WriteLine("--------------------");
        //
        //grades.Add("Peter", 3);
        //grades.Add("Maria", 6);
        //grades["George"] = 5;
        //Console.WriteLine("Grades:" + string.Join(",", grades));
        //Console.WriteLine("--------------------");
        //
        //grades.AddOrReplace("Peter", 33);
        //grades.AddOrReplace("Tanya", 4);
        //grades["George"] = 55;
        //Console.WriteLine("Grades:" + string.Join(",", grades));
        //Console.WriteLine("--------------------");
        //
        //Console.WriteLine("Keys: " + string.Join(", ", grades.Keys));
        //Console.WriteLine("Values: " + string.Join(", ", grades.Values));
        //Console.WriteLine("Count = " + string.Join(", ", grades.Count));
        //Console.WriteLine("--------------------");
        //
        //grades.Remove("Peter");
        //grades.Remove("George");
        //grades.Remove("George");
        //Console.WriteLine("Grades:" + string.Join(",", grades));
        //Console.WriteLine("--------------------");
        //
        //Console.WriteLine("ContainsKey[\"Tanya\"] = " + grades.ContainsKey("Tanya"));
        //Console.WriteLine("ContainsKey[\"George\"] = " + grades.ContainsKey("George"));
        //Console.WriteLine("Grades[\"Tanya\"] = " + grades["Tanya"]);
        //Console.WriteLine("--------------------");
    }
}
