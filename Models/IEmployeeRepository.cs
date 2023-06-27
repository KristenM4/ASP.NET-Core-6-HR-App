namespace SeaWolf.HR.Models
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> AllEmployees { get; }
        Employee? GetEmployeeById(int employeeId);
    }
}
