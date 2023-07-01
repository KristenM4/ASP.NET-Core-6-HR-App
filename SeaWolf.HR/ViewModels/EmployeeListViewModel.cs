using SeaWolf.HR.Models;

namespace SeaWolf.HR.ViewModels
{
    public class EmployeeListViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }
        public EmployeeListViewModel(IEnumerable<Employee> employees) 
        {
            Employees = employees;
        }
    }
}
