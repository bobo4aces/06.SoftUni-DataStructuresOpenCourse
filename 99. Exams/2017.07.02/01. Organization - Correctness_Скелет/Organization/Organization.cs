using System;
using System.Collections;
using System.Collections.Generic;

public class Organization : IOrganization
{
    private Dictionary<string, HashSet<Person>> byName;
    private List<Person> byInsertion;

    public Organization()
    {
        this.byName = new Dictionary<string, HashSet<Person>>();
        this.byInsertion = new List<Person>();
    }
    public IEnumerator<Person> GetEnumerator()
    {
        foreach (var person in this.byInsertion)
        {
            yield return person;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    public int Count
    {
        get
        {
            return this.byInsertion.Count;
        }
    }
    public bool Contains(Person person)
    {
        if (this.byName.ContainsKey(person.Name))
        {
            return this.byName[person.Name].Contains(person);
        }
        return false;
    }

    public bool ContainsByName(string name)
    {
        return this.byName.ContainsKey(name);
    }

    public void Add(Person person)
    {
        if (!this.byName.ContainsKey(person.Name))
        {
            this.byName[person.Name] = new HashSet<Person>();
        }
        this.byName[person.Name].Add(person);
        this.byInsertion.Add(person);
        //if (!this.byNameSize.ContainsKey(person.Name.Length))
        //{
        //    this.byNameSize.Add(person.Name.Length, new Bag<Person>());
        //}
        //this.byNameSize[person.Name.Length].Add(person);
    }

    public Person GetAtIndex(int index)
    {
        if (index < 0 || index >= this.byInsertion.Count)
        {
            throw new IndexOutOfRangeException();
        }
        return this.byInsertion[index];
    }

    public IEnumerable<Person> GetByName(string name)
    {
        if (!this.byName.ContainsKey(name))
        {
            return new List<Person>();
        }
        return this.byName[name];
    }

    public IEnumerable<Person> FirstByInsertOrder(int count = 1)
    {
        List<Person> result = new List<Person>();
        if (count < 0)
        {
            return result;
        }
        for (int i = 0; i < count && i < this.byInsertion.Count; i++)
        {
            result.Add(this.byInsertion[i]);
        }
        return result;
    }

    public IEnumerable<Person> SearchWithNameSize(int minLength, int maxLength)
    {
        List<Person> result = new List<Person>();
        foreach (var nameSize in this.byName)
        {
            if (nameSize.Key.Length >= minLength && nameSize.Key.Length <= maxLength)
            {
                result.AddRange(nameSize.Value);
            }
            
        }
        return result;
    }

    public IEnumerable<Person> GetWithNameSize(int length)
    {
        List<Person> result = new List<Person>();
        foreach (var nameSize in this.byName)
        {
            if (nameSize.Key.Length == length)
            {
                result.AddRange(nameSize.Value);
            }

        }
        if (result.Count == 0)
        {
            throw new ArgumentException();
        }
        return result;
    }

    public IEnumerable<Person> PeopleByInsertOrder()
    {
        return this.byInsertion;
    }
}