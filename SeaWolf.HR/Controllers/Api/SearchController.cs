using Microsoft.AspNetCore.Mvc;
using SeaWolf.HR.Models;
using static System.Net.Mime.MediaTypeNames;

namespace SeaWolf.HR.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public SearchController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var allEmployees = _employeeRepository.AllEmployees;
            return Ok(allEmployees);
        }

        [HttpPost]
        public IActionResult SearchEmployees([FromBody] string values)
        {
            IEnumerable<Employee> employees = new List<Employee>();
            IEnumerable<Employee> sortedEmployees;

            string[] newValues = values.Split("$$");
            string searchQuery = newValues[0];
            string sorter = newValues[1];

            employees = _employeeRepository.SearchEmployees(searchQuery);

            switch(sorter)
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

            return new JsonResult(sortedEmployees);
        }
    }
}
