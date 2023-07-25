using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SeaWolf.HR.Controllers.Api;
using SeaWolf.HR.Models;
using SeaWolf.HRTests.Mocks;

namespace SeaWolf.HR.Controllers
{
    public class LocationControllerTests
    {
        private readonly Mock<ILocationRepository> _mockLocationRepository;
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
        private readonly LocationController _controller;

        public LocationControllerTests()
        {
            _mockLocationRepository = RepositoryMocks.GetLocationRepository();
            _mockEmployeeRepository = RepositoryMocks.GetEmployeeRepository();
            _controller = new LocationController(_mockLocationRepository.Object, _mockEmployeeRepository.Object, Mock.Of<ILogger<LocationController>>());
        }

        [Fact]
        public void GetAllLocations_Returns_All_Locations()
        {
            var actionResult = _controller.GetAllLocations();
            var result = actionResult as OkObjectResult;
            var value = result.Value as IEnumerable<Location>;

            Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(2, value.Count());
        }

        [Fact]
        public void GetLocationDetails_Returns_Location()
        {
            var actionResult = _controller.GetLocationDetails(1);
            var result = actionResult as OkObjectResult;
            var value = result.Value as Location;

            Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal("Farrington Store", value.LocationName);
        }
    }
}
