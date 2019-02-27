using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

public class RoyaleArena : IArena
{
    private HashSet<Battlecard> battlecards;
    private Dictionary<int, Battlecard> byId;
    private Dictionary<CardType, OrderedDictionary<double, HashSet<Battlecard>>> byCard;
    private Dictionary<string, OrderedDictionary<double, HashSet<Battlecard>>> byName;
    private OrderedDictionary<double, HashSet<Battlecard>> bySwag;

    public RoyaleArena()
    {
        this.battlecards = new HashSet<Battlecard>();
        this.byId = new Dictionary<int, Battlecard>();
        this.byCard =
            new Dictionary<CardType, OrderedDictionary<double, HashSet<Battlecard>>>();
        this.byCard.Add(CardType.BUILDING, new OrderedDictionary<double, HashSet<Battlecard>>((x, y) => y.CompareTo(x)));
        this.byCard.Add(CardType.MELEE, new OrderedDictionary<double, HashSet<Battlecard>>((x, y) => y.CompareTo(x)));
        this.byCard.Add(CardType.RANGED, new OrderedDictionary<double, HashSet<Battlecard>>((x, y) => y.CompareTo(x)));
        this.byCard.Add(CardType.SPELL, new OrderedDictionary<double, HashSet<Battlecard>>((x, y) => y.CompareTo(x)));
        this.byName = new Dictionary<string, OrderedDictionary<double, HashSet<Battlecard>>>();
        this.bySwag = new OrderedDictionary<double, HashSet<Battlecard>>((x, y) => x.CompareTo(y));

    }
    public int Count => this.battlecards.Count;

    public void Add(Battlecard card)
    {
        this.battlecards.Add(card);
        if (!this.byId.ContainsKey(card.Id))
        {
            this.byId.Add(card.Id, null);
        }
        this.byId[card.Id] = card;
        if (!this.byCard[card.Type].ContainsKey(card.Damage))
        {
            this.byCard[card.Type].Add(card.Damage, new HashSet<Battlecard>());
        }
        this.byCard[card.Type][card.Damage].Add(card);
        if (!this.byName.ContainsKey(card.Name))
        {
            this.byName.Add(card.Name, new OrderedDictionary<double, HashSet<Battlecard>>((x, y) => y.CompareTo(x)));
        }
        if (!this.byName[card.Name].ContainsKey(card.Swag))
        {
            this.byName[card.Name].Add(card.Swag, new HashSet<Battlecard>());
        }
        this.byName[card.Name][card.Swag].Add(card);
        if (!this.bySwag.ContainsKey(card.Swag))
        {
            this.bySwag.Add(card.Swag, new HashSet<Battlecard>());
        }
        this.bySwag[card.Swag].Add(card);
    }

    public void ChangeCardType(int id, CardType type)
    {
        if (!this.byId.ContainsKey(id))
        {
            throw new ArgumentException();
        }
        Battlecard card = this.byId[id];
        this.byCard[card.Type][card.Damage].Remove(card);
        this.byId[id].Type = type;
        if (!this.byCard[type].ContainsKey(card.Damage))
        {
            this.byCard[type].Add(card.Damage, new HashSet<Battlecard>());
        }
        this.byCard[type][card.Damage].Add(this.byId[id]);
    }

    public bool Contains(Battlecard card)
    {
        return this.battlecards.Contains(card);
    }

    public IEnumerable<Battlecard> FindFirstLeastSwag(int n)
    {
        if (this.battlecards.Count < n)
        {
            throw new InvalidOperationException();
        }
        List<Battlecard> result = new List<Battlecard>();
        foreach (var swag in this.bySwag)
        {
            foreach (var card in swag.Value.OrderBy(x=>x.Id))
            {
                if (n == 0)
                {
                    return result;
                }
                result.Add(card);
                n--;
            }
        }
        return result;
    }

