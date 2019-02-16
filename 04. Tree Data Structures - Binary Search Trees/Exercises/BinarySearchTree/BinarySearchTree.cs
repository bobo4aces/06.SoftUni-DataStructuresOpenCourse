using BinarySearchTree;
using System;
using System.Collections.Generic;
using System.Linq;

public class BinarySearchTree<T> : IBinarySearchTree<T> where T : IComparable
{
    private Node root;

    private Node FindElement(T element)
    {
        Node current = this.root;

        while (current != null)
        {
            if (current.Value.CompareTo(element) > 0)
            {
                current = current.Left;
            }
            else if (current.Value.CompareTo(element) < 0)
            {
                current = current.Right;
            }
            else
            {
                break;
            }
        }

        return current;
    }

    private void PreOrderCopy(Node node)
    {
        if (node == null)
        {
            return;
        }

        this.Insert(node.Value);
        this.PreOrderCopy(node.Left);
        this.PreOrderCopy(node.Right);
    }

    private Node Insert(T element, Node node)
    {
        if (node == null)
        {
            node = new Node(element);
        }
        else if (element.CompareTo(node.Value) < 0)
        {
            node.Left = this.Insert(element, node.Left);
        }
        else if (element.CompareTo(node.Value) > 0)
        {
            node.Right = this.Insert(element, node.Right);
        }
        node.Count = 1 + this.Count(node.Left) + this.Count(node.Right);
        return node;
    }

    private void Range(Node node, Queue<T> queue, T startRange, T endRange)
    {
        if (node == null)
        {
            return;
        }

        int nodeInLowerRange = startRange.CompareTo(node.Value);
        int nodeInHigherRange = endRange.CompareTo(node.Value);

        if (nodeInLowerRange < 0)
        {
            this.Range(node.Left, queue, startRange, endRange);
        }
        if (nodeInLowerRange <= 0 && nodeInHigherRange >= 0)
        {
            queue.Enqueue(node.Value);
        }
        if (nodeInHigherRange > 0)
        {
            this.Range(node.Right, queue, startRange, endRange);
        }
    }

    private void EachInOrder(Node node, Action<T> action)
    {
        if (node == null)
        {
            return;
        }

        this.EachInOrder(node.Left, action);
        action(node.Value);
        this.EachInOrder(node.Right, action);
    }

    private BinarySearchTree(Node node)
    {
        this.PreOrderCopy(node);
    }

    public BinarySearchTree()
    {
    }

    public void Insert(T element)
    {
        this.root = this.Insert(element, this.root);
    }

    public bool Contains(T element)
    {
        Node current = this.FindElement(element);

        return current != null;
    }

    public void EachInOrder(Action<T> action)
    {
        this.EachInOrder(this.root, action);
    }

    public BinarySearchTree<T> Search(T element)
    {
        Node current = this.FindElement(element);

        return new BinarySearchTree<T>(current);
    }

    public void DeleteMin()
    {
        if (this.root == null)
        {
            throw new InvalidOperationException("The tree is empty");
        }
        this.root = this.DeleteMin(this.root);
    }

    private Node DeleteMin(Node node)
    {
        if (node.Left == null)
        {
            return node.Right;
        }
        node.Left = this.DeleteMin(node.Left);
        node.Count = 1 + this.Count(node.Left) + this.Count(node.Right);
        return node;
    }

    public IEnumerable<T> Range(T startRange, T endRange)
    {
        Queue<T> queue = new Queue<T>();

        this.Range(this.root, queue, startRange, endRange);

        return queue;
    }

    public void Delete(T element)
    {
        if (this.Count(this.root) == 0 || !this.Contains(element))
        {
            throw new InvalidOperationException();
        }
        this.root = this.Delete(this.root, element);
    }

