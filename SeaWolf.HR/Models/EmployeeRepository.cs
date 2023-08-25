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

        /// <summary>
        /// Get all employees in the database
        /// </summary>
        /// <returns>IEnumerable of Employees</returns>
        public IEnumerable<Employee> AllEmployees => _context.Employees.
            Include(e=>e.Location).OrderBy(e => e.LastName).ThenBy(e => e.FirstName);

        /// <summary>
        /// Add a new employee to the database
        /// </summary>
        public void AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
        }

        /// <summary>
        /// Delete an employee from the database
        /// </summary>
        public void DeleteEmployee(int employeeId)
        {
            var employee = GetEmployeeById(employeeId);

            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }
        }

        /// <summary>
        /// Get a specific employee from the database
        /// </summary>
        /// <returns>An Employee object</returns>
        public Employee? GetEmployeeById(int employeeId)
        {
            return _context.Employees.Include(e => e.Location).FirstOrDefault(e => e.EmployeeId == employeeId);
        }

        /// <summary>
        /// Save changes to the database
        /// </summary>
        /// <returns>True if any changes were saved, false if no changes were saved</returns>
        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        /// <summary>
        /// Keyword search and sort for employees
        /// </summary>
        /// <returns>IEnumerable of Employees</returns>
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

        // ASYNC

        /// <summary>
        /// Get all employees in the database. This is an async method.
        /// </summary>
        /// <returns>Async Task: IEnumerable of Employees</returns>
        public async Task<IEnumerable<Employee>> AllEmployeesAsync()
        {
            return await _context.Employees.
            Include(e => e.Location).OrderBy(e => e.LastName).ThenBy(e => e.FirstName)
            .ToListAsync();
        }

        /// <summary>
        /// Save changes to the database asynchronously
        /// </summary>
        /// <returns>Task with result of how many changes were saved</returns>
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
