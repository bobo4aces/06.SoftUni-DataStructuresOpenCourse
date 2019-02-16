using System;
using System.Collections.Generic;
using System.Collections;

public class Hierarchy<T> : IHierarchy<T>
{
    private Node root;
    private IDictionary<T, Node> elements;
    public Hierarchy(T root)
    {
        this.root = new Node(root);
        this.elements = new Dictionary<T, Node>();
        this.elements.Add(root, this.root);
    }

    public int Count
    {
        get
        {
            return this.elements.Count;
        }
    }

    public void Add(T element, T child)
    {
        if (!this.elements.ContainsKey(element))
        {
            throw new ArgumentException("Element does not exist in the hierarchy!");
        }
        if (this.elements.ContainsKey(child))
        {
            throw new ArgumentException("Child already exists (duplicates are not allowed)!");
        }
        Node parent = this.elements[element];
        Node currentChild = new Node(child, parent);
        parent.Children.Add(currentChild);
        this.elements.Add(child, currentChild);
    }

    public void Remove(T element)
    {
        if (!this.elements.ContainsKey(element))
        {
            throw new ArgumentException("Element does not exist in the hierarchy!");
        }
        if (this.elements[element] == this.root)
        {
            throw new InvalidOperationException("Element is root node!");
        }
        Node currentNode = this.elements[element];
        foreach (var child in currentNode.Children)
        {
            currentNode.Parent.AddChild(child);
            child.AddParent(currentNode.Parent);
        }
        currentNode.Parent.RemoveChild(currentNode);
        this.elements.Remove(currentNode.Value);
    }

    public IEnumerable<T> GetChildren(T item)
    {
        if (!this.elements.ContainsKey(item))
        {
            throw new ArgumentException("Element does not exist in the hierarchy!");
        }
        Node currentNode = this.elements[item];
        List<T> children = new List<T>();
        foreach (var child in currentNode.Children)
        {
            children.Add(child.Value);
        }
        return children;
    }

    public T GetParent(T item)
    {
        if (!this.elements.ContainsKey(item))
        {
            throw new ArgumentException("Element does not exist in the hierarchy!");
        }
        Node currentNode = this.elements[item];
        if (currentNode.Parent == null)
        {
            return default(T);
        }
        return currentNode.Parent.Value;
    }

    public bool Contains(T value)
    {
        return this.elements.ContainsKey(value);
    }

    public IEnumerable<T> GetCommonElements(Hierarchy<T> other)
    {
        IList<T> collection = new List<T>();
        foreach (var element in this.elements.Keys)
        {
            if (other.elements.ContainsKey(element))
            {
                collection.Add(element);
            }
        }
        return collection;
    }

    public IEnumerator<T> GetEnumerator()
    {
        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(this.root);
        while (queue.Count > 0)
        {
            Node currentNode = queue.Dequeue();
            foreach (var child in currentNode.Children)
            {
                queue.Enqueue(child);
            }
            yield return currentNode.Value;
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    private class Node
    {
        public T Value { get; private set; }
        public Node Parent { get; private set; }
        public IList<Node> Children { get; private set; }
        public Node(T value)
        {
            this.Value = value;
            this.Children = new List<Node>();
        }
        public Node(T value, Node parent)
            : this(value)
        {
            this.Parent = parent;
        }

        public void AddParent(Node parent)
        {
            this.Parent = parent;
        }
        public void AddChild(Node child)
        {
            this.Children.Add(child);
        }
        public void RemoveParent(Node parent)
        {
            this.Parent = null;
        }
        public void RemoveChild(Node child)
        {
            this.Children.Remove(child);
        }
    }
}