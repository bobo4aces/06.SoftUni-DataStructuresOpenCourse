using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
public class LinkedStack<T>
{
    private Node<T> firstNode;
    public int Count { get; private set; }

    public void Push(T element)
    {
        this.firstNode = new Node<T>(element, this.firstNode);

        this.Count++;
    }

    public T Pop()
    {
        if (this.Count == 0)
        {
            throw new InvalidOperationException("Linked stack is empty!");
        }
        Node<T> node = this.firstNode;
        this.firstNode = this.firstNode.NextNode;
        this.Count--;
        return node.Value;
    }

    public T[] ToArray()
    {
        T[] array = new T[this.Count];
        int index = 0;
        Node<T> currentNode = this.firstNode;
        while (currentNode != null)
        {
            array[index++] = currentNode.Value;
            currentNode = currentNode.NextNode;
        }

        return array;
    }

    private class Node<T>
    {
        public T Value { get; set; }
        public Node<T> NextNode { get; set; }

        public Node(T value, Node<T> nextNode = null)
        {
            this.Value = value;
            this.NextNode = nextNode;
        }
    }
}

