using Microsoft.AspNetCore.Mvc;

namespace graphqlapidemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService) {
            _employeeService = employeeService;
        }

        [HttpGet("~/Employees")]
        public IEnumerable<EmployeeDetails> GetEmployees()
        {
            return _employeeService.GetEmployees();
        }

        [HttpGet("~/EmployeesById")]
        public IEnumerable<EmployeeDetails> GetEmployeeById(int empId)
        {
            return _employeeService.GetEmployee(empId);
        }

        [HttpGet("~/EmployeesByDept")]
        public IEnumerable<EmployeeDetails> GetEmployeesByDeprtment(int deptId)
        {
            return _employeeService.GetEmployeesByDepartment(deptId);
        }
    }
}
