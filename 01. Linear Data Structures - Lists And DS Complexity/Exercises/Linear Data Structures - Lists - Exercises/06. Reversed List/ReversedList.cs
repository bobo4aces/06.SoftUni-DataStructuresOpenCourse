using System;
using System.Collections;
using System.Collections.Generic;

namespace _06._Reversed_List
{
    public class ReversedList<T> : IEnumerable<T>
    {
        private const int defaultCapacity = 2;
        private T[] array;
        private int capacity;
        private int nextIndex;
        public ReversedList(int capacity = defaultCapacity)
        {
            this.array = new T[capacity];
            this.capacity = capacity;
            this.nextIndex = 0;
        }

        public void Add(T item)
        {
            if (this.nextIndex == this.capacity)
            {
                this.Resize();
            }
            this.array[this.nextIndex++] = item;
        }

        public int Count()
        {
            if (this.nextIndex != 0)
            {
                return this.nextIndex + 1;
            }
            return 0;
        }

        public int Capacity()
        {
            return this.capacity;
        }

        public T this[int index]
        {
            get
            {
                return this.array[index];
            }
            private set
            {
                if (index < 0 || index > this.nextIndex)
                {
                    throw new ArgumentOutOfRangeException("The index is out of range!");
                }
                this.array[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index > this.nextIndex)
            {
                throw new ArgumentOutOfRangeException("The index is out of range!");
            }
            for (int i = index; i < this.nextIndex; i++)
            {
                this.array[i] = this.array[i + 1];
            }
            this.array[this.nextIndex] = default(T);
            this.nextIndex--;
        }
        private void Resize()
        {
            T[] newArray = new T[this.capacity *= 2];
            this.array.CopyTo(newArray, 0);
            this.array = newArray;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = this.nextIndex - 1; i >= 0; i--)
            {
                yield return this.array[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
