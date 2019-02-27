public class Competitor
{
    public Competitor(int id, string name)
    {
        this.Id = id;
        this.Name = name;
        this.TotalScore = 0;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public long TotalScore { get; set; }

    public int CompareTo(Competitor other)
    {
        return this.Id.CompareTo(other.Id);
    }

    public override bool Equals(object other)
    {
        Competitor competitor = (Competitor)other;
        return this.Equals(competitor);
    }
}
