using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wintellect.PowerCollections;

public class Board : IBoard
{
    Comparison<Card> nameComparer =
        (first, other) =>
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var c in first.Name.Reverse())
            {
                stringBuilder.Append(c);
            }
            string name = stringBuilder.ToString();
            stringBuilder.Clear();
            foreach (var c in other.Name.Reverse())
            {
                stringBuilder.Append(c);
            }
            string nameOther = stringBuilder.ToString();
            int result = name.CompareTo(nameOther);
            if (result == 0)
            {
                result = first.Level.CompareTo(other.Level);
            }
            return result;
        };

    private HashSet<Card> cards;
    private Dictionary<string, Card> byName;
    public Board()
    {
        this.cards = new HashSet<Card>();
        this.byName = new Dictionary<string, Card>();
    }
    public bool Contains(string name)
    {
        return this.byName.ContainsKey(name);
    }

    public int Count()
    {
        return this.cards.Count;
    }

    public void Draw(Card card)
    {
        if (this.byName.ContainsKey(card.Name))
        {
            throw new ArgumentException();
        }
        this.cards.Add(card);
        this.byName[card.Name] = card;
    }

    public IEnumerable<Card> GetBestInRange(int start, int end)
    {
        List<Card> cards = new List<Card>();
        foreach (var card in this.cards.OrderByDescending(x => x.Level))
        {
            if (card.Score >= start && card.Score <= end)
            {
                cards.Add(card);
            }
        }
        return cards;
    }

    public void Heal(int health)
    {
        foreach (var card in this.cards.OrderBy(x => x.Health))
        {
            card.Health += health;
            break;
        }
    }

    public IEnumerable<Card> ListCardsByPrefix(string prefix)
    {
        //List<Card> cards = this.cards.Where(x => x.Name.StartsWith(prefix)).OrderBy(.ToList();
        OrderedBag<Card> cards = new OrderedBag<Card>(nameComparer);
        foreach (var item in this.byName.Where(x => x.Key[prefix.Length -1].Equals(prefix)))
        {
            cards.Add(item.Value);
        }
        //return this.cards.Where(x => x.Name.StartsWith(prefix)).OrderByDescending().ThenBy(x => x.Level);
        return cards;
    }

    public int CompareNameTo(Card first, Card other)
    {
        var name = first.Name.Reverse();
        var nameOther = first.Name.Reverse();
        int index = 0;
        int result = 0;
        while (result != 0 || index >= first.Name.Length || index >= other.Name.Length)
        {
            result = Convert.ToInt32(first.Name[index]).CompareTo(Convert.ToInt32(other.Name[index]));
            index++;
        }
        return result;
    }

    public void Play(string attackerCardName, string attackedCardName)
    {
        if (!this.byName.ContainsKey(attackerCardName) || !this.byName.ContainsKey(attackedCardName))
        {
            throw new ArgumentException();
        }
        Card attacker = this.byName[attackerCardName];
        Card attacked = this.byName[attackedCardName];
        if (attacker.Level != attacked.Level)
        {
            throw new ArgumentException();
        }
        if (attacked.Health <= 0)
        {
            return;
        }
        attacked.Health -= attacker.Damage;
        if (attacked.Health <= 0)
        {
            attacker.Score += attacked.Level;
        }
    }

    public void Remove(string name)
    {
        if (!this.byName.ContainsKey(name))
        {
            throw new ArgumentException();
        }
        this.cards.Remove(this.byName[name]);
        this.byName.Remove(name);
    }

    public void RemoveDeath()
    {
        this.cards.RemoveWhere(x => x.Health <= 0);
    }

    public IEnumerable<Card> SearchByLevel(int level)
    {
        return this.cards.Where(x => x.Level == level).OrderByDescending(x => x.Score);
    }
}