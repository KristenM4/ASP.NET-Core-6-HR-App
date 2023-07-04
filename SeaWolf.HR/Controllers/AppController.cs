using Microsoft.AspNetCore.Mvc;
using SeaWolf.HR.Models;
using SeaWolf.HR.ViewModels;

namespace SeaWolf.HR.Controllers
{
    public class AppController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILocationRepository _locationRepository;

        public AppController(IEmployeeRepository employeeRepository, ILocationRepository locationRepository)
        {
            _employeeRepository = employeeRepository;
            _locationRepository = locationRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult EmployeeList()
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

        public IActionResult LocationList()
        {
            LocationListViewModel locationListViewModel = new LocationListViewModel
                (_locationRepository.AllLocations);
            return View(locationListViewModel);
        }

        public IActionResult LocationDetails(int id)
        {
            var location = _locationRepository.GetLocationById(id);
            if(location == null)
            {
                return NotFound();
            }
            ViewBag.Employees = _employeeRepository.GetEmployeesForLocation(id);
            return View(location);
        }
    }
}
