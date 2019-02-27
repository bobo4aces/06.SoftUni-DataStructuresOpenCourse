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
}
