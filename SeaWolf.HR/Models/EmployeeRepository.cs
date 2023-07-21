using Microsoft.EntityFrameworkCore;

namespace SeaWolf.HR.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly SeaWolfHRDbContext _context;

        public EmployeeRepository(SeaWolfHRDbContext seaWolfHRDbContext)
        {
            _context = seaWolfHRDbContext;
        }

        public IEnumerable<Employee> AllEmployees => _context.Employees.
            Include(e=>e.Location).OrderBy(e => e.LastName).ThenBy(e => e.FirstName);

        public void AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
        }

        public void DeleteEmployee(int employeeId)
        {
            var employee = GetEmployeeById(employeeId);

            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }
        }

        public Employee? GetEmployeeById(int employeeId)
        {
            return _context.Employees.Include(e => e.Location).FirstOrDefault(e => e.EmployeeId == employeeId);
        }

        public IEnumerable<Employee> GetEmployeesForLocation(int locationId)
        {
            return _context.Employees.Where(e => e.Location.LocationId == locationId).ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<Employee> SearchEmployees(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return _context.Employees.
                    Include(e => e.Location).OrderBy(e => e.LastName).ThenBy(e => e.FirstName);
            }
            else
            {
                return _context.Employees.Include(e => e.Location).OrderBy(e => e.LastName)
                    .Where(e =>
                    e.FirstName.Contains(searchQuery) || e.LastName.Contains(searchQuery) ||
                    e.MiddleName.Contains(searchQuery) || e.Position.Contains(searchQuery) ||
                    e.Location.LocationName.Contains(searchQuery) || e.Phone.Contains(searchQuery) ||
                    e.Email.Contains(searchQuery)
                    );
            }
        }
    }
}