    public IEnumerable<Battlecard> GetAllByNameAndSwag()
    {
        List<Battlecard> result = new List<Battlecard>();
        foreach (var name in this.byName)
        {
            foreach (var item in name.Value.Where(x=>x.Value.Count > 0))
            {
                result.AddRange(item.Value);
                break;
            }
        }
        //if (result.Count == 0)
        //{
        //    throw new InvalidOperationException();
        //}
        return result;
    }

    public IEnumerable<Battlecard> GetAllInSwagRange(double lo, double hi)
    {
        List<Battlecard> result = new List<Battlecard>();
        if (this.bySwag.Count == 0)
        {
            return result;
        }
        foreach (var card in this.bySwag.Range(lo,true,hi,true))
        {
            result.AddRange(card.Value);
        }
        return result;
    }

    public IEnumerable<Battlecard> GetByCardType(CardType type)
    {
        if (this.byCard[type].Count == 0)
        {
            throw new InvalidOperationException();
        }
        List<Battlecard> result = new List<Battlecard>();
        foreach (var damage in this.byCard[type])
        {
            result.AddRange(damage.Value.OrderBy(x => x.Id));
        }
        return result;
    }

    public IEnumerable<Battlecard> GetByCardTypeAndMaximumDamage(CardType type, double damage)
    {
        if (this.byCard[type].Count == 0)
        {
            throw new InvalidOperationException();
        }
        List<Battlecard> result = new List<Battlecard>();
        var a = this.byCard[type].RangeTo(damage, true);
        foreach (var card in this.byCard[type])
        {
            if (card.Key <= damage)
            {
                result.AddRange(card.Value.OrderBy(x => x.Id));
            }
            //foreach (var item in card.Value.OrderBy(x => x.Id))
            //{
            //    result.Add(item);
            //}
        }
        return result;
    }

    public Battlecard GetById(int id)
    {
        if (!this.byId.ContainsKey(id))
        {
            throw new InvalidOperationException();
        }
        return this.byId[id];
    }

    public IEnumerable<Battlecard> GetByNameAndSwagRange(string name, double lo, double hi)
    {
        if (!this.byName.ContainsKey(name) || this.byName[name].Range(lo, true, hi, false).Count == 0)
        {
            throw new InvalidOperationException();
        }
        List<Battlecard> result = new List<Battlecard>();
        foreach (var card in this.byName[name].Range(lo, true, hi, false))
        {
            result.AddRange(card.Value.OrderBy(x => x.Id));
        }
        return result;
    }

    public IEnumerable<Battlecard> GetByNameOrderedBySwagDescending(string name)
    {
        if (!this.byName.ContainsKey(name))
        {
            throw new InvalidOperationException();
        }
        List<Battlecard> result = new List<Battlecard>();
        foreach (var card in this.byName[name])
        {
            result.AddRange(card.Value.OrderBy(x => x.Id));
        }
        return result;
    }

    public IEnumerable<Battlecard> GetByTypeAndDamageRangeOrderedByDamageThenById(CardType type, int lo, int hi)
    {
        if (this.byCard[type].Count == 0)
        {
            throw new InvalidOperationException();
        }
        List<Battlecard> result = new List<Battlecard>();
        foreach (var card in this.byCard[type])
        {
            
            if (card.Key > lo && card.Key < hi)
            {
                result.AddRange(card.Value.OrderBy(x=>x.Id));
            }
            
        }
        if (result.Count == 0)
        {
            throw new InvalidOperationException();
        }
        return result;
    }

    public IEnumerator<Battlecard> GetEnumerator()
    {
        foreach (var card in this.battlecards)
        {
            yield return card;
        }
    }

    public void RemoveById(int id)
    {
        if (!this.byId.ContainsKey(id))
        {
            throw new InvalidOperationException();
        }
        Battlecard battlecard = this.byId[id];
        this.byCard[battlecard.Type][battlecard.Damage].Remove(battlecard);
        this.byName[battlecard.Name][battlecard.Swag].Remove(battlecard);
        this.bySwag[battlecard.Swag].Remove(battlecard);
        this.battlecards.Remove(battlecard);
        this.byId.Remove(battlecard.Id);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
