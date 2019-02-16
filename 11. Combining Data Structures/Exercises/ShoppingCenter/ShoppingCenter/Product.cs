using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wintellect.PowerCollections;

namespace ShoppingCenter
{
    public class Product : IComparable
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Producer { get; set; }

        public Product(string name, double price, string producer)
        {
            this.Name = name;
            this.Price = price;
            this.Producer = producer;
        }

        public int CompareTo(object other)
        {
            Product product = (Product)other;
            int result = this.Name.CompareTo(product.Name);
            if (result == 0)
            {
                result = this.Producer.CompareTo(product.Producer);
            }
            if (result == 0)
            {
                result = this.Price.CompareTo(product.Price);
            }
            return result;
        }
    }
}
