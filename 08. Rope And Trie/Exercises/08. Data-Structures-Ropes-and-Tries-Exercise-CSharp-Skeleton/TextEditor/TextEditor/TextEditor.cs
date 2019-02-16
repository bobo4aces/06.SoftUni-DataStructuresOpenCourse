using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wintellect.PowerCollections;

namespace TextEditor
{
    public class TextEditor : ITextEditor
    {
        private Trie<BigList<char>> usersStrings;
        private Trie<Stack<string>> oldStrings;
        private HashSet<string> users;

        public TextEditor()
        {
            this.usersStrings = new Trie<BigList<char>>();
            this.oldStrings = new Trie<Stack<string>>();
            this.users = new HashSet<string>();
        }
        public void Clear(string username)
        {
            if (!this.users.Contains(username))
            {
                return;
            }
            BigList<char> stringsByUser = this.usersStrings.GetValue(username);
            this.AddOldStrings(username, stringsByUser);

            stringsByUser.Clear();
        }

        private void AddOldStrings(string username, BigList<char> bigList)
        {
            string currentString = string.Join("", bigList);
            this.oldStrings.GetValue(username).Push(currentString);
        }

        public void Delete(string username, int startIndex, int length)
        {
            if (!this.users.Contains(username))
            {
                return;
            }
            BigList<char> stringsByUser = this.usersStrings.GetValue(username);

            this.AddOldStrings(username, stringsByUser);

            stringsByUser.RemoveRange(startIndex, length);
        }

        public void Insert(string username, int index, string str)
        {
            if (!this.users.Contains(username))
            {
                return;
            }
            BigList<char> stringsByUser = this.usersStrings.GetValue(username);
            string oldStrings = string.Join("", stringsByUser);
            if (oldStrings.Length >= index)
            {
                this.AddOldStrings(username, stringsByUser);
                stringsByUser.InsertRange(index,str);
            }
            
        }

        public int Length(string username)
        {
            if (!this.users.Contains(username))
            {
                return 0;
            }
            return this.usersStrings.GetValue(username).Count;
        }

        public void Login(string username)
        {
            this.usersStrings.Insert(username, new BigList<char>());
            this.users.Add(username);
            this.oldStrings.Insert(username, new Stack<string>());
        }

        public void Logout(string username)
        {
            this.usersStrings.Delete(username);
            this.oldStrings.Delete(username);
            this.users.Remove(username);
        }

        public void Prepend(string username, string str)
        {
            if (!this.users.Contains(username))
            {
                return;
            }
            BigList<char> currentStrings = this.usersStrings.GetValue(username);
            this.AddOldStrings(username, currentStrings);
            currentStrings.InsertRange(0, str);
        }

        public string Print(string username)
        {
            if (!this.users.Contains(username))
            {
                return null;
            }
            BigList<char> currentStrings = this.usersStrings.GetValue(username);
            return string.Join("", currentStrings);
        }

        public void Substring(string username, int startIndex, int length)
        {
            if (!this.users.Contains(username))
            {
                return;
            }
            BigList<char> currentStrings = this.usersStrings.GetValue(username);
            this.AddOldStrings(username, currentStrings);
            BigList<char> substring = currentStrings.GetRange(startIndex, length);
            currentStrings.Clear();
            currentStrings.AddRange(substring);
        }

        public void Undo(string username)
        {
            if (!this.users.Contains(username))
            {
                return;
            }
            Stack<string> currentUserStrings = this.oldStrings.GetValue(username);
            if (currentUserStrings.Count == 0)
            {
                return;
            }
            string lastString = currentUserStrings.Pop();
            BigList<char> currentString = this.usersStrings.GetValue(username);
            currentString.Clear();
            currentString.AddRange(lastString);
        }

        public IEnumerable<string> Users(string prefix = "")
        {
            if (prefix != string.Empty)
            {
                foreach (var user in this.usersStrings.GetByPrefix(prefix))
                {
                    yield return user;
                }
            }
            else
            {
                foreach (var user in this.users)
                {
                    yield return user;
                }
            }
        }
    }
}
