using System;
using System.Collections.Generic;

public class BinarySearchTree<T> where T : IComparable<T>
{
    private Node root;
    private class Node
    {
        public T Value { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }

        public Node(T value)
        {
            this.Value = value;
        }
    }

    public BinarySearchTree()
    {
        this.root = null;
    }
   
    public void Insert(T value)
    {
        if (this.root == null)
        {
            this.root = new Node(value);
            return;
        }
        Node parent = null;
        Node currentNode = this.root;
        while (currentNode != null)
        {
            int compare = value.CompareTo(currentNode.Value);

            if (compare < 0)
            {
                parent = currentNode;
                currentNode = currentNode.Left;
            }
            else if (compare > 0)
            {
                parent = currentNode;
                currentNode = currentNode.Right;
            }
            else
            {
                return;
            }
        }
        if (value.CompareTo(parent.Value) < 0)
        {
            parent.Left = new Node(value);
        }
        else if (value.CompareTo(parent.Value) > 0)
        {
            parent.Right = new Node(value);
        }
    }

    public bool Contains(T value)
    {
        Node currentNode = this.root;
        while (currentNode != null)
        {
            if (value.CompareTo(currentNode.Value) < 0)
            {
                currentNode = currentNode.Left;
            }
            else if (value.CompareTo(currentNode.Value) > 0)
            {
                currentNode = currentNode.Right;
            }
            else
            {
                break;
            }
        }
        return currentNode != null;
    }

    public void DeleteMin()
    {
        if (this.root == null)
        {
            return;
        }
        Node parent = null;
        Node minNode = this.root;
        while (minNode.Left != null)
        {
            parent = minNode;
            minNode = minNode.Left;
        }
        if (parent == null)
        {
            this.root = minNode.Right;
        }
        else
        {
            parent.Left = minNode.Right;
        }
    }

    public BinarySearchTree<T> Search(T item)
    {
        Node currentNode = this.root;
        while (currentNode != null)
        {
            if (item.CompareTo(currentNode.Value) < 0)
            {
                currentNode = currentNode.Left;
            }
            else if (item.CompareTo(currentNode.Value) > 0)
            {
                currentNode = currentNode.Right;
            }
            else
            {
                break;
            }
        }
        if (currentNode == null)
        {
            return null;
        }
        return new BinarySearchTree<T>(currentNode);
    }

    private BinarySearchTree(Node root)
    {
        this.Copy(root);
    }

    private void Copy(Node node)
    {
        if (node == null)
        {
            return;
        }
        this.Insert(node.Value);
        this.Copy(node.Left);
        this.Copy(node.Right);
    }

    public IEnumerable<T> Range(T startRange, T endRange)
    {
        Queue<T> queue = new Queue<T>();
        this.Range(this.root, queue, startRange, endRange);
        return queue;
    }

    private void Range(Node node, Queue<T> queue, T startRange, T endRange)
    {
        if (node == null)
        {
            return;
        }

        int nodeInLowerRange = startRange.CompareTo(node.Value);
        int nodeInHigherRange = endRange.CompareTo(node.Value);

        if (nodeInLowerRange < 0)
        {
            this.Range(node.Left, queue, startRange, endRange);
        }
        if (nodeInLowerRange <= 0 && nodeInHigherRange >= 0)
        {
            queue.Enqueue(node.Value);
        }
        if (nodeInHigherRange > 0)
        {
            this.Range(node.Right, queue, startRange, endRange);
        }
    }

    public void EachInOrder(Action<T> action)
    {
        this.EachInOrder(this.root, action);
    }

    private void EachInOrder(Node node, Action<T> action)
    {
        if (node == null)
        {
            return;
        }
        this.EachInOrder(node.Left, action);
        action(node.Value);
        this.EachInOrder(node.Right, action);
    }
}

public class Launcher
{
    public static void Main(string[] args)
    {
    }
}
