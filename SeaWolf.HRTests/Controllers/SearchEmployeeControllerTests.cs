using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SeaWolf.HR.Controllers.Api;
using SeaWolf.HR.Models;
using SeaWolf.HRTests.Mocks;

namespace SeaWolf.HR.Controllers
{
    public class SearchEmployeeControllerTests
    {
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
        private readonly SearchEmployeeController _controller;

        public SearchEmployeeControllerTests()
        {
            _mockEmployeeRepository = RepositoryMocks.GetEmployeeRepository();
            _controller = new SearchEmployeeController(_mockEmployeeRepository.Object, Mock.Of<ILogger<SearchEmployeeController>>());
        }

        [Fact]
        public void SearchEmployees_Returns_Specific_Employees()
        {
            var actionResult = _controller.SearchEmployees("Salesperson$$LastName");
            var result = actionResult as OkObjectResult;
            var value = result.Value as IEnumerable<Employee>;
            
            Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(2, value.Count());
        }
    }
}
