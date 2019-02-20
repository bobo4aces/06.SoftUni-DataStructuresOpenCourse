using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ShoppingCenterSlow : IShoppingCenter
{
    private List<Product> products;

    public ShoppingCenterSlow()
    {
        this.products = new List<Product>(); 
    }

    public string AddProduct(string name, decimal price, string producer)
    {
        var product = new Product(name, price, producer);

        this.products.Add(product);

        return "Product added"; 
    }

    public string DeleteProducts(string producer)
    {
        var totalProducts = this.products.Count;
        this.products = this.products
            .Where(p => p.Producer != producer)
            .ToList();

        return GetResultFromDeletion(totalProducts);
    }
    
    public string DeleteProducts(string name, string producer)
    {
        var totalProducts = this.products.Count;
        this.products = this.products
            .Where(p => p.Producer != producer && p.Name != name)
            .ToList();

        return GetResultFromDeletion(totalProducts);
    }

    public string FindProductsByName(string name)
    {
        List<Product> result = this.products
            .Where(p => p.Name == name)
            .ToList();

        return GetResultFromSearching(result);
    }

    public string FindProductsByPriceRange(decimal fromPrice, decimal toPrice)
    {
        List<Product> result = this.products
            .Where(p => p.Price >= fromPrice && p.Price <= toPrice)
            .ToList();

        return GetResultFromSearching(result);
    }

    public string FindProductsByProducer(string producer)
    {
        List<Product> result = this.products
            .Where(p => p.Producer == producer)
            .ToList();

        return GetResultFromSearching(result);
    }

    private static string GetResultFromSearching(List<Product> result)
    {
        if (result.Count == 0)
        {
            return "No products found";
        }

        StringBuilder sb = new StringBuilder();
        foreach (var pro in result.OrderBy(p => p.Name).ThenBy(p => p.Producer).ThenBy(p => p.Producer))
        {
            sb.AppendLine($"{{{pro.Name};{pro.Producer};{pro.Price.ToString("F2")}}}");
        }

        return sb.ToString().Trim();
    }

    private string GetResultFromDeletion(int totalProducts)
    {
        if (this.products.Count == totalProducts)
        {
            return "No products found";
        }

        return $"{totalProducts - this.products.Count} products deleted";
    }
}