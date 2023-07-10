using Microsoft.AspNetCore.Mvc;
using SeaWolf.HR.Models;

namespace SeaWolf.HR.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchEmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<SearchEmployeeController> _logger;

        public SearchEmployeeController(IEmployeeRepository employeeRepository, ILogger<SearchEmployeeController> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
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
