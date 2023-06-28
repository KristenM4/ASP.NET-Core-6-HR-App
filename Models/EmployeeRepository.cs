namespace SeaWolf.HR.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly SeaWolfHRDbContext _seaWolfHRDbContext;

        public EmployeeRepository(SeaWolfHRDbContext seaWolfHRDbContext)
        {
            _seaWolfHRDbContext = seaWolfHRDbContext;
        }

        public IEnumerable<Employee> AllEmployees => _seaWolfHRDbContext.Employees;

        public Employee? GetEmployeeById(int employeeId)
        {
            return _seaWolfHRDbContext.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
        }
    }
}
