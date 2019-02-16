using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

public class FirstLastList<T> : IFirstLastList<T> where T : IComparable<T>
{
    private LinkedList<T> byInsert;
    private OrderedBag<LinkedListNode<T>> byAsceningOrder;
    private OrderedBag<LinkedListNode<T>> byDesceningOrder;

    public FirstLastList()
    {
        this.byInsert = new LinkedList<T>();
        this.byAsceningOrder = new OrderedBag<LinkedListNode<T>>((x, y) => x.Value.CompareTo(y.Value));
        this.byDesceningOrder = new OrderedBag<LinkedListNode<T>>((x, y) => y.Value.CompareTo(x.Value));
    }
    public int Count
    {
        get
        {
            return this.byInsert.Count;
        }
    }

    public void Add(T element)
    {
        LinkedListNode<T> newNode = new LinkedListNode<T>(element);

        this.byAsceningOrder.Add(newNode);
        this.byDesceningOrder.Add(newNode);
        this.byInsert.AddLast(newNode);
    }

    public void Clear()
    {
        this.byInsert.Clear();
        this.byAsceningOrder.Clear();
        this.byDesceningOrder.Clear();
    }

    public IEnumerable<T> First(int count)
    {
        ValidateCount(count);

        return this.byInsert.Take(count);
    }

    public IEnumerable<T> Last(int count)
    {
        ValidateCount(count);

        return this.byInsert
                    .Reverse()
                    .Take(count);
    }

    public IEnumerable<T> Max(int count)
    {
        ValidateCount(count);

        return this.byDesceningOrder.Take(count).Select(x=>x.Value);
        
    }

    public IEnumerable<T> Min(int count)
    {
        ValidateCount(count);

        return this.byAsceningOrder
                    .Take(count)
                    .Select(v => v.Value);
    }

    
    public int RemoveAll(T element)
    {
        LinkedListNode<T> nodeToRemove = new LinkedListNode<T>(element);
        OrderedBag<LinkedListNode<T>>.View range = 
            this.byAsceningOrder.Range(nodeToRemove, true, nodeToRemove, true);
        foreach (var node in range)
        {
            this.byInsert.Remove(node);
        }
        int count = this.byDesceningOrder.RemoveAllCopies(nodeToRemove);
        this.byAsceningOrder.RemoveAllCopies(nodeToRemove);
        return count;
    }
    private void ValidateCount(int count)
    {
        if (count > this.Count)
        {
            throw new ArgumentOutOfRangeException("The structure holds less than count elements");
        }
    }
}
