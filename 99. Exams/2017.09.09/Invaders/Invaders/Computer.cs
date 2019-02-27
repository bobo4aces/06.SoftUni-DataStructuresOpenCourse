using System;
using System.Collections.Generic;
using System.Linq;

public class Computer : IComputer
{
    private int energy;
    private HashSet<Invader> invaders;
    private Dictionary<int, Dictionary<int, Invader>> byPriority;
    public Computer(int energy)
    {
        if (energy < 0)
        {
            throw new ArgumentException();
        }
        this.energy = energy;
        this.invaders = new HashSet<Invader>();
        this.byPriority = new Dictionary<int, Dictionary<int, Invader>>();
    }

    public int Energy
    {
        get
        {
            if (this.energy < 0)
            {
                return 0;
            }
            return this.energy;
        }
    }

    public void Skip(int turns)
    {
        foreach (var invader in this.invaders)
        {
            invader.Distance -= turns;
            if (invader.Distance <= 0)
            {
                this.energy -= invader.Damage;
            }
        }
        this.invaders.RemoveWhere(x => x.Distance <= 0);
    }

    public void AddInvader(Invader invader)
    {
        this.invaders.Add(invader);
    }

    public void DestroyHighestPriorityTargets(int count)
    {
        IEnumerable<Invader> invadersToRemove = this.invaders.OrderBy(x => x.Distance).ThenBy(x => -x.Damage).Take(count);
        invaders.ExceptWith(invadersToRemove);
    }

    public void DestroyTargetsInRadius(int radius)
    {
        this.invaders.RemoveWhere(x => x.Distance <= radius);
    }

    public IEnumerable<Invader> Invaders()
    {
        foreach (var invader in this.invaders)
        {
            yield return invader;
        }
    }
}
