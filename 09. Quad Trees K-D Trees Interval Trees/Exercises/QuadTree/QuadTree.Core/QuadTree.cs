﻿using System;
using System.Collections.Generic;
using System.Linq;

public class QuadTree<T> where T : IBoundable
{
    public const int DefaultMaxDepth = 5;

    public readonly int MaxDepth;

    private Node<T> root;

    public QuadTree(int width, int height, int maxDepth = DefaultMaxDepth)
    {
        this.root = new Node<T>(0, 0, width, height);
        this.Bounds = this.root.Bounds;
        this.MaxDepth = maxDepth;
    }

    public int Count { get; private set; }

    public Rectangle Bounds { get; private set; }

    public void ForEachDfs(Action<List<T>, int, int> action)
    {
        this.ForEachDfs(this.root, action);
    }

    public bool Insert(T item)
    {
        if (!item.Bounds.IsInside(this.Bounds))
        {
            return false;
        }
        int depth = 1;
        Node<T> currentNode = this.root;
        while (currentNode.Children != null)
        {
            int quadrant = GetQuadrant(currentNode, item.Bounds);
            if (quadrant == -1)
            {
                break;
            }
            currentNode = currentNode.Children[quadrant];
            depth++;
        }
        currentNode.Items.Add(item);
        this.Split(currentNode, depth);
        this.Count++;
        return true;
    }

    private void Split(Node<T> node, int depth)
    {
        if (!(node.ShouldSplit && depth < MaxDepth))
        {
            return;
        }

        int leftWidth = node.Bounds.Width / 2;
        int rightWidth = node.Bounds.Width - leftWidth;
        int topHeight = node.Bounds.Height / 2;
        int bottomHeight = node.Bounds.Height - topHeight;

        node.Children = new Node<T>[4];
        node.Children[0] = new Node<T>(node.Bounds.MidX, node.Bounds.Y1, rightWidth, topHeight);
        node.Children[1] = new Node<T>(node.Bounds.X1, node.Bounds.Y1, leftWidth, topHeight);
        node.Children[2] = new Node<T>(node.Bounds.X1, node.Bounds.MidY, leftWidth, bottomHeight);
        node.Children[3] = new Node<T>(node.Bounds.MidX, node.Bounds.MidY, rightWidth, bottomHeight);

        for (int i = 0; i < node.Items.Count; i++)
        {
            T item = node.Items[i];
            int quadrant = GetQuadrant(node, item.Bounds);
            if (quadrant != -1)
            {
                node.Items.Remove(item);
                node.Children[quadrant].Items.Add(item);
                i--;
            }
        }

        foreach (var child in node.Children)
        {
            this.Split(child, depth + 1);
        }
    }

    private static int GetQuadrant(Node<T> node, Rectangle bounds)
    {
        if (node.Children != null)
        {
            if (bounds.IsInside(node.Children[0].Bounds)) return 0;
            if (bounds.IsInside(node.Children[1].Bounds)) return 1;
            if (bounds.IsInside(node.Children[2].Bounds)) return 2;
            if (bounds.IsInside(node.Children[3].Bounds)) return 3;
        }
        return -1;
    }

    public List<T> Report(Rectangle bounds)
    {
        List<T> collisionCandidates = new List<T>();
        GetCollisionCandidates(this.root, bounds, collisionCandidates);
        return collisionCandidates;
    }

    private static void GetCollisionCandidates(Node<T> node, Rectangle bounds, List<T> collisionCandidates)
    {
        int quadrant = GetQuadrant(node, bounds);
        if (quadrant == -1)
        {
            GetSubtreeContents(node, bounds, collisionCandidates);
        }
        else
        {
            if (node.Children != null)
            {
                GetCollisionCandidates(node.Children[quadrant], bounds, collisionCandidates);
               
            }
            collisionCandidates.AddRange(node.Items);
        }
    }

    private static void GetSubtreeContents(Node<T> node, Rectangle bounds, List<T> collisionCandidates)
    {
        if (node.Children != null)
        {
            foreach (var child in node.Children)
            {
                if (child.Bounds.Intersects(bounds))
                {
                    GetSubtreeContents(child, bounds, collisionCandidates);
                }
            }
        }

        collisionCandidates.AddRange(node.Items);
    }

    private void ForEachDfs(Node<T> node, Action<List<T>, int, int> action, int depth = 1, int quadrant = 0)
    {
        if (node == null)
        {
            return;
        }

        if (node.Items.Any())
        {
            action(node.Items, depth, quadrant);
        }

        if (node.Children != null)
        {
            for (int i = 0; i < node.Children.Length; i++)
            {
                ForEachDfs(node.Children[i], action, depth + 1, i);
            }
        }
    }
}
