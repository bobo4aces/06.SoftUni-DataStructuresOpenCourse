using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wintellect.PowerCollections;

public class ShoppingCenter : IShoppingCenter
{
    private int count; 

    private readonly Dictionary<string, OrderedBag<Product>> byName;
    private readonly Dictionary<string, OrderedBag<Product>> byProducer;
    private readonly Dictionary<string, OrderedBag<Product>> byNameAndProducer; 
    private readonly OrderedDictionary<decimal, Bag<Product>> byPrice;

    public ShoppingCenter()
    {
        this.byName = new Dictionary<string, OrderedBag<Product>>();
        this.byProducer = new Dictionary<string, OrderedBag<Product>>();
        this.byNameAndProducer = new Dictionary<string, OrderedBag<Product>>();
        this.byPrice = new OrderedDictionary<decimal, Bag<Product>>(); 
    }

    public int Count => this.count;

    public string AddProduct(string name, decimal price, string producer)
    {
        var product = new Product(name, price, producer);
        var nameAndProducer = this.CombineNameAndProducer(name, producer);

        this.byProducer.AppendValueToKey(producer, product);
        this.byName.AppendValueToKey(name, product);
        this.byNameAndProducer.AppendValueToKey(nameAndProducer, product);
        this.byPrice.AppendValueToKey(price, product);

        this.count++;
        return "Product added"; 
    }

    public string DeleteProducts(string producer)
    {
        var productsToDelete = this.byProducer.GetValuesForKey(producer).ToList();

        return Delete(productsToDelete);
    }

    public string DeleteProducts(string name, string producer)
    {
        var nameAndProducer = this.CombineNameAndProducer(name, producer);
        var productsToDelete = this.byNameAndProducer.GetValuesForKey(nameAndProducer).ToList();

        return Delete(productsToDelete);
    }

    public string FindProductsByName(string name)
    {
        var productsToReturn = this.byName.GetValuesForKey(name);

        return Find(productsToReturn);
    }

    public string FindProductsByPriceRange(decimal fromPrice, decimal toPrice)
    {
        var filteredDictionaryWithProducts = this.byPrice.Range(fromPrice, true, toPrice, true); 
        OrderedBag<Product> orderedBag = new OrderedBag<Product>(); 
        
        foreach (var kvp in filteredDictionaryWithProducts)
        {
            foreach (var product in kvp.Value)
            {
                orderedBag.Add(product); 
            }
        }

        StringBuilder sb = new StringBuilder();
        foreach (var product in orderedBag)
        {
            sb.AppendLine(product.ToString());
        }

        return CheckIfAnyProductIsFound(sb);
    }

    public string FindProductsByProducer(string producer)
    {
        var productsToReturn = this.byProducer.GetValuesForKey(producer);

        return Find(productsToReturn);
    }

    private string Delete(IEnumerable<Product> productsToDelete)
    {
        var totalCount = this.Count;

        foreach (var product in productsToDelete)
        {
            var nameAndProducer = this.CombineNameAndProducer(product.Name, product.Producer);

            this.byProducer[product.Producer].Remove(product);
            this.byName[product.Name].Remove(product);
            this.byNameAndProducer[nameAndProducer].Remove(product);
            this.byPrice[product.Price].Remove(product);
            this.count--;
        }

        return CheckIfProductWasDeleted(totalCount);
    }

    private string CheckIfProductWasDeleted(int totalCount)
    {
        if (totalCount == this.Count)
        {
            return "No products found";
        }

        return $"{totalCount - this.Count} products deleted";
    }

    private static string Find(IEnumerable<Product> productsToReturn)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var product in productsToReturn)
        {
            sb.AppendLine(product.ToString());
        }

        return CheckIfAnyProductIsFound(sb);
    }

    private static string CheckIfAnyProductIsFound(StringBuilder sb)
    {
        if (sb.Length == 0)
        {
            return "No products found";
        }

        return sb.ToString().Trim();
    }

    private string CombineNameAndProducer(string name, string producer)
    {
        return name + "!" + producer;
    }
}