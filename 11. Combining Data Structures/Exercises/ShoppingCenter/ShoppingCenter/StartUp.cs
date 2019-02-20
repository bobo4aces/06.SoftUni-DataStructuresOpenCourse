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

            ShoppingCenter shoppingCenter = new ShoppingCenter();

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
                        shoppingCenter.AddProduct(name, price, producer);
                        Console.WriteLine("Product added");
                        break;
                    case "DeleteProducts":
                        producer = tokens[0];
                        int deletedProductsCount = 0;
                        if (tokens.Length > 1)
                        {
                            name = tokens[0];
                            producer = tokens[1];
                            deletedProductsCount = shoppingCenter.DeleteByNameAndProducer(name, producer);
                        }
                        else
                        {
                            producer = tokens[0];
                            deletedProductsCount = shoppingCenter.DeleteByProducer(producer);
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
                        shoppingCenter.PrintProducts(shoppingCenter?.FindByName(name));
                        break;
                    case "FindProductsByProducer":
                        producer = tokens[0];
                        shoppingCenter.PrintProducts(shoppingCenter?.FindByProducer(producer));
                        break;
                    case "FindProductsByPriceRange":
                        double[] prices = tokens.Select(double.Parse).ToArray();
                        double fromPrice = prices[0];
                        double toPrice = prices[1];
                        shoppingCenter.PrintProducts(shoppingCenter?.FindByPrice(fromPrice, toPrice));
                        break;
                }
                count--;
            }


        }
    }
}
