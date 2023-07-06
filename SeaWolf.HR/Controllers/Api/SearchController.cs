using Microsoft.AspNetCore.Mvc;
using SeaWolf.HR.Models;

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
        public IActionResult SearchEmployees([FromBody] string searchQuery)
        {
            IEnumerable<Employee> employees = new List<Employee>();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                employees = _employeeRepository.SearchEmployees(searchQuery);
            }
            else
            {
                employees = _employeeRepository.AllEmployees;
            }

            return new JsonResult(employees);
        }
    }
}
