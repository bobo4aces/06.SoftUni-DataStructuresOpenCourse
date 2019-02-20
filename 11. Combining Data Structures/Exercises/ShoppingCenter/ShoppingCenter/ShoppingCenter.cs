using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wintellect.PowerCollections;

namespace ShoppingCenter
{
    public class ShoppingCenter
    {
        private IDictionary<string, OrderedBag<Product>> byProducer;
        private IDictionary<string, OrderedBag<Product>> byNameAndProducer;
        private IDictionary<string, OrderedBag<Product>> byName;
        private OrderedDictionary<double, Bag<Product>> byPrice;

        public ShoppingCenter()
        {
            this.byProducer = new Dictionary<string, OrderedBag<Product>>();
            this.byNameAndProducer = new Dictionary<string, OrderedBag<Product>>();
            this.byName = new Dictionary<string, OrderedBag<Product>>();
            this.byPrice = new OrderedDictionary<double, Bag<Product>>();
        }

        public void AddProduct(string name, double price, string producer)
        {
            Product product = new Product(name, price, producer);
            OrderedBag<Product> products = new OrderedBag<Product>();
            AddToDictionary(this.byProducer, producer, product);
            AddToDictionary(this.byNameAndProducer, string.Concat(name, "!", producer), product);
            AddToDictionary(this.byName, name, product);
            if (!this.byPrice.ContainsKey(price))
            {
                this.byPrice.Add(price, new Bag<Product>());
            }
            this.byPrice[price].Add(product);
        }
        
        private void AddToDictionary(IDictionary<string, OrderedBag<Product>> dictionary,string key, Product value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, new OrderedBag<Product>());
            }
            dictionary[key].Add(value);
        }

        public int DeleteByProducer(string producer)
        {
            int count = 0;
            if (!this.byProducer.ContainsKey(producer))
            {
                return count;
            }
            foreach (var product in this.byProducer[producer])
            {
                this.byNameAndProducer[string.Concat(product.Name, "!", product.Producer)].Remove(product);
                this.byName[product.Name].Remove(product);
                this.byPrice[product.Price].Remove(product);
                count++;
            }
            
            this.byProducer.Remove(producer);

            return count;
        }

        public int DeleteByNameAndProducer(string name, string producer)
        {
            int count = 0;
            string nameAndProducer = string.Concat(name, "!", producer);
            if (!this.byNameAndProducer.ContainsKey(nameAndProducer))
            {
                return count;
            }
            foreach (var product in this.byNameAndProducer[nameAndProducer])
            {
                this.byProducer[product.Producer].Remove(product);
                this.byName[product.Name].Remove(product);
                this.byPrice[product.Price].Remove(product);
                count++;
            }
            this.byNameAndProducer.Remove(nameAndProducer);

            return count;
        }

        public IEnumerable<Product> FindByName(string name)
        {
            if (!this.byName.ContainsKey(name)|| this.byName[name].Count == 0)
            {
                return null;
            }
            return this.byName[name];
        }
        public IEnumerable<Product> FindByProducer(string producer)
        {
            if (!this.byProducer.ContainsKey(producer) || this.byProducer[producer].Count == 0)
            {
                return null;
            }
            return this.byProducer[producer];
        }

        public IEnumerable<Product> FindByPrice(double fromPrice, double toPrice)
        {
            if (this.byPrice.Range(fromPrice, true, toPrice, true) != null)
            {
                foreach (var bag in this.byPrice.Range(fromPrice, true, toPrice, true).Values)
                {
                    foreach (var product in bag)
                    {
                        yield return product;
                    }
                }
            }
        }

        public void PrintProducts(IEnumerable<Product> products)
        {
            if (products == null)
            {
                Console.WriteLine("No products found");
                return;
            }
            foreach (var product in products)
            {
                Console.WriteLine(product.ToString());
            }
        }
    }
}
