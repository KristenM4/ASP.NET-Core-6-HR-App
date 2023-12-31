﻿using AutoMapper;
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
    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    [ApiVersion("1.0")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeRepository employeeRepository,
            ILocationRepository locationRepository,
            ILogger<EmployeeController> logger,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _locationRepository = locationRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all employees in the database
        /// </summary>
        /// <returns>IActionResult</returns>
        /// <response code="200">Returns all employees</response>
        /// <response code="400">API has failed to get all employees</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// <response code="200">Returns the specified employee</response>
        /// <response code="404">Employee with that id does not exist</response>
        /// <response code="400">API has failed to get employee</response>
        [HttpGet("{id}", Name = "GetEmployeeDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Add a new employee to the database
        /// </summary>
        /// <param name="model">An AddEmployeeViewModel object with all required properties</param>
        /// <returns>An Employee ActionResult with the new employee's details in the database</returns>
        /// <response code="201">Displays new employee's details</response>
        /// <response code="400">Invalid data for new employee or API error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Employee> AddEmployee(AddEmployeeViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string? userName = User.Identity.Name;
                    var modelLocation = _locationRepository.GetLocationByName(model.Location);
                    if (modelLocation == null) return BadRequest();

                    var newEmployee = _mapper.Map<Employee>(model);
                    newEmployee.Location = modelLocation;

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

        /// <summary>
        /// Fully update an existing employee
        /// </summary>
        /// <param name="id">Id of the employee to update</param>
        /// <param name="model">An UpdateEmployeeViewModel object with all required properties</param>
        /// <returns>IActionResult</returns>
        /// <response code="204">Employee details successfully updated</response>
        /// <response code="404">Employee id or location name not valid</response>
        /// <response code="400">Invalid data for employee details or API error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
                    _mapper.Map(model, employee);
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

        /// <summary>
        /// Partially update an existing employee using JsonPatchDocument
        /// </summary>
        /// <param name="id">Id of the employee to partially update</param>
        /// <param name="patchDocument">A JsonPatchDocument object which updates an UpdateEmployeeViewModel property</param>
        /// <returns>IActionResult</returns>
        /// <response code="204">Employee details successfully updated</response>
        /// <response code="404">Employee id or location name not valid</response>
        /// <response code="400">Invalid data for employee details or API error</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PartiallyUpdateEmployee(int id, JsonPatchDocument<UpdateEmployeeViewModel> patchDocument)
        {
            try
            {
                string? userName = User.Identity.Name;
                var employeeInDB = _employeeRepository.GetEmployeeById(id);
                if (employeeInDB == null) return NotFound();

                // map Employee to UpdateEmployeeViewModel
                var employeeToPatch = _mapper.Map<UpdateEmployeeViewModel>(employeeInDB);
                employeeToPatch.Location = employeeInDB.Location.LocationName;

                patchDocument.ApplyTo(employeeToPatch, ModelState);

                if (!ModelState.IsValid) return BadRequest(ModelState);

                var employeeToPatchLocation = _locationRepository.GetLocationByName(employeeToPatch.Location);
                if (employeeToPatchLocation == null) return NotFound();

                // apply changes if it passes all validation checks

                _mapper.Map(employeeToPatch, employeeInDB);
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

        /// <summary>
        /// Delete an employee from the database
        /// </summary>
        /// <param name="id">Id of the employee to delete</param>
        /// <returns>IActionResult</returns>
        /// <response code="204">Employee has been sucessfully deleted</response>
        /// <response code="404">Employee id is not valid</response>
        /// <response code="400">API failed to delete employee</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        //ASYNC

        /// <summary>
        /// Get all employees in the database (async)
        /// </summary>
        /// <returns>IEnumberable of Employees</returns>
        /// <response code="200">Returns all employees asynchronously</response>
        /// <response code="400">API has failed to get all employees</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployeesAsync()
        {
            try
            {
                var allEmployees = await _employeeRepository.AllEmployeesAsync();
                return Ok(allEmployees);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Employee/GetAllEmployeesAsync: {ex}");
                return BadRequest("Failed to get all employees from Employee api");
            }
        }
    }
}
