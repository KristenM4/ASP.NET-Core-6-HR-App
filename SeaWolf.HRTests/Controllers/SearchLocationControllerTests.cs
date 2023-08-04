using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SeaWolf.HR.Controllers.Api;
using SeaWolf.HR.Models;
using SeaWolf.HRTests.Mocks;

namespace SeaWolf.HR.Controllers
{
    public class SearchLocationControllerTests
    {
        private readonly Mock<ILocationRepository> _mockLocationRepository;
        private readonly SearchLocationController _controller;

        public SearchLocationControllerTests()
        {
            _mockLocationRepository = LocationRepositoryMock.GetLocationRepository();
            _controller = new SearchLocationController(_mockLocationRepository.Object, Mock.Of<ILogger<SearchLocationController>>());
        }

        [Fact]
        public void SearchLocations_Returns_Specific_Locations()
        {
            var actionResult = _controller.SearchLocations("Waianae$$LocationName");
            var result = actionResult as OkObjectResult;
            var value = result.Value as IEnumerable<Location>;

            Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(1, value.Count());
        }
    }
}
