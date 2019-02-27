using System;
using System.Collections;
using System.Collections.Generic;
using Wintellect.PowerCollections;
using System.Linq;

public class Instock : IProductStock
{
    Dictionary<string, LinkedListNode<Product>> fastRetrievalByLabel = 
        new Dictionary<string, LinkedListNode<Product>>();
    List<Product> byInsertion = new List<Product>();
    Dictionary<int, LinkedList<Product>> byQuantity =
        new Dictionary<int, LinkedList<Product>>();
    OrderedBag<Product> byPrice =
        new OrderedBag<Product>((x,y) => y.Price.CompareTo(x.Price));
    SortedSet<Product> byLabel =
        new SortedSet<Product>();

    public int Count => this.byInsertion.Count;

    public void Add(Product product)
    {
        this.byInsertion.Add(product);
        this.byLabel.Add(product);
        this.byPrice.Add(product);
        LinkedListNode<Product> node = new LinkedListNode<Product>(product);
        if (!this.byQuantity.ContainsKey(product.Quantity))
        {
            this.byQuantity.Add(product.Quantity, new LinkedList<Product>());
        }
        this.byQuantity[product.Quantity].AddLast(node);
        this.fastRetrievalByLabel.Add(product.Label, node);
    }

    public void ChangeQuantity(string label, int quantity)
    {
        if (!this.fastRetrievalByLabel.ContainsKey(label))
        {
            throw new ArgumentException();
        }
        
        LinkedListNode<Product> node = this.fastRetrievalByLabel[label];
        int oldQuantity = node.Value.Quantity;
        this.byQuantity[oldQuantity].Remove(node);
        node.Value.Quantity = quantity;
        if (!this.byQuantity.ContainsKey(quantity))
        {
            this.byQuantity.Add(quantity, new LinkedList<Product>());
        }
        this.byQuantity[quantity].AddLast(node);
    }


    public bool Contains(Product product)
    {
        return this.fastRetrievalByLabel.ContainsKey(product.Label);
    }

    public Product Find(int index)
    {
        if(index < 0 || index >= this.byInsertion.Count)
        {
            throw new IndexOutOfRangeException();
        }
        return this.byInsertion[index];
    }

    public IEnumerable<Product> FindAllByPrice(double price)
    {
        Product p = new Product("", price, 0);
        IEnumerable<Product> enumeration = this.byPrice.Range(p, true, p, true);
        return enumeration;
    }

    public IEnumerable<Product> FindAllByQuantity(int quantity)
    {
        if (this.byQuantity.ContainsKey(quantity))
        {
            return this.byQuantity[quantity];
        }
        return new List<Product>();
    }

    public IEnumerable<Product> FindAllInRange(double lo, double hi)
    {
        Product start = new Product("", lo, 2);
        Product end = new Product("", hi, 2);
        IEnumerable<Product> enumeration = this.byPrice.Range(end, true, start, false);
        return enumeration;
    }

    public Product FindByLabel(string label)
    {
        if (!this.fastRetrievalByLabel.ContainsKey(label))
        {
            throw new ArgumentException();
        }
        return this.fastRetrievalByLabel[label].Value;
    }


    public IEnumerable<Product> FindFirstByAlphabeticalOrder(int count)
    {
        if(count < 0 || count > this.Count)
        {
            throw new ArgumentException();
        }
        
        IEnumerable<Product> result =  this.byLabel.Take(count);
        return result;
    }

    public IEnumerable<Product> FindFirstMostExpensiveProducts(int count)
    {
        if(count <= 0 || count > this.Count)
        {
            throw new ArgumentException();
        }
        IEnumerable<Product> products = this.byPrice.Take(count);
        return products;
    }

    public IEnumerator<Product> GetEnumerator()
    {
        return this.byInsertion.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
