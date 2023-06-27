using Microsoft.AspNetCore.Mvc;
using SeaWolf.HR.Models;

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
            return View(_employeeRepository.AllEmployees);
        }
    }
}
