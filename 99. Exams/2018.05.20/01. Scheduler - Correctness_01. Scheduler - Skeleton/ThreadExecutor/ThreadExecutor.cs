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
    private List<Task> tasks;
    private Dictionary<int, Task> byId;
    private Dictionary<Priority, HashSet<Task>> byPriority;
    private OrderedDictionary<int, HashSet<Task>> byConsumption;
  
    public ThreadExecutor()
    {
        this.tasks = new List<Task>();
        this.byId = new Dictionary<int, Task>();
        this.byPriority = new Dictionary<Priority, HashSet<Task>>();
        this.byConsumption = new OrderedDictionary<int, HashSet<Task>>();
    }

    public int Count => this.tasks.Count;

    public void ChangePriority(int id, Priority newPriority)
    {
        if (!this.byId.ContainsKey(id))
        {
            throw new ArgumentException();
        }
        Task currentTask = this.byId[id];
        Priority oldPriority = currentTask.TaskPriority;
        if (!this.byPriority.ContainsKey(currentTask.TaskPriority))
        {
            this.byPriority.Add(currentTask.TaskPriority, new HashSet<Task>());
        }
        
        this.byId[id].TaskPriority = newPriority;
        this.byPriority[oldPriority].Add(currentTask);
        this.tasks
            .Where(t => t.Id == currentTask.Id)
            .First()
            .TaskPriority = newPriority;
        this.byConsumption[currentTask.Consumption]
            .Where(t => t.Id == currentTask.Id)
            .First()
            .TaskPriority = newPriority;
        //Task currentTask = this.byId[id];
        //this.byPriority[currentTask.TaskPriority].Remove(currentTask);
        //currentTask.TaskPriority = newPriority;
        //if (!this.byPriority.ContainsKey(currentTask.TaskPriority))
        //{
        //    this.byPriority.Add(currentTask.TaskPriority, new HashSet<Task>());
        //}
        //this.byPriority[currentTask.TaskPriority].Add(currentTask);
        //this.tasks
        //    .Where(t => t.Id == currentTask.Id)
        //    .First()
        //    .TaskPriority = newPriority;
        //this.byConsumption[currentTask.Consumption]
        //    .Where(t => t.Id == currentTask.Id)
        //    .First()
        //    .TaskPriority = newPriority;
        //this.byId[currentTask.Id].TaskPriority = newPriority;
    }

    public bool Contains(Task task)
    {
        return this.byId.ContainsKey(task.Id) && this.byId[task.Id].CompareTo(task) == 0;
    }

    public int Cycle(int cycles)
    {
        if (this.tasks.Count == 0)
        {
            throw new InvalidOperationException();
        }
        int counter = 0;
        for (int i = this.tasks.Count - 1; i >= 0 ; i--)
        {
            int difference = this.tasks[i].Consumption - cycles;
            Task oldTask = this.tasks[i];
            
            //Task newTask = new Task(oldTask.Id, difference, oldTask.TaskPriority);

            //this.byPriority[oldTask.TaskPriority].Remove(oldTask);
            //this.byConsumption[oldTask.Consumption].Remove(oldTask);
            
            if (difference <= 0)
            {
                //Task oldTask = this.tasks[i];
                //Task newTask = new Task(oldTask.Id, difference, oldTask.TaskPriority);
                //this.byPriority[oldTask.TaskPriority].Remove(oldTask);
                //this.byConsumption[oldTask.Consumption].Remove(oldTask);
                //this.byId.Remove(oldTask.Id);
                this.tasks.Remove(oldTask);
                //this.byPriority[oldTask.TaskPriority].Remove(oldTask);
                //this.byConsumption[oldTask.Consumption].Remove(oldTask);
                counter++;
            }
            else
            {
                
                //this.byId[this.tasks[i].Id].ReduceConsumption(cycles);
                //this.byPriority[this.tasks[i].TaskPriority].Where(t=>t == this.tasks[i]).First().ReduceConsumption(cycles);
                //this.byConsumption[this.tasks[i].Consumption].Remove(this.tasks[i]);
                //if (!this.byConsumption.ContainsKey(this.tasks[i].Consumption))
                //{
                //    this.byConsumption.Add(this.tasks[i].Consumption, new HashSet<Task>());
                //}
                //this.byConsumption[this.tasks[i].Consumption].Add(this.tasks[i]);
                this.tasks[i].ReduceConsumption(cycles);
            }    
        }
        return counter;
    }

    public void Execute(Task task)
    {
        this.tasks.Add(task);
        if (this.byId.ContainsKey(task.Id))
        {
            throw new ArgumentException();
        }
        else
        {
            this.byId.Add(task.Id, task);
        }
        if (!this.byConsumption.ContainsKey(task.Consumption))
        {
            this.byConsumption.Add(task.Consumption, new HashSet<Task>());
        }
        this.byConsumption[task.Consumption].Add(task);
        if (!this.byPriority.ContainsKey(task.TaskPriority))
        {
            this.byPriority.Add(task.TaskPriority, new HashSet<Task>());
        }
        this.byPriority[task.TaskPriority].Add(task);
    }

    public IEnumerable<Task> GetByConsumptionRange(int lo, int hi, bool inclusive)
    {
        List<Task> result = new List<Task>();
        OrderedDictionary<int, HashSet<Task>>.View collection = 
            this.byConsumption.Range(lo, inclusive, hi, inclusive);
        if (collection.Count == 0)
        {
            return result;
        }
        foreach (var consumption in collection)
        {
            foreach (var task in consumption.Value.OrderByDescending(t=>t.TaskPriority))
            {
                result.Add(task);
            }
        }
        return result;
    }

    public Task GetById(int id)
    {
        if (!this.byId.ContainsKey(id) || this.byId != null)
        {
            throw new ArgumentException();
        }
        return this.byId[id];
    }

    public Task GetByIndex(int index)
    {
        if (index < 0 || index >= this.tasks.Count)
        {
            throw new ArgumentOutOfRangeException();
        }
        return this.tasks[index];
    }

    public IEnumerable<Task> GetByPriority(Priority type)
    {
        List<Task> result = new List<Task>();
        if (!this.byPriority.ContainsKey(type))
        {
            return result;
        }
        return this.byPriority[type].OrderByDescending(t => t.Id);
    }

    public IEnumerable<Task> GetByPriorityAndMinimumConsumption(Priority priority, int lo)
    {
        List<Task> result = new List<Task>();
        if (!this.byPriority.ContainsKey(priority))
        {
            return result;
        }
        foreach (var task in this.byPriority[priority])
        {
            if (task.Consumption >= lo)
            {
                result.Add(task);
            }
        }

        return result?.OrderByDescending(t=>t.Id);
    }


    public IEnumerator<Task> GetEnumerator()
    {
        foreach (var task in this.tasks)
        {
            yield return task;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();    
    }
}
