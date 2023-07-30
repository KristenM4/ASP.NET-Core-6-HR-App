using Microsoft.AspNetCore.Mvc;
using SeaWolf.HR.Models;

namespace SeaWolf.HR.Controllers.Api
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
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

        /// <summary>
        /// Find employees matching a keyword
        /// </summary>
        /// <param name="values">A keyword and sorter(LastName, NameDesc, Position, PositionDesc, Location, LocationDesc) separated by a '$$'. Example:
            /// <example>
            /// <code>
            /// "bob$$LastName"
            /// </code>
            /// </example>
        /// </param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Returns a list of matching employees</response>
        /// <response code="400">API has failed to complete the search</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
