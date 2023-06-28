using Microsoft.AspNetCore.Mvc;
using SeaWolf.HR.Models;
using SeaWolf.HR.ViewModels;

namespace SeaWolf.HR.Controllers
{
    public class AppController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        public AppController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public IActionResult Index()
        {
            EmployeeListViewModel employeeListViewModel = new EmployeeListViewModel
                (_employeeRepository.AllEmployees);
            return View(employeeListViewModel);
        }

        public IActionResult EmployeeDetails(int id)
        {
            var employee = _employeeRepository.GetEmployeeById(id);
            if(employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
    }
}
