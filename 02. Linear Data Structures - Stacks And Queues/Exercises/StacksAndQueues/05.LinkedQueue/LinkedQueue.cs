using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class LinkedQueue<T>
{
    private QueueNode<T> head;
    private QueueNode<T> tail;
    public int Count { get; private set; }
    public void Enqueue(T element)
    {
        QueueNode<T> currentQueueNode = new QueueNode<T>(element);
        if (this.Count == 0)
        {
            this.head = this.tail = currentQueueNode;
        }
        else
        {
            this.tail.NextNode = currentQueueNode;
            this.tail = currentQueueNode;
        }
        this.Count++;
    }

    public T Dequeue()
    {
        if (this.Count == 0)
        {
            throw new InvalidOperationException("Linked queue is empty");
        }
       
        T value = this.head.Value;
        this.head = this.head.NextNode;
        this.head.PrevNode = null;

        this.Count--;
        return value;
    }

    public T[] ToArray()
    {
        T[] array = new T[this.Count];
        QueueNode<T> currentQueueNode = this.head;
        int index = 0;
        while (currentQueueNode != null)
        {
            array[index++] = currentQueueNode.Value;
            currentQueueNode = currentQueueNode.NextNode;
        }
        return array;
    }

    private class QueueNode<T>
    {
        public T Value { get; private set; }
        public QueueNode<T> NextNode { get; set; }
        public QueueNode<T> PrevNode { get; set; }

        public QueueNode(T value)
        {
            this.Value = value;
        }
    }
}

