using System;
using System.Linq;

public class ArrayStack<T>
{
    private const int initialCapacity = 16;

    private T[] elements;
    public int Count { get; private set; }

    public ArrayStack(int capacity = initialCapacity)
    {
        this.elements = new T[capacity];
        this.Count = 0;
    }

    public void Push(T element)
    {
        if (this.elements.Length == this.Count)
        {
            this.Grow();
        }
        elements[this.Count] = element;
        this.Count++;
    }

    public T Pop()
    {
        if (this.Count == 0)
        {
            throw new InvalidOperationException("Stack is empty!");
        }
        T value = this.elements[this.Count - 1];
        this.elements[this.Count - 1] = default(T);
        this.Count--;
        return value;
    }

    public T[] ToArray()
    {
        T[] array = new T[this.Count];
        for (int i = this.Count - 1; i >= 0; i--)
        {
            array[i] = this.elements[this.Count - 1 - i];
        }
        return array;
    }

    private void Grow()
    {
        T[] newArray = new T[this.elements.Length * 2];
        Array.Copy(this.elements, newArray, this.Count);
        this.elements = newArray;
    }
}
