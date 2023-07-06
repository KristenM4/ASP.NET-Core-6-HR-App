using Microsoft.AspNetCore.Mvc;
using Moq;
using SeaWolf.HR.Models;
using SeaWolf.HRTests.Mocks;

namespace SeaWolf.HR.Controllers
{
    public class SearchControllerTests
    {
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;

        public SearchControllerTests()
        {
            _mockEmployeeRepository = RepositoryMocks.GetEmployeeRepository();
        }
    }
}
