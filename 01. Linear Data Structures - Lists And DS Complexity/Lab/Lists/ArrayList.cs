using System;

public class ArrayList<T>
{
    public int Count { get; set; }

    private T[] array;

    public ArrayList()
    {
        this.array = new T[2];
        this.Count = 0;
    }
    public T this[int index]
    {
        get
        {
            return this.array[index];
        }

        set
        {
            if (index < 0 || index >= this.Count)
            {
                throw new ArgumentOutOfRangeException("The index is out of range");
            }
            this.array[index] = value;
        }
    }

    public void Add(T item)
    {
        if (this.Count == this.array.Length)
        {
            this.ResizeArray();
        }
        this.array[this.Count++] = item;
    }

    private void ResizeArray()
    {
        T[] newArray = new T[this.Count * 2];
        this.array.CopyTo(newArray, 0);
        this.array = newArray;
    }

    public T RemoveAt(int index)
    {
        if (index < 0 || index >= this.Count)
        {
            throw new ArgumentOutOfRangeException("The index is out of range!");
        }
        T valueToRemove = this.array[index];
        for (int i = index; i < this.Count; i++)
        {
            this.array[i] = this.array[i + 1];
        }
        this.array[this.Count] = default(T);
        this.Count--;
        return valueToRemove;
    }
}
