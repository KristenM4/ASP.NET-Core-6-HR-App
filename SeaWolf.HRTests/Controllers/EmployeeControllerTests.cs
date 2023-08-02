using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SeaWolf.HR.Controllers.Api;
using SeaWolf.HR.Models;
using SeaWolf.HR.ViewModels;
using SeaWolf.HRTests.Mocks;
using System.Security.Claims;
using System.Security.Principal;

namespace SeaWolf.HR.Controllers
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
        private readonly Mock<ILocationRepository> _mockLocationRepository;
        private readonly EmployeeController _controller;

        public EmployeeControllerTests()
        {
            _mockEmployeeRepository = RepositoryMocks.GetEmployeeRepository();
            _mockLocationRepository = RepositoryMocks.GetLocationRepository();
            
            _controller = new EmployeeController(_mockEmployeeRepository.Object, _mockLocationRepository.Object, Mock.Of<ILogger<EmployeeController>>());

            // create tester identity to pass identity checks in controller
            var identity = new GenericIdentity("tester");
            identity.AddClaim(new Claim("Name", "tester"));
            var principal = new ClaimsPrincipal(identity);
            var context = new DefaultHttpContext() { User = principal };
            _controller.ControllerContext = new ControllerContext { HttpContext = context };
        }

        [Fact]
        public void GetAllEmployees_Returns_All_Employees()
        {
            var actionResult = _controller.GetAllEmployees();
            var result = actionResult as OkObjectResult;
            var value = result.Value as IEnumerable<Employee>;

            Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(5, value.Count());
        }

        [Fact]
        public void GetEmployeeDetails_Returns_Employee()
        {
            var actionResult = _controller.GetEmployeeDetails(1);
            var result = actionResult as OkObjectResult;
            var value = result.Value as Employee;

            Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal("Bob", value.FirstName);
        }

        [Fact]
        public void GetEmployeeDetails_Returns_NotFound_On_Invalid_Id()
        {
            var actionResult = _controller.GetEmployeeDetails(99);

            Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public void AddEmployee_Returns_CreatedAtRouteResult()
        {
            var newEmployee = new AddEmployeeViewModel()
            {
                FirstName = "New",
                LastName = "Employee",
                DateOfBirth = new DateTime(1999, 01, 01),
                Email = "nemployee@email.com",
                Phone = "1234567895",
                Position = "Tester",
                Location = "Farrington Store"
            };
            var result = _controller.AddEmployee(newEmployee);

            var actionResult = Assert.IsAssignableFrom<ActionResult<Employee>>(result);
            Assert.IsType<CreatedAtRouteResult>(actionResult.Result);
        }
    }
}
