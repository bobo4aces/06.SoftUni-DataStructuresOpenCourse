public interface IShoppingCenter
{
    string AddProduct(string name, decimal price, string producer); 
    string DeleteProducts(string producer); 
    string DeleteProducts(string name, string producer); 
    string FindProductsByName(string name); 
    string FindProductsByProducer(string producer); 
    string FindProductsByPriceRange(decimal fromPrice, decimal toPrice); 
}