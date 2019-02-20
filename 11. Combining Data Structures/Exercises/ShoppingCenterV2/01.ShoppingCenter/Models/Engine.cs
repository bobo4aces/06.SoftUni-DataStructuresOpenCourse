public class Engine : IEngine
{
    private readonly IReader reader;
    private readonly IWriter writer;

    private readonly IShoppingCenter shoppingCenter;

    public Engine(IShoppingCenter shoppingCenter, IReader reader, IWriter writer)
    {
        this.shoppingCenter = shoppingCenter;
        this.reader = reader;
        this.writer = writer;
    }

    public void Run()
    {
        var inputRowsCount = int.Parse(this.reader.ReadLine());

        for (int i = 0; i < inputRowsCount; i++)
        {
            var command = this.reader.ReadLine();
            var output = this.ProcessCommand(command);
            this.writer.WriteLine(output);
        }
    }

    public string ProcessCommand(string input)
    {
        var tokens = input.Split(';');
        var command = tokens[0];
        var result = string.Empty;

        if (command.StartsWith("AddProduct"))
        {
            var name = tokens[0].Replace("AddProduct", "").Trim();
            var price = decimal.Parse(tokens[1]);
            var producer = tokens[2];

            result = this.shoppingCenter.AddProduct(name, price, producer);
        }
        else if (command.StartsWith("DeleteProducts"))
        {
            if (tokens.Length == 1)
            {
                var producer = tokens[0].Replace("DeleteProducts", "").Trim();
                result = this.shoppingCenter.DeleteProducts(producer);
            }
            else
            {
                var name = tokens[0].Replace("DeleteProducts", "").Trim();
                var producer = tokens[1];
                result = this.shoppingCenter.DeleteProducts(name, producer);
            }
        }
        else if (command.StartsWith("FindProductsByName"))
        {
            var name = tokens[0].Replace("FindProductsByName", "").Trim();
            result = this.shoppingCenter.FindProductsByName(name);
        }
        else if (command.StartsWith("FindProductsByProducer"))
        {
            var producer = tokens[0].Replace("FindProductsByProducer", "").Trim();
            result = this.shoppingCenter.FindProductsByProducer(producer);
        }
        else if (command.StartsWith("FindProductsByPriceRange"))
        {
            var fromPrice = decimal.Parse(tokens[0].Replace("FindProductsByPriceRange", "").Trim());
            var toPrice = decimal.Parse(tokens[1]);
            result = this.shoppingCenter.FindProductsByPriceRange(fromPrice, toPrice);
        }

        return result; 
    }
}