using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SeaWolf.HR.Controllers.Api;
using SeaWolf.HR.Models;
using SeaWolf.HRTests.Mocks;

namespace SeaWolf.HR.Controllers
{
    public class SearchControllerTests
    {
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
        private readonly SearchController _controller;

        public SearchControllerTests()
        {
            _mockEmployeeRepository = RepositoryMocks.GetEmployeeRepository();
            _controller = new SearchController(_mockEmployeeRepository.Object, Mock.Of<ILogger<SearchController>>());
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
