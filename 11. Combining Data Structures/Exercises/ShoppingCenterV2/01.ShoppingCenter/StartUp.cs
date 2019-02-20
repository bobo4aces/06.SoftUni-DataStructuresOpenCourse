public class StartUp
{
    public static void Main()
    {
        IShoppingCenter shoppingCenter = new ShoppingCenter();
        //IShoppingCenter shoppingCenter = new ShoppingCenterSlow();

        IReader reader = new ConsoleReader();
        IWriter writer = new ConsoleWriter(); 

        IEngine engine = new Engine(shoppingCenter, reader, writer);
        engine.Run(); 
    }
}