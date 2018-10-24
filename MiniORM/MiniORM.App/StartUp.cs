using System;

namespace MiniORM.App
{
    using System.Linq;
    using Data;
    using Data.Entities;

    public class StartUp
    {
        static void Main(string[] args)
        {
            var connectionString = @"Server=DESKTOP-D1G8VKM\SQLEXPRESS;DATABASE=MiniORM;Integrated Security=True";

            var context = new SoftUniDbContext(connectionString);

            context.Employees.Add(new Employee
            {
                FirstName = "Gosho",
                LastName = "Inserted",
                DepartmentId = context.Departments.First().Id,
                IsEmployeed = true,

            });

            var employee = context.Employees.Last();

            employee.FirstName = "Modified";

            context.SaveChanges();

        }
    }
}
