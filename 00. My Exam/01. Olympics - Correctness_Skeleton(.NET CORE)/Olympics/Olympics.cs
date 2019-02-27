using System;
using System.Collections.Generic;
using System.Linq;

public class Olympics : IOlympics
{
    private HashSet<Competitor> competitors;
    private HashSet<Competition> competitions;
    private Dictionary<int, Competitor> byCompetitorId;
    private Dictionary<int, Competition> byCompetitionId;
    //private Dictionary<string, HashSet<Competitor>> byCompetitorName;
    //private Dictionary<string, HashSet<Competition>> byCompetitionName;

    public Olympics()
    {
        this.competitors = new HashSet<Competitor>();
        this.competitions = new HashSet<Competition>();
        this.byCompetitorId = new Dictionary<int, Competitor>();
        this.byCompetitionId = new Dictionary<int, Competition>();
        //this.byCompetitorName = new Dictionary<string, HashSet<Competitor>>();
        //this.byCompetitionName = new Dictionary<string, HashSet<Competition>>();
    }
    public void AddCompetition(int id, string name, int participantsLimit)
    {
        if (this.byCompetitionId.ContainsKey(id))
        {
            throw new ArgumentException();
        }
        Competition competition = new Competition(name, id, participantsLimit);
        this.competitions.Add(competition);
        this.byCompetitionId.Add(id, competition);
        //if (!this.byCompetitionName.ContainsKey(name))
        //{
        //    this.byCompetitionName.Add(name, new HashSet<Competition>());
        //}
        //this.byCompetitionName[name].Add(competition);
    }

    public void AddCompetitor(int id, string name)
    {
        if (this.byCompetitorId.ContainsKey(id))
        {
            throw new ArgumentException();
        }
        Competitor competitor = new Competitor(id, name);
        this.competitors.Add(competitor);
        this.byCompetitorId.Add(id, competitor);
        //if (!this.byCompetitorName.ContainsKey(name))
        //{
        //    this.byCompetitorName.Add(name, new HashSet<Competitor>());
        //}
        //this.byCompetitorName[name].Add(competitor);
    }

    public void Compete(int competitorId, int competitionId)
    {
        if (this.byCompetitorId.ContainsKey(competitorId) && this.byCompetitionId.ContainsKey(competitionId))
        {
            Competitor competitor = this.byCompetitorId[competitorId];
            Competition competition = this.byCompetitionId[competitionId];
            competitor.TotalScore += competition.Score;
            competition.Competitors.Add(competitor);
        }
        else
        {
            throw new ArgumentException();
        }
    }

    public int CompetitionsCount()
    {
        return this.competitions.Count;
    }

    public int CompetitorsCount()
    {
        return this.competitors.Count;
    }

    public bool Contains(int competitionId, Competitor comp)
    {
        if (this.byCompetitionId.ContainsKey(competitionId))
        {
            //return this.byCompetitionId[competitionId].Competitors.Contains(comp);
            return this.byCompetitionId[competitionId].Competitors.Contains(comp);
            //foreach (var competitor in competitors)
            //{
            //    if (competitor.CompareTo(comp) == 0)
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }
        throw new ArgumentException();
    }

    public void Disqualify(int competitionId, int competitorId)
    {

        if (this.byCompetitorId.ContainsKey(competitorId) && this.byCompetitionId.ContainsKey(competitionId))
        {

            this.byCompetitorId[competitorId].TotalScore -= this.byCompetitionId[competitionId].Score;
            this.byCompetitionId[competitionId].Competitors.Remove(this.byCompetitorId[competitorId]);

        }
        else
        {
            throw new ArgumentException();
        }
    }

    public IEnumerable<Competitor> FindCompetitorsInRange(long min, long max)
    {
        foreach (var competitior in this.competitors.OrderBy(x => x.Id))
        {
            if (competitior.TotalScore > min && competitior.TotalScore <= max)
            {
                yield return competitior;
            }
        }
    }

    public IEnumerable<Competitor> GetByName(string name)
    {
        if (this.competitors.Where(x => x.Name == name).Count() == 0)
        {
            throw new ArgumentException();
        }
        return this.competitors.Where(x => x.Name == name).OrderBy(x => x.Id);
        //if (!this.byCompetitorName.ContainsKey(name))
        //{
        //    throw new ArgumentException();
        //}
        //return this.byCompetitorName[name].OrderBy(x => x.Id);
    }

    public Competition GetCompetition(int id)
    {
        if (!this.byCompetitionId.ContainsKey(id))
        {
            throw new ArgumentException();
        }
        return this.byCompetitionId[id];
    }

    public IEnumerable<Competitor> SearchWithNameLength(int min, int max)
    {
        List<Competitor> competitors = new List<Competitor>();
        foreach (var competitor in this.competitors)
        {
            if (competitor.Name.Length >= min && competitor.Name.Length <= max)
            {
                competitors.Add(competitor);
            }
        }
        return competitors.OrderBy(x => x.Id);
    }
}