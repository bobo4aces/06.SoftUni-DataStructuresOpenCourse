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
        int result = this.Id.CompareTo(other.Id);
        if (result == 0)
        {
            result = this.Name.CompareTo(other.Name);
            //if (result == 0)
            //{
            //    result = this.TotalScore.CompareTo(other.TotalScore);
            //}
        }
        return result;
    }
}
