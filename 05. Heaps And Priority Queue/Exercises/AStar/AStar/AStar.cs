using System;
using System.Collections.Generic;
using System.Linq;

public class AStar
{
    private char[,] maze;
    private PriorityQueue<Node> priorityQueue;
    private Dictionary<Node, Node> parents;
    private Dictionary<Node, int> cost;
    public AStar(char[,] map)
    {
        this.maze = map;
        this.priorityQueue = new PriorityQueue<Node>();
        this.parents = new Dictionary<Node, Node>();
        this.cost = new Dictionary<Node, int>();
    }

    public static int GetH(Node current, Node goal)
    {
        int deltaX = Math.Abs(current.Col - goal.Col);
        int deltaY = Math.Abs(current.Row - goal.Row);

        return deltaX + deltaY;
    }

    public IEnumerable<Node> GetPath(Node start, Node goal)
    {
        this.priorityQueue.Enqueue(start);
        this.parents[start] = null;
        this.cost[start] = 0;

        while (this.priorityQueue.Count > 0)
        {
            Node current = this.priorityQueue.Dequeue();
            if (current.Equals(goal))
            {
                break;
            }
            foreach (var neighbor in Neighbors(current))
            {
                int newCost = this.cost[current] + 1;
                if (!this.cost.ContainsKey(neighbor) || newCost < this.cost[neighbor])
                {
                    this.cost[neighbor] = newCost;
                    neighbor.F = newCost + GetH(neighbor, goal);
                    this.priorityQueue.Enqueue(neighbor);
                    this.parents[neighbor] = current;
                }
            }
        }
        if (!this.parents.ContainsKey(goal))
        {
            return new List<Node>() { start };
        }
        return this.GetPath(goal);
    }

    private IEnumerable<Node> GetPath(Node goal)
    {
        Stack<Node> path = new Stack<Node>();
        path.Push(goal);
        Node current = this.parents[goal];
        while (current != null)
        {
            path.Push(current);
            current = this.parents[current];
        }
        return path;
    }

    private List<Node> Neighbors(Node node)
    {
        Node up = new Node(node.Row + 1, node.Col);
        Node right = new Node(node.Row, node.Col + 1);
        Node down = new Node(node.Row - 1, node.Col);
        Node left = new Node(node.Row, node.Col - 1);
        List<Node> nodes = new List<Node>();
        this.AddValidNodes(nodes, up);
        this.AddValidNodes(nodes, right);
        this.AddValidNodes(nodes, down);
        this.AddValidNodes(nodes, left);
        return nodes;
    }

    private bool IsValidNode(Node node)
    {
        return node.Row >= 0 && node.Row < this.maze.GetLength(0) &&
               node.Col >= 0 && node.Col < this.maze.GetLength(1) &&
               this.maze[node.Row, node.Col] != 'W';
    }

    private void AddValidNodes(List<Node> nodes, Node current)
    {
        if (IsValidNode(current))
        {
            nodes.Add(current);
        }
    }
}

