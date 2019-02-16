using System;
using System.Collections.Generic;

public class Tree<T>
{
    public T Value { get; set; }
    public IList<Tree<T>> Children { get; private set; }

    public Tree(T value)
    {
        this.Value = value;
        this.Children = new List<Tree<T>>();
    }

    public Tree(T value, params Tree<T>[] children)
    {
        this.Value = value;
        this.Children = new List<Tree<T>>();
        foreach (var child in children)
        {
            this.Children.Add(child);
        }
    }

    public void Print(int indent = 0)
    {
        Console.Write(new string(' ', 2 * indent));
        Console.WriteLine(this.Value);
        foreach (var child in this.Children)
        {
            child.Print(indent + 1);
        }
    }

    public void Each(Action<T> action)
    {
        action(this.Value);
        foreach (var child in this.Children)
        {
            child.Each(action);
        }
    }

    public IEnumerable<T> OrderDFS()
    {
        IList<T> result = new List<T>();
        this.DFS(this, result);
        return result;
    }

    private void DFS(Tree<T> tree, IList<T> result)
    {
        foreach (var child in tree.Children)
        {
            this.DFS(child, result);
        }
        result.Add(tree.Value);
    }

    public IEnumerable<T> OrderBFS()
    {
        IList<T> result = new List<T>();
        Queue<Tree<T>> queue = new Queue<Tree<T>>();
        queue.Enqueue(this);
        while (queue.Count > 0)
        {
            Tree<T> currentNode = queue.Dequeue();
            result.Add(currentNode.Value);
            foreach (var child in currentNode.Children)
            {
                queue.Enqueue(child);
            }
        }
        return result;
    }
}
