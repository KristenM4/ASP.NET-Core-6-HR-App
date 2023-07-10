using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SeaWolf.HR.Controllers.Api;
using SeaWolf.HR.Models;
using SeaWolf.HRTests.Mocks;

namespace SeaWolf.HR.Controllers
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
        private readonly EmployeeController _controller;

        public EmployeeControllerTests()
        {
            _mockEmployeeRepository = RepositoryMocks.GetEmployeeRepository();
            _controller = new EmployeeController(_mockEmployeeRepository.Object, Mock.Of<ILogger<EmployeeController>>());
        }
    }
}
