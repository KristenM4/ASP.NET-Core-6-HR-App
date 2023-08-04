using AutoMapper;
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

namespace SeaWolf.HR.Controllers
{
    public class LocationControllerTests
    {
        private readonly Mock<ILocationRepository> _mockLocationRepository;
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
        private readonly LocationController _controller;

        public LocationControllerTests()
        {
            _mockLocationRepository = LocationRepositoryMock.GetLocationRepository();
            _mockEmployeeRepository = EmployeeRepositoryMock.GetEmployeeRepository();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new LocationProfile());
            });
            var mapper = mockMapper.CreateMapper();

            _controller = new LocationController(_mockLocationRepository.Object,
                _mockEmployeeRepository.Object,
                Mock.Of<ILogger<LocationController>>(),
                mapper);
        }

        [Fact]
        public void GetAllLocations_Returns_All_Locations()
        {
            var actionResult = _controller.GetAllLocations();
            var result = actionResult as OkObjectResult;
            var value = result.Value as IEnumerable<GetAllLocationsViewModel>;

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

        [Fact]
        public void GetLocationDetails_Returns_NotFound_On_Invalid_Id()
        {
            var actionResult = _controller.GetLocationDetails(99);

            Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public void AddLocation_Returns_CreatedAtRouteResult()
        {
            var newLocation = new AddLocationViewModel()
            {
                LocationName = "Test Location",
                Phone = "12343412341",
                AddressLine1 = "123 Test Street",
                City = "Testville",
                State = "Testxas",
                PostalCode = "54321",
                Country = "United States of Tests"
            };

            var result = _controller.AddLocation(newLocation);

            var actionResult = Assert.IsAssignableFrom<ActionResult<Location>>(result);
            Assert.IsType<CreatedAtRouteResult>(actionResult.Result);
        }

        [Fact]
        public void UpdateLocation_Changes_Location_Details()
        {
            var updatedLocationInfo = new UpdateLocationViewModel()
            {
                LocationName = "Updated Location",
                Phone = "12343412341",
                AddressLine1 = "123 Test Street",
                City = "Testville",
                State = "Testxas",
                PostalCode = "54321",
                Country = "United States of Tests"
            };
            var result = _controller.UpdateLocation(1, updatedLocationInfo);
            var location = _mockLocationRepository.Object.GetLocationById(1);

            Assert.IsType<NoContentResult>(result);
            Assert.Equal("Updated Location", location.LocationName);
        }

        [Fact]
        public void PartiallyUpdateLocation_Changes_Some_Location_Details()
        {
            var jsonPatch = new JsonPatchDocument<UpdateLocationViewModel>();
            jsonPatch.Replace(l => l.LocationName, "patchName");
            var result = _controller.PartiallyUpdateLocation(1, jsonPatch);
            var location = _mockLocationRepository.Object.GetLocationById(1);

            Assert.IsType<NoContentResult>(result);
            Assert.Equal("patchName", location.LocationName);
        }

        [Fact]
        public void DeleteLocation_Returns_NoContent()
        {
            var result = _controller.DeleteLocation(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
