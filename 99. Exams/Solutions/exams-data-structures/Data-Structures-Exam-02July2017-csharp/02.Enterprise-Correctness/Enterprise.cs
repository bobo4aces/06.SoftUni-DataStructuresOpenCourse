using System;
using System.Collections;
using System.Collections.Generic;

public class Enterprise : IEnterprise
{
    private Dictionary<Guid, Employee> employeesById;

    public Enterprise()
    {
        this.employeesById = new Dictionary<Guid, Employee>();
        this.Count = 0;
    }

    public int Count { get; private set; }

    public void Add(Employee employee)
    {
        this.employeesById.Add(employee.Id, employee);
        this.Count++;
    }

    public IEnumerable<Employee> AllWithPositionAndMinSalary(Position position, double minSalary)
    {
        LinkedList<Employee> employees = new LinkedList<Employee>();

        foreach (var employee in this.employeesById)
        {
            if (employee.Value.Position == position && employee.Value.Salary >= minSalary)
            {
                employees.AddLast(employee.Value);
            }
        }

        return employees;
    }

    public bool Change(Guid guid, Employee employee)
    {
        if (this.employeesById.ContainsKey(guid))
        {
            this.employeesById[guid] = employee;
            return true;
        }

        return false;
    }

    public bool Contains(Guid guid)
    {
        return this.employeesById.ContainsKey(guid);
    }

    public bool Contains(Employee employee)
    {
        return this.employeesById.ContainsKey(employee.Id);
    }

    public bool Fire(Guid guid)
    {
        if (this.Contains(guid))
        {
            this.employeesById.Remove(guid);
            this.Count--;
            return true;
        }

        return false;
    }

    public Employee GetByGuid(Guid guid)
    {
        if (!this.employeesById.ContainsKey(guid))
        {
            throw new ArgumentException();
        }

        return this.employeesById[guid];
    }

    public IEnumerable<Employee> GetByPosition(Position position)
    {
        LinkedList<Employee> employees = new LinkedList<Employee>();

        foreach (var employee in this.employeesById)
        {
            if (employee.Value.Position == position)
            {
                employees.AddLast(employee.Value);
            }
        }

        if (employees.Count == 0)
        {
            throw new ArgumentException();
        }

        return employees;
    }

    public IEnumerable<Employee> GetBySalary(double minSalary)
    {
        LinkedList<Employee> employees = new LinkedList<Employee>();

        foreach (var employee in this.employeesById)
        {
            if (employee.Value.Salary >= minSalary)
            {
                employees.AddLast(employee.Value);
            }
        }

        if (employees.Count == 0)
        {
            throw new InvalidOperationException();
        }

        return employees;
    }

    public IEnumerable<Employee> GetBySalaryAndPosition(double salary, Position position)
    {
        LinkedList<Employee> employees = new LinkedList<Employee>();

        foreach (var employee in this.employeesById)
        {
            if (employee.Value.Position == position && employee.Value.Salary.Equals(salary))
            {
                employees.AddLast(employee.Value);
            }
        }

        if (employees.Count == 0)
        {
            throw new InvalidOperationException();
        }

        return employees;
    }

    public IEnumerator<Employee> GetEnumerator()
    {
        foreach (var employee in employeesById)
        {
            yield return employee.Value;
        }
    }

    public Position PositionByGuid(Guid guid)
    {
        if (!this.Contains(guid))
        {
            throw new ArgumentException();
        }

        return this.employeesById[guid].Position;
    }

    public bool RaiseSalary(int months, int percent)
    {
        DateTime current = DateTime.Now;

        bool hasRaised = false;

        foreach (var employee in employeesById)
        {
            DateTime employeeHireDate = employee.Value.HireDate;

            int differnenceInMonths = ((current.Year - employeeHireDate.Year)*12) + current.Month - employeeHireDate.Month;

            if (differnenceInMonths >= months)
            {
                employee.Value.Salary += (employee.Value.Salary*percent)/100;
                hasRaised = true;
            }
        }

        return hasRaised;
    }

    public IEnumerable<Employee> SearchByFirstName(string firstName)
    {
        LinkedList<Employee> employees = new LinkedList<Employee>();

        foreach (var employee in this.employeesById)
        {
            if (employee.Value.FirstName == firstName)
            {
                employees.AddLast(employee.Value);
            }
        }

        return employees;
    }

    public IEnumerable<Employee> SearchByNameAndPosition(string firstName, string lastName, Position position)
    {
        LinkedList<Employee> employees = new LinkedList<Employee>();

        foreach (var employee in this.employeesById)
        {
            if (employee.Value.FirstName == firstName && employee.Value.LastName == lastName && employee.Value.Position == position)
            {
                employees.AddLast(employee.Value);
            }
        }

        return employees;
    }

    public IEnumerable<Employee> SearchByPosition(IEnumerable<Position> positions)
    {
        HashSet<Position> searchPositions = new HashSet<Position>(positions);
        LinkedList<Employee> employees = new LinkedList<Employee>();

        foreach (var employee in this.employeesById)
        {
            if (searchPositions.Contains(employee.Value.Position))
            {
                employees.AddLast(employee.Value);
            }
        }

        return employees;
    }

    public IEnumerable<Employee> SearchBySalary(double minSalary, double maxSalary)
    {
        LinkedList<Employee> employees = new LinkedList<Employee>();

        foreach (var employee in this.employeesById)
        {
            if (employee.Value.Salary >= minSalary && employee.Value.Salary <= maxSalary)
            {
                employees.AddLast(employee.Value);
            }
        }

        return employees;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}

