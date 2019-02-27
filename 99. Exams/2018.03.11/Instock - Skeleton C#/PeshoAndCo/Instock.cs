using System;
using System.Collections;
using System.Collections.Generic;
using Wintellect.PowerCollections;
using System.Linq;

public class Instock : IProductStock
{
    private List<Product> products;
    private SortedDictionary<string, Product> byLabel;
    private Dictionary<double, List<Product>> byPrice;
    private Dictionary<int, List<Product>> byQuantity;
    private OrderedDictionary<double, OrderedBag<Product>> byRangePrice;

    public Instock()
    {
        this.products = new List<Product>();
        this.byLabel = new SortedDictionary<string, Product>();
        this.byPrice = new Dictionary<double, List<Product>>();
        this.byQuantity = new Dictionary<int, List<Product>>();
        this.byRangePrice = new OrderedDictionary<double, OrderedBag<Product>>();
    }
    public int Count => products.Count;

    public void Add(Product product)
    {
        this.products.Add(product);
        if (!this.byLabel.ContainsKey(product.Label))
        {
            this.byLabel.Add(product.Label, product);
        }
        this.byLabel[product.Label] = product;
        if (!this.byPrice.ContainsKey(product.Price))
        {
            this.byPrice.Add(product.Price, new List<Product>());
        }
        this.byPrice[product.Price].Add(product);
        if (!this.byQuantity.ContainsKey(product.Quantity))
        {
            this.byQuantity.Add(product.Quantity, new List<Product>());
        }
        this.byQuantity[product.Quantity].Add(product);
        if (!this.byRangePrice.ContainsKey(product.Price))
        {
            this.byRangePrice.Add(product.Price, new OrderedBag<Product>());
        }
        this.byRangePrice[product.Price].Add(product);
    }

    public void ChangeQuantity(string product, int quantity)
    {
        if (!this.byLabel.ContainsKey(product))
        {
            throw new ArgumentException("This product is missing!");
        }
        Product currentProduct = this.byLabel[product];
        int oldQuantity = this.byLabel[product].Quantity;
        
        this.byPrice[currentProduct.Price].Where(p => p.CompareTo(currentProduct) == 0).FirstOrDefault().Quantity = quantity;

        this.byQuantity[oldQuantity].Remove(currentProduct);
        if (!this.byQuantity.ContainsKey(quantity))
        {
            this.byQuantity[quantity] = new List<Product>();
        }
        Product productWithNewQuantity = currentProduct;
        productWithNewQuantity.Quantity = quantity;
        this.byQuantity[quantity].Add(productWithNewQuantity);
        this.byRangePrice[currentProduct.Price].Where(p => p.CompareTo(currentProduct) == 0).FirstOrDefault().Quantity = quantity;
        this.byLabel[product].Quantity = quantity;
    }

    public bool Contains(Product product)
    {
        if (!this.byLabel.ContainsKey(product.Label))
        {
            return false;
        }
        return this.byLabel[product.Label].CompareTo(product) == 0;
    }

    public Product Find(int index)
    {
        if (index < 0 || index >= this.products.Count)
        {
            throw new IndexOutOfRangeException("There is no such index!");
        }
        return this.products[index];
    }

    public IEnumerable<Product> FindAllByPrice(double price)
    {
        if (!this.byPrice.ContainsKey(price))
        {
            return new List<Product>();
        }
        return this.byPrice[price];

        //if (!this.byRangePrice.ContainsKey(price))
        //{
        //    return new List<Product>();
        //}
        //return this.byRangePrice[price];
    }

    public IEnumerable<Product> FindAllByQuantity(int quantity)
    {
        if (!this.byQuantity.ContainsKey(quantity))
        {
            return new List<Product>();
        }
        return this.byQuantity[quantity];
    }

    public IEnumerable<Product> FindAllInRange(double lo, double hi)
    {
        OrderedDictionary<double, OrderedBag<Product>>.View collection = this.byRangePrice.Range(lo, false, hi, true);

        Stack<Product> result = new Stack<Product>();
        if (collection.Count == 0)
        {
            return result;
        }
        foreach (var price in collection)
        {
            foreach (var product in price.Value)
            {
                result.Push(product);
            }
        }
        return result;
    }

    public Product FindByLabel(string label)
    {
        if (!this.byLabel.ContainsKey(label))
        {
            throw new ArgumentException("Product is missing!");
        }
        return this.byLabel[label];
    }

    public IEnumerable<Product> FindFirstByAlphabeticalOrder(int count)
    {
        List<Product> products = new List<Product>();

        if (count < 0 || count > this.products.Count)
        {
            //return products;
            throw new ArgumentException();
        }

        int counter = 0;
        
        foreach (var item in this.byLabel)
        {
            
            if (counter == count)
            {
                break;
            }
            counter++;
            products.Add(item.Value);
        }
        return products;
    }

    public IEnumerable<Product> FindFirstMostExpensiveProducts(int count)
    {
        List<Product> products = new List<Product>();
        if (count < 0 || count > this.products.Count)
        {
            throw new ArgumentException("Products are less than count!");
        }
        int counter = 0;
        foreach (var item in this.byRangePrice.Reverse())
        {
            foreach (var product in item.Value)
            {
                if (counter == count)
                {
                    break;
                }
                counter++;
                products.Add(product);
            }
        }
        return products;
    }

    public IEnumerator<Product> GetEnumerator()
    {
        foreach (var product in this.products)
        {
            yield return product;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