    private Node Delete(Node node, T element)
    {
        if (node == null)
        {
            return null;
        }
        int compare = element.CompareTo(node.Value);
        if (compare < 0)
        {
            node.Left = this.Delete(node.Left, element);
        }
        else if (compare > 0)
        {
            node.Right = this.Delete(node.Right, element);
        }
        else
        {
            if (node.Left == null)
            {
                return node.Right;
            }
            if (node.Right == null)
            {
                return node.Left;
            }
            Node leftmost = this.SubtreeLeftmost(node.Right);

            node.Value = leftmost.Value;
            node.Right = this.Delete(node.Right, leftmost.Value);
        }
        node.Count = 1 + this.Count(node.Left) + this.Count(node.Right);
        return node;
    }
    private Node SubtreeLeftmost(Node node)
    {
        Node current = node;

        while (current.Left != null)
        {
            current = current.Left;
        }
        return current;
    }
    public void DeleteMax()
    {
        if (this.root == null)
        {
            throw new InvalidOperationException("The tree is empty");
        }
        this.root = this.DeleteMax(this.root);
    }

    private Node DeleteMax(Node node)
    {
        if (node == null)
        {
            return null;
        }
        if (node.Right == null)
        {
            return node.Left;
        }
        node.Right = this.DeleteMax(node.Right);
        node.Count = 1 + this.Count(node.Left) + this.Count(node.Right);
        return node;
    }
    public int Count()
    {
        return this.Count(this.root);
    }
    private int Count(Node node)
    {
        if (node == null)
        {
            return 0;
        }

        return node.Count;
    }
    public int Rank(T element)
    {
        return this.Rank(this.root, element);
    }
    private int Rank(Node node, T element)
    {
        if (node == null)
        {
            return 0;
        }
        int compare = element.CompareTo(node.Value);
        if (compare < 0)
        {
            return this.Rank(node.Left, element);
        }
        if (compare > 0)
        {
            return 1 + this.Count(node.Left) + this.Rank(node.Right, element);
        }
        return this.Count(node.Left);
    }
    public T Select(int rank)
    {
        Node node = this.Select(this.root, rank);
        if (node == null)
        {
            throw new InvalidOperationException();
        }
        return node.Value;
    }
    private Node Select(Node node, int rank)
    {
        if (node == null)
        {
            return null;
        }
        int leftCount = this.Count(node.Left);
        int compare = rank.CompareTo(leftCount);
        if (compare < 0)
        {
            return this.Select(node.Left, rank);
        }
        if (compare > 0)
        {
            return this.Select(node.Right, rank - leftCount - 1);
        }
        return node;
    }
    public T Ceiling(T element)
    {
        return this.Select(this.Rank(element) + 1);
    }

    public T Floor(T element)
    {
        return this.Select(this.Rank(element)-1);
    }
    public void EachPreOrder(Action<T> action)
    {
        this.EachPreOrder(this.root, action);
    }

    private void EachPreOrder(Node node, Action<T> action)
    {
        if (node == null)
        {
            return;
        }
        action(node.Value);
        this.EachPreOrder(node.Left, action);
        this.EachPreOrder(node.Right, action);
    }

    private class Node
    {
        public Node(T value)
        {
            this.Value = value;
        }

        public T Value { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public int Count { get; set; }
    }
}

public class Launcher
{
    static Dictionary<int, Tree<int>> nodeByValue = new Dictionary<int, Tree<int>>();
    static Tree<int> GetTreeNodeByValue(int value)
    {
        if (!nodeByValue.ContainsKey(value))
        {
            nodeByValue[value] = new Tree<int>(value);
        }
        return nodeByValue[value];
    }

    static void AddEdge(int parent, int child)
    {
        Tree<int> parentNode = GetTreeNodeByValue(parent);
        Tree<int> childNode = GetTreeNodeByValue(child);

        parentNode.Children.Add(childNode);
        childNode.Parent = parentNode;
    }

    static void ReadTree()
    {
        int nodeCount = int.Parse(Console.ReadLine());

        for (int i = 1; i < nodeCount; i++)
        {
            int[] edge = Console.ReadLine()
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();
            AddEdge(edge[0], edge[1]);
        }
    }

