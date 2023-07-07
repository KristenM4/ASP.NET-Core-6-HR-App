using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SeaWolf.HR.Controllers;
using SeaWolf.HR.Models;
using SeaWolf.HR.ViewModels;
using SeaWolf.HRTests.Mocks;

namespace SeaWolf.HRTests.Controllers
{
    public class AppControllerTests
    {
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
        private readonly Mock<ILocationRepository> _mockLocationRepository;
        private readonly AppController _appController;

        public AppControllerTests()
        {
            // arrange
            _mockEmployeeRepository = RepositoryMocks.GetEmployeeRepository();
            _mockLocationRepository = RepositoryMocks.GetLocationRepository();
            _appController = new AppController(_mockEmployeeRepository.Object,
                _mockLocationRepository.Object, Mock.Of<ILogger<AppController>>());
        }

        [Fact]
        public void Index_Returns_Home_Page()
        {
            // act
            var result = _appController.Index();

            // assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void EmployeeList_Returns_Employees_List()
        {
            // act
            var result = _appController.EmployeeList();

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var employeeListViewModel = Assert.IsAssignableFrom<EmployeeListViewModel>
                (viewResult.ViewData.Model);
            Assert.Equal(5, employeeListViewModel.Employees.Count());
        }

        [Fact]
        public void EmployeeDetails_Returns_Employee_Details_Page()
        {
            // act
            var result = _appController.EmployeeDetails(1);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var employeeModel = Assert.IsAssignableFrom<Employee>(viewResult.ViewData.Model);
            Assert.Equal("Bob", employeeModel.FirstName);
        }

        [Fact]
        public void EmployeeDetails_Returns_NotFound_For_Invalid_Id()
        {
            // act
            var result = _appController.EmployeeDetails(99);

            // assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void LocationList_Returns_List_Of_Locations()
        {
            // act
            var result = _appController.LocationList();

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var locationListViewModel = Assert.IsAssignableFrom<LocationListViewModel>
                (viewResult.ViewData.Model);
            Assert.Equal(2, locationListViewModel.Locations.Count());
        }

        [Fact]
        public void LocationDetails_Returns_Location_Details_Page()
        {
            // act
            var result = _appController.LocationDetails(1);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var locationModel = Assert.IsAssignableFrom<Location>(viewResult.ViewData.Model);
            Assert.Equal("Farrington Store", locationModel.LocationName);
        }

        [Fact]
        public void LocationDetails_Returns_NotFound_For_Invalid_Id()
        {
            // act
            var result = _appController.LocationDetails(99);

            // assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }
    }
}
