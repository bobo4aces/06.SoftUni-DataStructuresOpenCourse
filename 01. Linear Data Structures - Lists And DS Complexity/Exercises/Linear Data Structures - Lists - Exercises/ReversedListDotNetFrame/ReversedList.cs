using System;
using System.Collections;
using System.Collections.Generic;

public class ReversedList<T> : IEnumerable<T>
{
    private const int defaultCapacity = 2;
    private T[] array;
    private int capacity;
    private int count;
    public ReversedList(int capacity = defaultCapacity)
    {
        this.array = new T[capacity];
        this.capacity = capacity;
        this.count = 0;
    }

    public void Add(T item)
    {
        if (this.count == this.capacity)
        {
            this.Resize();
        }
        this.array[this.count++] = item;
    }
    public int Count
    {
        get
        {
            return this.count;
        }
        private set
        {
            this.count = value;
        }
    }

    public int Capacity
    {
        get
        {
            return this.capacity;
        }
        private set
        {
            this.capacity = value;
        }
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= this.count)
            {
                throw new ArgumentOutOfRangeException("The index is out of range!");
            }
            return this.array[(this.Count - 1) - index];
        }
        private set
        {
            if (index < 0 || index >= this.count)
            {
                throw new ArgumentOutOfRangeException("The index is out of range!");
            }
            this.array[(this.Count - 1) - index] = value;
        }
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= this.count)
        {
            throw new ArgumentOutOfRangeException("The index is out of range!");
        }
        index = (this.Count - 1) - index;
        for (int i = index; i < this.count - 1; i++)
        {
            this.array[i] = this.array[i + 1];
        }
        this.array[this.count-1] = default(T);
        this.count--;
    }

    private void Resize()
    {
        T[] newArray = new T[this.capacity *= 2];
        this.array.CopyTo(newArray, 0);
        this.array = newArray;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = this.count - 1; i >= 0; i--)
        {
            yield return this.array[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
