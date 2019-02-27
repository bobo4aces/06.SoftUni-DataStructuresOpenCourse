using System;
using System.Collections;
using System.Collections.Generic;

public class Organization : IOrganization
{
    private Dictionary<string, HashSet<Person>> personsByName;

    private List<Person> indexedPersons;

    public Organization()
    {
        this.personsByName = new Dictionary<string, HashSet<Person>>();
        this.indexedPersons = new List<Person>();
        this.Count = 0;
    }

    public int Count { get; private set; }
    public bool Contains(Person person)
    {
        if (this.personsByName.ContainsKey(person.Name))
        {
            return this.personsByName[person.Name].Contains(person);
        }

        return false;
    }

    public bool ContainsByName(string name)
    {
        return this.personsByName.ContainsKey(name);
    }

    public void Add(Person person)
    {
        if (!this.personsByName.ContainsKey(person.Name))
        {
            this.personsByName[person.Name] = new HashSet<Person>();
        }

        this.personsByName[person.Name].Add(person);

        this.indexedPersons.Add(person);
        this.Count++;
    }

    public Person GetAtIndex(int index)
    {
        if (index < 0 || index >= this.indexedPersons.Count)
        {
            throw new IndexOutOfRangeException();
        }

        return this.indexedPersons[index];
    }

    public IEnumerable<Person> GetByName(string name)
    {
        if (!this.personsByName.ContainsKey(name))
        {
            return new List<Person>();
        }

        return this.personsByName[name];
    }

    public IEnumerable<Person> FirstByInsertOrder(int count = 1)
    {
        List<Person> result = new List<Person>();

        if (this.indexedPersons.Count == 0)
        {
            return result;
        }

        for (int i = 0; i < count && i < this.indexedPersons.Count; i++)
        {
            result.Add(this.indexedPersons[i]);
        }

        return result;
    }

    public IEnumerable<Person> SearchWithNameSize(int minLength, int maxLength)
    {
        List<Person> resultPersons = new List<Person>();

        //TODO: PERFORMANCE TEST
        foreach (var entry in this.personsByName)
        {
            if (entry.Key.Length >= minLength && entry.Key.Length <= maxLength)
            {
                resultPersons.AddRange(entry.Value);
            }
        }

        return resultPersons;
    }

    public IEnumerable<Person> GetWithNameSize(int length)
    {
        List<Person> resultPersons = new List<Person>();

        //TODO: PERFORMANCE TEST
        foreach (var entry in this.personsByName)
        {
            if (entry.Key.Length == length)
            {
                resultPersons.AddRange(entry.Value);
            }
        }

        if (resultPersons.Count == 0)
        {
            throw new ArgumentException();
        }

        return resultPersons;
    }

    public IEnumerable<Person> PeopleByInsertOrder()
    {
        foreach (var entry in this)
        {
            yield return entry;
        }
    }

    public IEnumerator<Person> GetEnumerator()
    {
        foreach (var entry in this.indexedPersons)
        {
            yield return entry;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}