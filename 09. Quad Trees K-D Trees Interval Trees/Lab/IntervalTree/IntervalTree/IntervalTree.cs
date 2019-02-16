using System;
using System.Collections.Generic;

public class IntervalTree
{
    private class Node
    {
        internal Interval interval;
        internal double max;
        internal Node right;
        internal Node left;

        public Node(Interval interval)
        {
            this.interval = interval;
            this.max = interval.Hi;
        }
    }

    private Node root;

    public void Insert(double lo, double hi)
    {
        this.root = this.Insert(this.root, lo, hi);
    }

    public void EachInOrder(Action<Interval> action)
    {
        EachInOrder(this.root, action);
    }

    public Interval SearchAny(double lo, double hi)
    {
        Node currentNode = this.root;
        while (currentNode != null && !currentNode.interval.Intersects(lo, hi))
        {
            if (currentNode.left != null && currentNode.left.max > lo)
            {
                currentNode = currentNode.left;
            }
            else
            {
                currentNode = currentNode.right;
            }
        }
        if (currentNode == null)
        {
            return null;
        }

        return currentNode.interval;
    }

    public IEnumerable<Interval> SearchAll(double lo, double hi)
    {
        IList<Interval> result = new List<Interval>();
        SearchAll(this.root, lo, hi, result);
        return result;
    }

    private void SearchAll(Node node, double lo, double hi, IList<Interval> result)
    {
        if (node == null)
        {
            return;
        }

        bool goLeft = node.left != null && node.left.max > lo;
        bool goRight = node.right != null && node.right.interval.Lo < hi;

        if (goLeft)
        {
            this.SearchAll(node.left, lo, hi, result);
        }
        if (node.interval.Intersects(lo, hi))
        {
            result.Add(node.interval);
        }
        if (goRight)
        {
            this.SearchAll(node.right, lo, hi, result);
        }
    }

    private void EachInOrder(Node node, Action<Interval> action)
    {
        if (node == null)
        {
            return;
        }

        EachInOrder(node.left, action);
        action(node.interval);
        EachInOrder(node.right, action);
    }

    private Node Insert(Node node, double lo, double hi)
    {
        if (node == null)
        {
            return new Node(new Interval(lo, hi));
        }

        int cmp = lo.CompareTo(node.interval.Lo);
        if (cmp < 0)
        {
            node.left = Insert(node.left, lo, hi);
        }
        else if (cmp > 0)
        {
            node.right = Insert(node.right, lo, hi);
        }
        
        return node;
    }

    private void UpdateMax(Node node)
    {
        Node maxChild = this.GetMax(node.left, node.right);
        node.max = this.GetMax(node, maxChild).max;
    }

    private Node GetMax(Node left, Node right)
    {
        if (left == null)
        {
            return right;
        }
        if (right == null)
        {
            return left;
        }
        return left.max > right.max ? left : right;
    }
}
