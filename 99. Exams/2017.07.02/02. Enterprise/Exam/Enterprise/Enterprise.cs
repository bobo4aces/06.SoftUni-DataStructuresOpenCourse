using System;
using System.Collections;
using System.Collections.Generic;

public class Enterprise : IEnterprise
{
    private Dictionary<Guid, Employee> byGuid;
    private Dictionary<DateTime, HashSet<Employee>> byHireDate;
    private Dictionary<Position, HashSet<Employee>> byPosition;
    private Dictionary<double, HashSet<Employee>> bySalary;
    private Dictionary<string, HashSet<Employee>> byFirstName;

    public Enterprise()
    {
        this.byGuid = new Dictionary<Guid, Employee>();
        this.byHireDate = new Dictionary<DateTime, HashSet<Employee>>();
        this.byPosition = new Dictionary<Position, HashSet<Employee>>();
        this.bySalary = new Dictionary<double, HashSet<Employee>>();
        this.byFirstName = new Dictionary<string, HashSet<Employee>>();
    }
    public int Count => this.byGuid.Count;

    public void Add(Employee employee)
    {
        if (!this.byGuid.ContainsKey(employee.Id))
        {
            this.byGuid.Add(employee.Id, null);
        }
        this.byGuid[employee.Id] = employee;
        if (!this.byHireDate.ContainsKey(employee.HireDate))
        {
            this.byHireDate.Add(employee.HireDate, new HashSet<Employee>());
        }
        this.byHireDate[employee.HireDate].Add(employee);
        if (!this.byPosition.ContainsKey(employee.Position))
        {
            this.byPosition.Add(employee.Position, new HashSet<Employee>());
        }
        this.byPosition[employee.Position].Add(employee);
        if (!this.bySalary.ContainsKey(employee.Salary))
        {
            this.bySalary.Add(employee.Salary, new HashSet<Employee>());
        }
        this.bySalary[employee.Salary].Add(employee);
        if (!this.byFirstName.ContainsKey(employee.FirstName))
        {
            this.byFirstName.Add(employee.FirstName, new HashSet<Employee>());
        }
        this.byFirstName[employee.FirstName].Add(employee);
    }

    public IEnumerable<Employee> AllWithPositionAndMinSalary(Position position, double minSalary)
    {
        List<Employee> result = new List<Employee>();
        if (!this.byPosition.ContainsKey(position))
        {
            return result;
        }

        foreach (var employee in this.byPosition[position])
        {
            if (employee.Salary >= minSalary)
            {
                result.Add(employee);
            }
        }
        return result;
    }

    public bool Change(Guid guid, Employee employee)
    {
        if (this.byGuid.ContainsKey(guid))
        {
            Employee currentEmployee = this.byGuid[guid];
            this.Fire(guid);
            currentEmployee.FirstName = employee.FirstName;
            currentEmployee.HireDate = employee.HireDate;
            currentEmployee.LastName = employee.LastName;
            currentEmployee.Position = employee.Position;
            currentEmployee.Salary = employee.Salary;
            this.Add(currentEmployee);
            return true;
        }
        return false;
    }

    public bool Contains(Guid guid)
    {
        return this.byGuid.ContainsKey(guid);
    }

    public bool Contains(Employee employee)
    {
        if (!this.byGuid.ContainsKey(employee.Id))
        {
            return false;
        }
        return true;
    }

    public bool Fire(Guid guid)
    {
        if (!this.byGuid.ContainsKey(guid))
        {
            return false;
        }
        Employee employee = this.byGuid[guid];
        this.byGuid.Remove(guid);
        this.byHireDate[employee.HireDate].Remove(employee);
        this.byPosition[employee.Position].Remove(employee);
        this.bySalary[employee.Salary].Remove(employee);
        this.byFirstName[employee.FirstName].Remove(employee);
        return true;
    }

    public Employee GetByGuid(Guid guid)
    {
        if (!this.byGuid.ContainsKey(guid))
        {
            throw new ArgumentException();
        }
        return this.byGuid[guid];
    }

    public IEnumerable<Employee> GetByPosition(Position position)
    {
        if (!this.byPosition.ContainsKey(position))
        {
            throw new ArgumentException();
        }
        return this.byPosition[position];
    }

    public IEnumerable<Employee> GetBySalary(double minSalary)
    {
        List<Employee> result = new List<Employee>();
        foreach (var salary in this.bySalary)
        {
            if (salary.Key >= minSalary)
            {
                result.AddRange(salary.Value);
            }
        }
        if (result.Count == 0)
        {
            throw new InvalidOperationException();
        }
        return result;
    }

    public IEnumerable<Employee> GetBySalaryAndPosition(double salary, Position position)
    {
        List<Employee> result = new List<Employee>();
        if (!this.bySalary.ContainsKey(salary))
        {
            throw new InvalidOperationException();
        }
        foreach (var employee in this.bySalary[salary])
        {
            if (employee.Position == position)
            {
                result.Add(employee);
            }
        }
        if (result.Count == 0)
        {
            throw new InvalidOperationException();
        }
        return result;
    }

    public IEnumerator<Employee> GetEnumerator()
    {
        foreach (var id in this.byGuid)
        {
            yield return id.Value;
        }
    }

    public Position PositionByGuid(Guid guid)
    {
        if (!this.byGuid.ContainsKey(guid))
        {
            throw new InvalidOperationException();
        }
        return this.byGuid[guid].Position;
    }

    public bool RaiseSalary(int months, int percent)
    {
        DateTime currentDate = DateTime.Now;
        bool isRaise = false;
        foreach (var date in this.byHireDate)
        {
            TimeSpan timespan = currentDate.Subtract(date.Key);
            if (timespan.Days > months * 30)
            {
                foreach (var employee in date.Value)
                {
                    double oldSalary = employee.Salary;
                    employee.Salary += employee.Salary * ((double)percent / 100);
                    this.bySalary[oldSalary].Remove(employee);
                    if (!this.bySalary.ContainsKey(employee.Salary))
                    {
                        this.bySalary.Add(employee.Salary, new HashSet<Employee>());
                    }
                    this.bySalary[employee.Salary].Add(employee);
                }
                isRaise = true;
            }
        }
        return isRaise;
    }

    public IEnumerable<Employee> SearchByFirstName(string firstName)
    {
        if (!this.byFirstName.ContainsKey(firstName))
        {
            return new List<Employee>();
        }
        return this.byFirstName[firstName];
    }

    public IEnumerable<Employee> SearchByNameAndPosition(string firstName, string lastName, Position position)
    {
        List<Employee> result = new List<Employee>();
        if (!this.byFirstName.ContainsKey(firstName))
        {
            return result;
        }

        foreach (var employee in this.byFirstName[firstName])
        {
            if (employee.LastName == lastName && employee.Position == position)
            {
                result.Add(employee);
            }
        }
        return result;
    }

    public IEnumerable<Employee> SearchByPosition(IEnumerable<Position> positions)
    {
        List<Employee> result = new List<Employee>();
        if (positions == null)
        {
            return result;
        }
        foreach (var position in positions)
        {
            if (this.byPosition.ContainsKey(position))
            {
                result.AddRange(this.byPosition[position]);
            }
        }
        return result;
    }

    public IEnumerable<Employee> SearchBySalary(double minSalary, double maxSalary)
    {
        List<Employee> result = new List<Employee>();
        foreach (var salary in this.bySalary)
        {
            if (minSalary <= salary.Key && salary.Key <= maxSalary)
            {
                result.AddRange(salary.Value);
            }
        }
        return result;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}

