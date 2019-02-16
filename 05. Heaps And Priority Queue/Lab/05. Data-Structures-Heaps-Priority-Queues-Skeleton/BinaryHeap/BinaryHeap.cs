using System;
using System.Collections.Generic;

public class BinaryHeap<T> where T : IComparable<T>
{
    private List<T> heap;

    public BinaryHeap()
    {
        this.heap = new List<T>();
    }

    public int Count
    {
        get
        {
            return this.heap.Count;
        }
    }

    public void Insert(T item)
    {
        this.heap.Add(item);
        int lastIndex = this.heap.Count - 1;
        this.HeapifyUp(lastIndex);
    }

    private void HeapifyUp(int childIndex)
    {
        int parentIndex = this.Parent(childIndex);
        bool isLess = this.IsLess(parentIndex, childIndex);

        while (childIndex > 0 && isLess)
        {
            this.Swap(parentIndex, childIndex);
            childIndex = this.Parent(childIndex);

            parentIndex = this.Parent(childIndex);
            isLess = this.IsLess(parentIndex, childIndex);
        }
    }

    private void Swap(int parentIndex, int childIndex)
    {
        T oldParent = this.heap[parentIndex];
        this.heap[parentIndex] = this.heap[childIndex];
        this.heap[childIndex] = oldParent;
    }

    private bool IsLess(int firstIndex, int secondIndex)
    {
        T firstElement = this.heap[firstIndex];
        T secondElement = this.heap[secondIndex];

        return firstElement.CompareTo(secondElement) < 0;
    }

    private int Parent(int childIndex)
    {
        if (childIndex > 0)
        {
            return (childIndex - 1) / 2;
        }
        return 0;
    }

    public T Peek()
    {
        if (this.heap.Count == 0)
        {
            throw new InvalidOperationException("Heap is empty");
        }
        return this.heap[0];
    }

    public T Pull()
    {
        if (this.heap.Count == 0)
        {
            throw new InvalidOperationException("Heap is empty!");
        }
        T elementToDelete = this.heap[0];
        int lastIndex = this.heap.Count - 1;
        this.Swap(0, lastIndex);
        this.heap.RemoveAt(lastIndex);
        this.HeapifyDown(0);
        return elementToDelete;
    }

    private void HeapifyDown(int parentIndex)
    {
        while (parentIndex < this.heap.Count / 2)
        {
            int childIndex = this.Child(parentIndex);
            bool isLess = this.IsLess(childIndex, parentIndex);
            if (isLess)
            {
                break;
            }
            this.Swap(parentIndex, childIndex);
            parentIndex = childIndex;
            
        }
    }

    private int Child(int parentIndex)
    {
        int leftChildIndex = parentIndex * 2 + 1;
        int childIndex = leftChildIndex;
        if (childIndex + 1 < this.heap.Count)
        {
            int rightChildIndex = parentIndex * 2 + 2;

            if (this.IsLess(leftChildIndex, rightChildIndex))
            {
                childIndex = rightChildIndex;
            }
        }
        return childIndex;
    }
}
