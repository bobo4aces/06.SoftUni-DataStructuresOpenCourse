using System;
public class Launcher
{
    public static void Main()
    {
        DateTime currentDate = DateTime.Now;
        DateTime dateTime = DateTime.Now.AddDays(33);
        TimeSpan a = currentDate.Subtract(dateTime);
        Console.WriteLine(a.Days);
    }
}
