using System;

public class Invader : IInvader
{
    public Invader(int damage, int distance)
    {
        this.Damage = damage;
        this.Distance = distance;
    }
    
    public int Damage { get; set; }
    public int Distance { get; set; }

    public int CompareTo(IInvader other)
    {
        int result = this.Damage.CompareTo(other.Damage);
        if (result == 0)
        {
            result = this.Distance.CompareTo(other.Distance);
        }
        return result;
    }
}
