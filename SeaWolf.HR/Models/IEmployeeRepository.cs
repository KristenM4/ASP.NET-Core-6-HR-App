namespace SeaWolf.HR.Models
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> AllEmployees { get; }
        Employee? GetEmployeeById(int employeeId);
        IEnumerable<Employee> GetEmployeesForLocation(int locationId);
        IEnumerable<Employee> SearchEmployees(string searchQuery);
    }
}
