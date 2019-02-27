using System;

class Launcher
{
    static void Main(string[] args)
    {
        IEnterprise enterprise = new Enterprise();

        Employee employee = new Employee("pesho", "gosho", 123, Position.Hr, DateTime.Now);
        enterprise.Add(employee);

        Employee toReplace = new Employee("replaced", "replacedEmployee", 555, Position.Owner, DateTime.Now);
        enterprise.Add(new Employee("sosho", "pesho", 321, Position.Manager, DateTime.Now));
        enterprise.Add(new Employee("posho", "kosho", 55, Position.Hr, DateTime.Now));
        enterprise.Add(new Employee("gosho", "mosho", 1, Position.Manager, DateTime.Now));

        Console.WriteLine(
        enterprise.Change(employee.Id, toReplace));
        Console.WriteLine(enterprise.Contains(employee.Id));

        Employee byUuid = enterprise.GetByGuid(employee.Id);
        Console.WriteLine(byUuid.FirstName); //REPLACED
        Console.WriteLine(
        byUuid.LastName);//REPLACED EMPLOYEE
        Console.WriteLine(byUuid.Salary);
    }
}
