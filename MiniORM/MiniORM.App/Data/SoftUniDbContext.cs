using System;
using System.Collections.Generic;
using System.Text;

namespace MiniORM.App.Data
{
    using Entities;
    public class SoftUniDbContext: DBContext
    {
        public SoftUniDbContext(string connectionString) : base(connectionString) { }

        public DBSet<Employee> Employees { get; }

        public DBSet<Project> Projects { get; }

        public DBSet<Department> Departments { get; }

        public DBSet<EmployeeProject> EmployeesProjects { get; }
    }
}
