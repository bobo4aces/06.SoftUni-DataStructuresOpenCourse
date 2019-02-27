
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        long elapsed = 0;
        for (int i = 0; i < 50; i++)
        {
            //Arrange
            IProductStock stock = new Instock();
            Stopwatch sw = new Stopwatch();
            //Act
            sw.Start();
            for (int j = 0; j < 50000; j++)
            {
                stock.Add(new Product(j.ToString(), j, j));
            }
            sw.Stop();
            elapsed += sw.ElapsedMilliseconds;
        }
        System.Console.WriteLine("Average: " + (elapsed/50));
        //Assert
    }
}

