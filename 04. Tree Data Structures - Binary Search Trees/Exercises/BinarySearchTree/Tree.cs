using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySearchTree
{
    public class Tree<T>
    {
        public T Value { get; set; }
        public Tree<T> Parent { get; set; }
        public List<Tree<T>> Children { get; private set; }

        public Tree(T value, params Tree<T>[] children)
        {
            this.Value = value;
            this.Children = new List<Tree<T>>();
            foreach (var child in children)
            {
                this.Children.Add(child);
                child.Parent = this;
            }
        }

        public void EachPreOrder(Action<T> action)
        {
            this.EachPreOrder(this.Parent, action);
        }

        private void EachPreOrder(Tree<T> parent, Action<T> action)
        {
            action(this.Value);
            foreach (var child in this.Children)
            {
                child.EachPreOrder(action);
            }
        }

        public void PrintEachPreOrder(int indent = 0)
        {
            Console.Write(new string(' ', 2 * indent));
            Console.WriteLine(this.Value);
            foreach (var child in this.Children)
            {
                child.PrintEachPreOrder(indent + 1);
            }
        }

        public List<T> GetLeafNodes()
        {
            List<T> leafNodes = new List<T>();
            GetLeafNodes(leafNodes);
            
            return leafNodes;
        }

        private void GetLeafNodes(List<T> leafNodes)
        {
            if (this.Children.Count == 0)
            {
                leafNodes.Add(this.Value);
                return;
            }
            foreach (var child in this.Children)
            {
                child.GetLeafNodes(leafNodes);
            }
        }

        public List<T> GetMiddleNodes()
        {
            List<T> middleNodes = new List<T>();
            GetMiddleNodes(middleNodes);

            return middleNodes;
        }

        private void GetMiddleNodes(List<T> middleNodes)
        {
            if (this.Children.Count > 0 && this.Parent != null)
            {
                middleNodes.Add(this.Value);
            }
            foreach (var child in this.Children)
            {
                child.GetMiddleNodes(middleNodes);
            }
        }
    }
}
