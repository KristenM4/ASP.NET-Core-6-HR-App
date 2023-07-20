using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeaWolf.HR.Models;
using SeaWolf.HR.ViewModels;

namespace SeaWolf.HR.Controllers
{
    public class AppController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ILogger<AppController> _logger;

        public AppController(IEmployeeRepository employeeRepository, ILocationRepository locationRepository, ILogger<AppController> logger)
        {
            _employeeRepository = employeeRepository;
            _locationRepository = locationRepository;
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                return View();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get App/Index: {ex}");
                return BadRequest("Failed to get home page");
            }
        }

        public IActionResult EmployeeList()
        {
            try
            {
                EmployeeListViewModel employeeListViewModel = new EmployeeListViewModel
                    (_employeeRepository.AllEmployees);
                return View(employeeListViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get App/EmployeeList: {ex}");
                return BadRequest("Failed to get employee list");
            }
        }

        [Authorize]
        public IActionResult EmployeeDetails(int id)
        {
            try
            {
                var employee = _employeeRepository.GetEmployeeById(id);
                if (employee == null)
                {
                    return NotFound();
                }
                return View(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get App/EmployeeDetails: {ex}");
                return BadRequest("Failed to get employee details");
            }
        }

        public IActionResult AddEmployee()
        {
            try
            {
                var allLocations = _locationRepository.AllLocations;
                ViewBag.AllLocations = allLocations.ToList();
                return View();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get App/AddEmployee: {ex}");
                return BadRequest("Failed to get add employee page");
            }
        }

        public IActionResult LocationList()
        {
            try
            {
                LocationListViewModel locationListViewModel = new LocationListViewModel
                (_locationRepository.AllLocations);
                return View(locationListViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get App/LocationList: {ex}");
                return BadRequest("Failed to get location list");
            }
        }

        public IActionResult LocationDetails(int id)
        {
            try
            {
                var location = _locationRepository.GetLocationById(id);
                if (location == null)
                {
                    return NotFound();
                }
                ViewBag.Employees = _employeeRepository.GetEmployeesForLocation(id);
                return View(location);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get App/LocationDetails: {ex}");
                return BadRequest("Failed to get location details");
            }
        }
    }
}
