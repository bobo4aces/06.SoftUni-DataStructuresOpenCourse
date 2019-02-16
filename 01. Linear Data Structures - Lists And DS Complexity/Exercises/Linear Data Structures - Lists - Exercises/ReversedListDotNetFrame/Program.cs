public class Program
{
    public static void Main()
    {
        ReversedList<int> vs = new ReversedList<int>();
        vs.Add(1);
        vs.Add(2);
        vs.Add(3);
        vs.Add(4);
        vs.Add(5);
        vs.Add(6);
        vs.RemoveAt(5);
        foreach (var item in vs)
        {
            System.Console.WriteLine(item);
        }
    }
}
