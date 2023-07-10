﻿using Microsoft.AspNetCore.Mvc;
using SeaWolf.HR.Models;
using static System.Net.Mime.MediaTypeNames;

namespace SeaWolf.HR.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<SearchController> _logger;

        public SearchController(IEmployeeRepository employeeRepository, ILogger<SearchController> logger)
        {
            _employeeRepository = employeeRepository;
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
                _logger.LogError($"Failed to get Search/GetAllEmployees: {ex}");
                return BadRequest("Failed to get all employees from Search api");
            }
        }

        [HttpPost]
        public IActionResult SearchEmployees([FromBody] string values)
        {
            IEnumerable<Employee> employees = new List<Employee>();
            IEnumerable<Employee> sortedEmployees;

            string[] newValues = values.Split("$$");
            string searchQuery = newValues[0];
            string sorter = newValues[1];

            try
            {
                employees = _employeeRepository.SearchEmployees(searchQuery);

                switch (sorter)
                {
                    case "NameDesc":
                        sortedEmployees = employees.OrderByDescending(e => e.LastName).ThenBy(e => e.FirstName);
                        break;
                    case "Position":
                        sortedEmployees = employees.OrderBy(e => e.Position)
                            .ThenBy(e => e.LastName).ThenBy(e => e.FirstName);
                        break;
                    case "PositionDesc":
                        sortedEmployees = employees.OrderByDescending(e => e.Position)
                            .ThenBy(e => e.LastName).ThenBy(e => e.FirstName);
                        break;
                    case "Location":
                        sortedEmployees = employees.OrderBy(e => e.Location.LocationName)
                            .ThenBy(e => e.LastName).ThenBy(e => e.FirstName);
                        break;
                    case "LocationDesc":
                        sortedEmployees = employees.OrderByDescending(e => e.Location.LocationName)
                            .ThenBy(e => e.LastName).ThenBy(e => e.FirstName);
                        break;
                    default:
                        sortedEmployees = employees;
                        break;
                }

                return Ok(sortedEmployees);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Search/SearchEmployees: {ex}");
                return BadRequest("Failed to get employee search from Search api");
            }
        }
    }
}
