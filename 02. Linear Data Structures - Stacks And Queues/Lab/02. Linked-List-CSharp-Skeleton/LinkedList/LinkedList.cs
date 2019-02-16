using System;
using System.Collections;
using System.Collections.Generic;

public class LinkedList<T> : IEnumerable<T>
{
    
    public Node Head { get; private set; }
    public Node Tail { get; private set; }
    public int Count { get; private set; }

    public void AddFirst(T item)
    {
        Node oldNode = this.Head;
        this.Head = new Node(item);
        this.Head.Next = oldNode;

        if (this.IsEmpty())
        {
            this.Tail = this.Head;
        }
        this.Count++;
        
    }

    private bool IsEmpty()
    {
        if (this.Count == 0)
        {
            return true;
        }
        return false;
    }

    public void AddLast(T item)
    {
        Node oldNode = this.Tail;
        this.Tail = new Node(item);

        if (this.IsEmpty())
        {
            this.Head = this.Tail;
        }
        else
        {
            oldNode.Next = this.Tail;
        }
        this.Count++;
    }

    public T RemoveFirst()
    {
        if (this.IsEmpty())
        {
            throw new InvalidOperationException("The list is empty!");
        }

        T removedValue = this.Head.Value;
        this.Head = this.Head.Next;
        this.Count--;

        if (this.IsEmpty())
        {
            this.Tail = null;
        }

        return removedValue;
    }

    public T RemoveLast()
    {
        if (this.IsEmpty())
        {
            throw new InvalidOperationException("The list is empty!");
        }

        T removedValue = this.Tail.Value;

        if (this.Count == 1)
        {
            this.Head = null;
            this.Tail = null;
        }
        else
        {
            Node newTail = this.GetSecondToLast();
            newTail.Next = null;
            this.Tail = newTail;
        }

        this.Count--;

        return removedValue;
    }

    private Node GetSecondToLast()
    {
        Node currentNode = this.Head;
        while (currentNode.Next != this.Tail)
        {
            currentNode = currentNode.Next;
        }
        return currentNode;
    }

    public IEnumerator<T> GetEnumerator()
    {
        Node currentNode = this.Head;

        while (currentNode != null)
        {
            yield return currentNode.Value;
            currentNode = currentNode.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    public class Node
    {
        public Node(T value)
        {
            this.Value = value;
        }

        public T Value { get; set; }
        public Node Next { get; set; }
    }
}
