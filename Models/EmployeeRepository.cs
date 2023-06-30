namespace SeaWolf.HR.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly SeaWolfHRDbContext _context;

        public EmployeeRepository(SeaWolfHRDbContext seaWolfHRDbContext)
        {
            _context = seaWolfHRDbContext;
        }

        public IEnumerable<Employee> AllEmployees => _context.Employees;

        public Employee? GetEmployeeById(int employeeId)
        {
            return _context.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
        }
    }
}
