using GraphQL;
using GraphQL.Types;
using System.Collections.Generic;
using System.Linq;

namespace graphqlapidemo
{
    public record Employee(int Id, string Name, int Age, int DeptId );

    public record Department(int Id, string Name);

    public class EmployeeDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string DeptName { get; set; }
    }

    public class EmployeeDetailsType : ObjectGraphType<EmployeeDetails>
    {
        public EmployeeDetailsType()
        {
            Field(x => x.Id);
            Field(x => x.Name);
            Field(x => x.Age);
            Field(x => x.DeptName);
        }
    }

    public interface IEmployeeService
    {
        public List<EmployeeDetails> GetEmployees();

        public List<EmployeeDetails> GetEmployee(int empId);

        public List<EmployeeDetails> GetEmployeesByDepartment(int deptId);
    }

    public class EmployeeService : IEmployeeService
    {
        public EmployeeService()
        {

        }
        private List<Employee> employees = new List<Employee>
        {
            new Employee(1, "Tom", 25, 1),
            new Employee(2, "Henry", 28, 1),
            new Employee(3, "Steve", 30, 2),
            new Employee(4, "Ben", 26, 2),
            new Employee(5, "John", 35, 3),

        };

        private List<Department> departments = new List<Department>
        {
            new Department(1, "IT"),
            new Department(2, "Finance"),
            new Department(3, "HR"),
        };

        public List<EmployeeDetails> GetEmployees()
        {
            return employees.Select(emp => new EmployeeDetails { 
                Id = emp.Id,
                Name = emp.Name,
                Age = emp.Age,
                DeptName = departments.First(d => d.Id == emp.DeptId).Name,
            }).ToList();
            
        }
        public List<EmployeeDetails> GetEmployee(int empId)
        {
            return employees.Where(emp => emp.Id == empId).Select(emp => new EmployeeDetails
            {
                Id = emp.Id,
                Name = emp.Name,
                Age = emp.Age,
                DeptName = departments.First(d => d.Id == emp.DeptId).Name,
            }).ToList();
        }

        public List<EmployeeDetails> GetEmployeesByDepartment(int deptId)
        {
            return employees.Where(emp => emp.DeptId == deptId).Select(emp => new EmployeeDetails
            {
                Id = emp.Id,
                Name = emp.Name,
                Age = emp.Age,
                DeptName = departments.First(d => d.Id == deptId).Name,
            }).ToList();
        }
    }

    public class EmployeeQuery : ObjectGraphType
    {
        public EmployeeQuery(IEmployeeService employeeService) {
            Field<ListGraphType<EmployeeDetailsType>>(Name = "Employees", resolve : x => employeeService.GetEmployees());
            Field<ListGraphType<EmployeeDetailsType>>(Name = "Employee", 
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id"}),
                resolve: x => employeeService.GetEmployee(x.GetArgument<int>("id")));
        }
    }

    public class EmployeeDetailsSchema : Schema
    {
        public EmployeeDetailsSchema(IServiceProvider serviceProvider) : base(serviceProvider) {
            Query = serviceProvider.GetRequiredService<EmployeeQuery>();
        }
    }
}
