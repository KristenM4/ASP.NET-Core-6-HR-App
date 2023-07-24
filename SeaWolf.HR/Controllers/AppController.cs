using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

        public AppController(IEmployeeRepository employeeRepository,
            ILocationRepository locationRepository,
            ILogger<AppController> logger)
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

        [Authorize]
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

        [Authorize]
        [HttpPost]
        public IActionResult AddEmployee(AddEmployeeViewModel model)
        {
            var allLocations = _locationRepository.AllLocations;
            ViewBag.AllLocations = allLocations.ToList();

            if (ModelState.IsValid)
            {
                var modelLocation = _locationRepository.GetLocationByName(model.Location);

                var newEmployee = new Employee()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    DateOfBirth = model.DateOfBirth,
                    Email = model.Email,
                    Phone = model.Phone,
                    Position = model.Position,
                    Location = modelLocation,
                };

                _employeeRepository.AddEmployee(newEmployee);

                if (_employeeRepository.Save())
                {
                    return RedirectToAction("EmployeeDetails", "App", new { id = newEmployee.EmployeeId });
                }
            }

            ModelState.AddModelError("", "Failed to add employee");

            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult EditEmployee(int id)
        {
            try
            {
                var employee = _employeeRepository.GetEmployeeById(id);
                if (employee == null)
                {
                    return NotFound();
                }
                ViewBag.Employee = employee;

                var allLocations = _locationRepository.AllLocations.ToList();
                allLocations.Remove(employee.Location);
                ViewBag.AllLocations = allLocations;

                return View();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get App/EditEmployee: {ex}");
                return BadRequest("Failed to get edit employee page");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditEmployee(int id, UpdateEmployeeViewModel model)
        {
            try
            {
                var employee = _employeeRepository.GetEmployeeById(id);
                if (employee == null)
                {
                    return NotFound();
                }
                ViewBag.Employee = employee;

                var allLocations = _locationRepository.AllLocations.ToList();
                allLocations.Remove(employee.Location);
                ViewBag.AllLocations = allLocations;

                if (ModelState.IsValid)
                {
                    var modelLocation = _locationRepository.GetLocationByName(model.Location);

                    employee.FirstName = model.FirstName;
                    employee.LastName = model.LastName;
                    employee.MiddleName = model.MiddleName;
                    employee.DateOfBirth = model.DateOfBirth;
                    employee.Email = model.Email;
                    employee.Phone = model.Phone;
                    employee.Position = model.Position;
                    employee.Location = modelLocation;


                    if (_employeeRepository.Save())
                    {
                        return RedirectToAction("EmployeeDetails", "App", new { id = employee.EmployeeId });
                    }
                }

                ModelState.AddModelError("", "Failed to edit employee");

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to post App/EditEmployee: {ex}");
                return BadRequest("Failed to edit employee");
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult DeleteEmployee(int id)
        {
            _employeeRepository.DeleteEmployee(id);

            if (_employeeRepository.Save())
            {
                return RedirectToAction("EmployeeList", "App");
            }

            return BadRequest();
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

        [Authorize]
        [HttpGet]
        public IActionResult AddLocation()
        {
            try
            {
                return View();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get App/AddLocation: {ex}");
                return BadRequest("Failed to get add location page");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddLocation(AddLocationViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (model.AddressLine2 == null) model.AddressLine2 = string.Empty;

                var newLocation = new Location()
                {
                    LocationName = model.LocationName,
                    Phone = model.Phone,
                    AddressLine1 = model.AddressLine1,
                    AddressLine2 = model.AddressLine2,
                    City = model.City,
                    State = model.State,
                    PostalCode = model.PostalCode,
                    Country = model.Country
                };

                _locationRepository.AddLocation(newLocation);

                if (_locationRepository.Save())
                {
                    return RedirectToAction("LocationDetails", "App", new { id = newLocation.LocationId });
                }
            }

            ModelState.AddModelError("", "Failed to add location");

            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult EditLocation(int id)
        {
            try
            {
                var location = _locationRepository.GetLocationById(id);
                if (location == null)
                {
                    return NotFound();
                }
                ViewBag.Location = location;
                return View();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get App/EditLocation: {ex}");
                return BadRequest("Failed to get edit location page");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditLocation(int id, AddLocationViewModel model)
        {
            try
            {
                var location = _locationRepository.GetLocationById(id);
                if (location == null)
                {
                    return NotFound();
                }
                ViewBag.Location = location;

                if (ModelState.IsValid)
                {
                    if (model.AddressLine2 == null) model.AddressLine2 = string.Empty;

                    location.LocationName = model.LocationName;
                    location.Phone = model.Phone;
                    location.AddressLine1 = model.AddressLine1;
                    location.AddressLine2 = model.AddressLine2;
                    location.City = model.City;
                    location.State = model.State;
                    location.PostalCode = model.PostalCode;
                    location.Country = model.Country;


                    if (_locationRepository.Save())
                    {
                        return RedirectToAction("LocationDetails", "App", new { id = location.LocationId });
                    }
                }

                ModelState.AddModelError("", "Failed to edit location");

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to post App/EditLocation: {ex}");
                return BadRequest("Failed to edit location");
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult DeleteLocation(int id)
        {
            _locationRepository.DeleteLocation(id);

            if (_locationRepository.Save())
            {
                return RedirectToAction("LocationList", "App");
            }

            return BadRequest();
        }
    }
}
