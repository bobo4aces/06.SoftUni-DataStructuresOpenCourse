using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

/// <summary>
/// The ThreadExecutor is the concrete implementation of the IScheduler.
/// You can send any class to the judge system as long as it implements
/// the IScheduler interface. The Tests do not contain any <e>Reflection</e>!
/// </summary>
public class ThreadExecutor : IScheduler
{

    private int cycles;

    // Slow Implementation
    private List<Task> tasks;
    //private Dictionary<int, Task> byIdSlow;

    /// <summary>
    /// Fast Implementation
    /// </summary>
    private LinkedList<Task> byInsertion;
    private Dictionary<int, Task> byId;
    private Dictionary<Priority, OrderedBag<Task>> byPriority;
    private OrderedBag<LinkedListNode<Task>> byConsumption;

    public ThreadExecutor()
    {

        this.byInsertion = new LinkedList<Task>();
        this.byConsumption =
            new OrderedBag<LinkedListNode<Task>>(
                (x, y) => x.Value.CompareTo(y.Value)
            );
        this.byId = new Dictionary<int, Task>();

        this.byPriority = new Dictionary<Priority, OrderedBag<Task>>();

        this.byPriority.Add(Priority.LOW, new OrderedBag<Task>((x, y) => y.Id.CompareTo(x.Id)));
        this.byPriority.Add(Priority.MEDIUM, new OrderedBag<Task>((x, y) => y.Id.CompareTo(x.Id)));
        this.byPriority.Add(Priority.HIGH, new OrderedBag<Task>((x, y) => y.Id.CompareTo(x.Id)));
        this.byPriority.Add(Priority.EXTREME, new OrderedBag<Task>((x, y) => y.Id.CompareTo(x.Id)));
        this.tasks = new List<Task>();
    }

    public int Count
    {
        get => this.byInsertion.Count;
    }

    public int ThreadsCount => throw new NotImplementedException();

    public void ChangePriority(int id, Priority newPriority)
    {

        if (!this.byId.ContainsKey(id))
        {
            throw new ArgumentException();
        }

        Task byId = this.byId[id];

        if (newPriority == byId.TaskPriority)
        {
            return;
        }

        this.byPriority[byId.TaskPriority].Remove(byId);
        byId.TaskPriority = newPriority;
        this.byPriority[newPriority].Add(byId);
    }

    public bool Contains(Task task)
    {
        return this.byId.ContainsKey(task.Id);
    }

    public int Cycle(int cycles)
    {

        if (this.byInsertion.Count == 0)
        {
            throw new InvalidOperationException();
        }

        LinkedListNode<Task> low = new LinkedListNode<Task>(new Task(5, this.cycles + 1, Priority.EXTREME));
        LinkedListNode<Task> high = new LinkedListNode<Task>(new Task(7, this.cycles + cycles, Priority.LOW));

        this.cycles += cycles;

        IEnumerable<LinkedListNode<Task>> result = this.byConsumption.Range(low, true, high, true);

        int count = 0;
        foreach (var t in result)
        {
            count++;
            this.byInsertion.Remove(t);
            this.byPriority[t.Value.TaskPriority].Remove(t.Value);
            this.byId.Remove(t.Value.Id);
        }
        return count;

        //int result = this.tasks.RemoveAll(x => {
        //    if (x.Consumption <= this.cycles)
        //    {
        //        this.byIdSlow.Remove(x.Id);
        //        this.byId[x.TaskPriority].Remove(x.Id);
        //        return true;
        //    }
        //    return false;
        //});
        //
        //return result;
    }

    public void Execute(Task task)
    {
        if (this.byId.ContainsKey(task.Id))
        {
            throw new ArgumentException();
        }
        this.tasks.Add(task);
        LinkedListNode<Task> node = new LinkedListNode<Task>(task);

        this.byInsertion.AddLast(node);
        this.byConsumption.Add(node);
        this.byId.Add(task.Id, task);
        this.byPriority[task.TaskPriority].Add(task);
    }

    public IEnumerable<Task> GetByConsumptionRange(int lo, int hi, bool inclusive)
    {
        LinkedListNode<Task> low = new LinkedListNode<Task>(new Task(5, lo + this.cycles, inclusive ? Priority.EXTREME : Priority.LOW));
        LinkedListNode<Task> high = new LinkedListNode<Task>(new Task(7, hi + this.cycles, inclusive ? Priority.LOW : Priority.EXTREME));
        return this.byConsumption.Range(low, inclusive, high, inclusive).Select(x => x.Value);

        // This here is the slow implementation
        //IEnumerable<Task> result = this.tasks.Where(x =>
        //{
        //    bool inBoundsExclusive = x.Consumption > lo && x.Consumption < hi;
        //
        //    return inclusive ?
        //        (x.Consumption == lo || x.Consumption == hi) || inBoundsExclusive
        //        : inBoundsExclusive;
        //}).OrderBy(x => x.Consumption).ThenByDescending(x => x.TaskPriority);
        //
        //if (result.Any())
        //{
        //    return result;
        //}
        //
        //return new List<Task>();
    }

    public Task GetById(int id)
    {
        if (!this.byId.ContainsKey(id))
        {
            throw new ArgumentException();
        }

        return this.byId[id];
    }

    public Task GetByIndex(int index)
    {
        if (index < 0 || index >= this.Count)
        {
            throw new ArgumentOutOfRangeException();
        }

        return this.byInsertion.ElementAt(index);
    }

    public IEnumerable<Task> GetByPriority(Priority type)
    {
        //return this.byPriority[type];

        //if (!result.Any())
        //{
        //    return new List<Task>();
        //}
        //
        //return result;

        return this.byInsertion.Where(x => x.TaskPriority == type).OrderByDescending(x => x.Id);
    }

    public IEnumerable<Task> GetByPriorityAndMinimumConsumption(Priority priority, int lo)
    {
        return this.byInsertion.Where(x => x.TaskPriority == priority && x.Consumption >= lo).OrderByDescending(x => x.Id);
    }

    /// <summary>
    /// The ThreadExecutor should be enumerated by insertion
    /// </summary>
    /// <returns></returns>
    public IEnumerator<Task> GetEnumerator()
    {
        return this.byInsertion.GetEnumerator();

        //return this.tasks.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
