using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SeaWolf.HR.Controllers.Api;
using SeaWolf.HR.Mocks;
using SeaWolf.HR.Models;
using SeaWolf.HR.Profiles;
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
            _mockEmployeeRepository = EmployeeRepositoryMock.GetEmployeeRepository();
            _mockLocationRepository = LocationRepositoryMock.GetLocationRepository();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EmployeeProfile());
            });
            var mapper = mockMapper.CreateMapper();

            _controller = new EmployeeController(_mockEmployeeRepository.Object,
                _mockLocationRepository.Object,
                Mock.Of<ILogger<EmployeeController>>(),
                mapper);

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

        [Fact]
        public void UpdateEmployee_Changes_Employee_Details()
        {
            var updatedEmployeeInfo = new UpdateEmployeeViewModel()
            {
                FirstName = "New",
                LastName = "Employee",
                DateOfBirth = new DateTime(1999, 01, 01),
                Email = "nemployee@email.com",
                Phone = "1234567895",
                Position = "Tester",
                Location = "Farrington Store"
            };
            var result = _controller.UpdateEmployee(1, updatedEmployeeInfo);
            var employee = _mockEmployeeRepository.Object.GetEmployeeById(1);

            Assert.IsType<NoContentResult>(result);
            Assert.Equal("New", employee.FirstName);
        }

        [Fact]
        public void PartiallyUpdateEmployee_Changes_Some_Employee_Details()
        {
            var jsonPatch = new JsonPatchDocument<UpdateEmployeeViewModel>();
            jsonPatch.Replace(e => e.LastName, "patchName");
            var result = _controller.PartiallyUpdateEmployee(1, jsonPatch);
            var employee = _mockEmployeeRepository.Object.GetEmployeeById(1);

            Assert.IsType<NoContentResult>(result);
            Assert.Equal("patchName", employee.LastName);
        }

        [Fact]
        public void DeleteEmployee_Returns_NoContent()
        {
            var result = _controller.DeleteEmployee(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetAllEmployeesAsync_Returns_All_Employees()
        {
            var actionResult = await _controller.GetAllEmployeesAsync();

            var result = Assert.IsType<ActionResult<IEnumerable<Employee>>>(actionResult);
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var employees = Assert.IsAssignableFrom<IEnumerable<Employee>>(okObjectResult.Value);
            Assert.Equal(5, employees.Count());
        }
    }
}
