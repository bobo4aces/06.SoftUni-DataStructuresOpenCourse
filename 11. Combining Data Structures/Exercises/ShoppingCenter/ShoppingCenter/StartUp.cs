using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wintellect.PowerCollections;

namespace ShoppingCenter
{
    public class Program
    {
        public static void Main()
        {
            int count = int.Parse(Console.ReadLine());
            OrderedBag<Product> products = new OrderedBag<Product>();
            while (count > 0)
            {
                string[] tokens = Console.ReadLine().Split(';').ToArray();
                string command = tokens[0].Substring(0,tokens[0].IndexOf(' '));
                tokens[0] = tokens[0].Substring(tokens[0].IndexOf(' ')+1);
                string name = string.Empty;
                double price = 0;
                string producer = string.Empty;
                switch (command)
                {
                    case "AddProduct":
                        name = tokens[0];
                        price = double.Parse(tokens[1]);
                        producer = tokens[2];
                        Product product = new Product(name, price, producer);
                        AddProduct(products, product);
                        Console.WriteLine("Product added");
                        break;
                    case "DeleteProducts":
                        producer = tokens[0];
                        int deletedProductsCount = 0;
                        if (tokens.Length > 1)
                        {
                            name = tokens[0];
                            producer = tokens[1];
                            deletedProductsCount = DeleteProduct(products, name, producer);
                        }
                        else
                        {
                            producer = tokens[0];
                            deletedProductsCount = DeleteProduct(products, producer);
                        }
                        if (deletedProductsCount > 0)
                        {
                            Console.WriteLine($"{deletedProductsCount} products deleted");
                        }
                        else
                        {
                            Console.WriteLine("No products found");
                        }
                        break;
                    case "FindProductsByName":
                        name = tokens[0];
                        IEnumerable<Product> currentProducts = FindProductsByName(products, name);
                        PrintProducts(currentProducts);
                        break;
                    case "FindProductsByProducer":
                        producer = tokens[0];
                        currentProducts = FindProductsByProducer(products, producer);
                        PrintProducts(currentProducts);
                        break;
                    case "FindProductsByPriceRange":
                        double[] prices = tokens.Select(double.Parse).ToArray();
                        double fromPrice = prices[0];
                        double toPrice = prices[1];
                        currentProducts = FindProductsByPriceRange(products, fromPrice, toPrice);
                        PrintProducts(currentProducts);
                        break;
                }
                count--;
            }


        }

        private static void PrintProducts(IEnumerable<Product> currentProducts)
        {
            if (currentProducts.Count() == 0)
            {
                Console.WriteLine("No products found");
            }
            else
            {
                foreach (var currentProduct in currentProducts)
                {
                    Console.WriteLine(
                        $"{{{currentProduct.Name};{currentProduct.Producer};{currentProduct.Price.ToString("0.00")}}}");
                }
            }
        }

        private static void AddProduct(OrderedBag<Product> products, Product product)
        {
            products.Add(product);
            //products.OrderBy(p => p.Name).ThenBy(p => p.Producer).ThenBy(p => p.Price);
        }

        private static int DeleteProduct(OrderedBag<Product> products, string producer)
        {
            int productsCount = products
                                .Where(p => p.Producer == producer)
                                .Count();
            products.RemoveAll(p => p.Producer == producer);
            return productsCount;
        }
        private static int DeleteProduct(OrderedBag<Product> products, string name, string producer)
        {
            int productsCount = products
                                .Where(p => p.Name == name && p.Producer == producer)
                                .Count();
            products.RemoveAll(p => p.Name == name && p.Producer == producer);
            return productsCount;
        }

        private static IEnumerable<Product> FindProductsByName(OrderedBag<Product> products, string name)
        {
            return products.FindAll(p => p.Name == name);
        }

        private static IEnumerable<Product> FindProductsByProducer(OrderedBag<Product> products, string producer)
        {
            return products.FindAll(p => p.Producer == producer);
        }

        private static IEnumerable<Product> FindProductsByPriceRange(OrderedBag<Product> products, double fromPrice, double toPrice)
        {
            return products.FindAll(p => p.Price >= fromPrice && p.Price <= toPrice);
        }
    }
}