    static Tree<int> GetRoot()
    {
        return nodeByValue.Values.FirstOrDefault(n => n.Parent == null);
    }

    static void DFS(Tree<int> node, int targetSum, int sum = 0)
    {
        sum += node.Value;
        if (targetSum == sum)
        {
            PrintPath(node);
        }
        foreach (var child in node.Children)
        {
            DFS(child, targetSum, sum);
        }
    }
    private static int maxDepth = 0;
    static string DFSLongestPath(Tree<int> node, Stack<int> longestPath, int depth = 0)
    {
        if (node.Children.Count == 0 && depth > maxDepth)
        {
            maxDepth = depth;
            GetPath(node, longestPath);
        }
        
        foreach (var child in node.Children)
        {
            DFSLongestPath(child, longestPath, depth + 1);
        }
        return string.Join(" ", longestPath);
    }
    static void GetPath(Tree<int> node, Stack<int> longestPath)
    {
        longestPath.Clear();

        while (node != null)
        {
            longestPath.Push(node.Value);
            node = node.Parent;
        }
    }
    static void PrintPath(Tree<int> node)
    {
        Stack<int> path = new Stack<int>();
        Tree<int> start = node;
        path.Push(start.Value);
        while (start.Parent != null)
        {
            start = start.Parent;
            path.Push(start.Value);
        }

        Console.WriteLine(string.Join(" ", path));
    }
    public static void Main(string[] args)
    {
        BinarySearchTree<int> bst = new BinarySearchTree<int>();
        
        bst.Insert(10);
        bst.Insert(5);
        bst.Insert(3);
        bst.Insert(1);
        bst.Insert(4);
        bst.Insert(8);
        bst.Insert(9);
        bst.Insert(37);
        bst.Insert(39);
        bst.Insert(45);

        //Problem 1.	Delete Max
        //bst.EachInOrder(Console.WriteLine);
        //bst.DeleteMax();
        //bst.EachInOrder(Console.WriteLine);

        //Problem 2.	Count
        //int nodeCount = bst.Count();
        //Console.WriteLine(nodeCount);

        //Problem 3.	Rank
        //int rank = bst.Rank(8);
        //Console.WriteLine(rank);

        //Problem 4.	Select
        //int select = bst.Select(4);
        //Console.WriteLine(select);

        //Problem 5.	Floor
        //int floor = bst.Floor(5);
        //Console.WriteLine(floor);

        //Problem 6.	Ceiling
        //int ceiling = bst.Ceiling(4);
        //Console.WriteLine(ceiling);

        //Problem 7.	Delete*
        //bst.Delete(37);
        //bst.EachInOrder(Console.WriteLine);
        //bst.Delete(5);
        //bst.EachInOrder(Console.WriteLine);


        //ReadTree();
        //Tree<int> tree = GetRoot();
        //01. Root Node
        //Console.WriteLine($"Root node: {tree.Value}");
        //02. Print Tree
        //tree.PrintEachPreOrder();
        //03. Leaf Nodes
        //List<int> orderedLeafNodes = tree.GetLeafNodes().OrderBy(a => a).ToList();
        //Console.WriteLine($"Leaf nodes: {string.Join(" ", orderedLeafNodes)}");
        //04. Middle Nodes 
        //List<int> orderedMiddleNodes = tree.GetMiddleNodes().OrderBy(a => a).ToList();
        //Console.WriteLine($"Middle nodes: {string.Join(" ", orderedMiddleNodes)}");
        //06. Longest Path 
        //string longestPath = DFSLongestPath(tree, new Stack<int>());
        //Console.WriteLine($"Longest path: {longestPath}");
        //07. Paths With Given Sum
        //List<Tree<int>> leaf = nodeByValue.Values
        //    .Where(l => l.Children.Count == 0)
        //    .ToList();
        //int targetSum = int.Parse(Console.ReadLine());
        //Console.WriteLine($"Paths of sum {targetSum}:");
        //DFS(tree, targetSum);
    }
}