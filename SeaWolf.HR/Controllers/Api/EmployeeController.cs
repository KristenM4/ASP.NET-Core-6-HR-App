using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeaWolf.HR.Models;
using SeaWolf.HR.ViewModels;
using SeaWolf.HR.Views.App;

namespace SeaWolf.HR.Controllers.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeRepository employeeRepository, ILocationRepository locationRepository, ILogger<EmployeeController> logger)
        {
            _employeeRepository = employeeRepository;
            _locationRepository = locationRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            try
            {
                var allEmployees = _employeeRepository.AllEmployees;
                return Ok(allEmployees);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Employee/GetAllEmployees: {ex}");
                return BadRequest("Failed to get all employees from Employee api");
            }
        }

        [HttpGet("{id}", Name = "GetEmployeeDetails")]
        public IActionResult GetEmployeeDetails(int id)
        {
            try
            {
                var employee = _employeeRepository.GetEmployeeById(id);
                if (employee == null)
                {
                    return NotFound();
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Employee/GetEmployeeDetails: {ex}");
                return BadRequest("Failed to get employee from Employee api");
            }
        }

        [HttpPost]
        public ActionResult<Employee> AddEmployee(AddEmployeeViewModel model)
        {
            try
            {
                var modelLocation = _locationRepository.GetLocationByName(model.Location);
                if (modelLocation == null) return BadRequest();

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
                    return CreatedAtRoute("GetEmployeeDetails", new { id = newEmployee.EmployeeId });
                }
                else return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to post Employee/AddEmployee: {ex}");
                return BadRequest("Failed to add new employee with Employee api");
            }
        }
    }
}
