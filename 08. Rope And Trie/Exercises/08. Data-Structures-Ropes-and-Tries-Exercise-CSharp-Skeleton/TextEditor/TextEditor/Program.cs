using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine();
            ITextEditor textEditor = new TextEditor();
            while (input != "end")
            {
                string currentString = string.Empty;
                if (input.Contains("\""))
                {
                    string[] currentInput = input
                        .Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToArray();
                    currentString = currentInput[1];
                    input = currentInput[0].ToString().Trim();
                }
                string[] commands = input
                    .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();
                switch (commands[0])
                {
                    case "login":
                        string username = commands[1];
                        textEditor.Login(username);
                        break;
                    case "logout":
                        username = commands[1];
                        textEditor.Logout(username);
                        break;
                    case "users":
                        string prefix = string.Empty;
                        if (commands.Length > 1)
                        {
                            prefix = commands[1];
                        }
                        foreach (var user in textEditor.Users(prefix))
                        {
                            Console.WriteLine(user);
                        }
                        break;
                    default:
                        username = commands[0];

                        if (commands[1] == "insert")
                        {
                            int index = int.Parse(commands[2]);
                            textEditor.Insert(username, index, currentString);
                        }
                        else if (commands[1] == "prepend")
                        {
                            textEditor.Prepend(username, currentString);
                        }
                        else if (commands[1] == "substring")
                        {
                            int startIndex = int.Parse(commands[2]);
                            int length = int.Parse(commands[3]);
                            textEditor.Substring(username, startIndex, length);
                        }
                        else if (commands[1] == "delete")
                        {
                            int startIndex = int.Parse(commands[2]);
                            int length = int.Parse(commands[3]);
                            textEditor.Delete(username, startIndex, length);
                        }
                        else if (commands[1] == "clear")
                        {
                            textEditor.Clear(username);
                        }
                        else if (commands[1] == "length")
                        {
                            int length = textEditor.Length(username);
                            Console.WriteLine(length);
                        }
                        else if (commands[1] == "print")
                        {
                            string print = textEditor.Print(username);
                            Console.WriteLine(print);
                        }
                        else if (commands[1] == "undo")
                        {
                            textEditor.Undo(username);
                        }
                        break;
                }
                input = Console.ReadLine();
            }
        }
    }
}
