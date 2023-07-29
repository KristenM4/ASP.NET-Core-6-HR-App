using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SeaWolf.HR.Models;
using SeaWolf.HR.ViewModels;
using SeaWolf.HR.Views.App;

namespace SeaWolf.HR.Controllers.Api
{
    [Route("api/v{version:apiVersion}/[controller]")]
    // [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    [ApiVersion("1.0")]
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

        /// <summary>
        /// Get an employee by id number
        /// </summary>
        /// <param name="id">Id of the employee to get</param>
        /// <returns>IActionResult</returns>
        [HttpGet("{id}", Name = "GetEmployeeDetails")]
        public IActionResult GetEmployeeDetails(int id)
        {
            try
            {
                string? userName = User.Identity.Name;
                var employee = _employeeRepository.GetEmployeeById(id);
                if (employee == null)
                {
                    return NotFound();
                }

                _logger.LogInformation($"Employee details were accessed by {userName}");
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
                if (ModelState.IsValid)
                {
                    string? userName = User.Identity.Name;
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
                        _logger.LogInformation($"New employee with id of {newEmployee.EmployeeId} created by {userName}");
                        return CreatedAtRoute("GetEmployeeDetails", new { id = newEmployee.EmployeeId }, newEmployee);
                    }
                    else return BadRequest();
                }
                else return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to post Employee/AddEmployee: {ex}");
                return BadRequest("Failed to add new employee with Employee api");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, UpdateEmployeeViewModel model)
        {
            try
            {
                string? userName = User.Identity.Name;
                var employee = _employeeRepository.GetEmployeeById(id);
                if (employee == null) return NotFound();

                var modelLocation = _locationRepository.GetLocationByName(model.Location);
                if (modelLocation == null) return NotFound();

                if (ModelState.IsValid)
                {
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
                        _logger.LogInformation($"Details of employee id {id} updated(HTTP Put) by {userName}");
                        return NoContent();

                    }
                    else return BadRequest();
                }
                else return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to put Employee/UpdateEmployee: {ex}");
                return BadRequest("Failed to update employee details with Employee api");
            }

        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateEmployee(int id, JsonPatchDocument<UpdateEmployeeViewModel> patchDocument)
        {
            try
            {
                string? userName = User.Identity.Name;
                var employeeInDB = _employeeRepository.GetEmployeeById(id);
                if (employeeInDB == null) return NotFound();

                // map Employee to UpdateEmployeeViewModel
                var employeeToPatch = new UpdateEmployeeViewModel()
                {
                    FirstName = employeeInDB.FirstName,
                    LastName = employeeInDB.LastName,
                    MiddleName = employeeInDB.MiddleName,
                    DateOfBirth = employeeInDB.DateOfBirth,
                    Email = employeeInDB.Email,
                    Phone = employeeInDB.Phone,
                    Position = employeeInDB.Position,
                    Location = employeeInDB.Location.LocationName
                };

                patchDocument.ApplyTo(employeeToPatch, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!TryValidateModel(employeeToPatch))
                {
                    return BadRequest(ModelState);
                }

                var employeeToPatchLocation = _locationRepository.GetLocationByName(employeeToPatch.Location);
                if (employeeToPatchLocation == null) return NotFound();

                // apply changes if it passes all validation checks

                employeeInDB.FirstName = employeeToPatch.FirstName;
                employeeInDB.LastName = employeeToPatch.LastName;
                employeeInDB.MiddleName = employeeToPatch.MiddleName;
                employeeInDB.DateOfBirth = employeeToPatch.DateOfBirth;
                employeeInDB.Email = employeeToPatch.Email;
                employeeInDB.Phone = employeeToPatch.Phone;
                employeeInDB.Position = employeeToPatch.Position;
                employeeInDB.Location = employeeToPatchLocation;

                _logger.LogInformation($"Details of employee id {id} partially updated(HTTP Patch) by {userName}");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to patch Employee/PartiallyUpdateEmployee: {ex}");
                return BadRequest("Failed to partially update employee details with Employee api");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                string? userName = User.Identity.Name;
                var employee = _employeeRepository.GetEmployeeById(id);
                if (employee == null) return NotFound();

                _employeeRepository.DeleteEmployee(id);
                if (_employeeRepository.Save())
                {
                    _logger.LogInformation($"Employee with id {id} deleted by {userName}");
                    return NoContent();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete at Employee/DeleteEmployee: {ex}");
                return BadRequest("Failed to delete employee with Employee api");
            }
        }
    }
}
