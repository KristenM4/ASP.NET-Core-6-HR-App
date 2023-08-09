using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SeaWolf.HR.Controllers;
using SeaWolf.HR.Mocks;
using SeaWolf.HR.Models;
using SeaWolf.HR.Profiles;
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
            _mockEmployeeRepository = EmployeeRepositoryMock.GetEmployeeRepository();
            _mockLocationRepository = LocationRepositoryMock.GetLocationRepository();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new LocationProfile());
                cfg.AddProfile(new EmployeeProfile());
            });
            var mapper = mockMapper.CreateMapper();

            _appController = new AppController(_mockEmployeeRepository.Object,
                _mockLocationRepository.Object,
                Mock.Of<ILogger<AppController>>(),
                mapper);
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
                (viewResult.Model);
            Assert.Equal(5, employeeListViewModel.Employees.Count());
            Assert.Equal("Bob", employeeListViewModel.Employees.First().FirstName);
        }

        [Fact]
        public void EmployeeDetails_Returns_Employee_Details_Page()
        {
            // act
            var result = _appController.EmployeeDetails(1);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var employeeModel = Assert.IsAssignableFrom<Employee>(viewResult.Model);
            Assert.Equal("Bob", employeeModel.FirstName);
        }

        [Fact]
        public void EmployeeDetails_Returns_NotFound_For_Invalid_Id()
        {
            // act
            var result = _appController.EmployeeDetails(99);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void AddEmployee_Get_Uses_Returns_ViewResult()
        {
            var result = _appController.AddEmployee();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void AddEmployee_Post_Returns_View_On_Invalid_Model()
        {
            var newEmployeeViewModel = new AddEmployeeViewModel();
            _appController.ModelState.AddModelError("FirstName", "Required");

            var result = _appController.AddEmployee(newEmployeeViewModel);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.False(viewResult.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void AddEmployee_Post_Returns_Redirect_On_Valid_Model()
        {
            var newEmployeeViewModel = new AddEmployeeViewModel() {
                FirstName = "New",
                LastName = "Employee",
                DateOfBirth = new DateTime(1999, 01, 01),
                Email = "nemployee@email.com",
                Phone = "1234567895",
                Position = "Tester",
                Location = "Farrington Store"};

            var result = _appController.AddEmployee(newEmployeeViewModel);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("EmployeeDetails", redirectResult.ActionName);
        }

        [Fact]
        public void EditEmployee_Get_Uses_Returns_ViewResult()
        {
            var result = _appController.EditEmployee(1);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void EditEmployee_Post_Returns_View_On_Invalid_Model()
        {
            var updatedEmployeeViewModel = new UpdateEmployeeViewModel();
            _appController.ModelState.AddModelError("FirstName", "Required");

            var result = _appController.EditEmployee(1, updatedEmployeeViewModel);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.False(viewResult.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void EditEmployee_Post_Returns_Redirect_On_Valid_Model()
        {
            var updatedEmployeeViewModel = new UpdateEmployeeViewModel()
            {
                FirstName = "Bob",
                LastName = "Evans",
                DateOfBirth = new DateTime(1999, 01, 01),
                Email = "bevans@email.com",
                Phone = "1234567895",
                Position = "Tester",
                Location = "Farrington Store"
            };

            var result = _appController.EditEmployee(1, updatedEmployeeViewModel);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("EmployeeDetails", redirectResult.ActionName);
        }

        [Fact]
        public void LocationList_Returns_List_Of_Locations()
        {
            // act
            var result = _appController.LocationList();

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var locationListViewModel = Assert.IsAssignableFrom<LocationListViewModel>
                (viewResult.Model);
            Assert.Equal(2, locationListViewModel.Locations.Count());
            Assert.Equal("Farrington Store", locationListViewModel.Locations.First().LocationName);
        }

        [Fact]
        public void LocationDetails_Returns_Location_Details_Page()
        {
            // act
            var result = _appController.LocationDetails(1);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var locationModel = Assert.IsAssignableFrom<Location>(viewResult.Model);
            Assert.Equal("Farrington Store", locationModel.LocationName);
        }

        [Fact]
        public void LocationDetails_Returns_NotFound_For_Invalid_Id()
        {
            // act
            var result = _appController.LocationDetails(99);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void AddLocation_Get_Uses_Returns_ViewResult()
        {
            var result = _appController.AddLocation();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void AddLocation_Post_Returns_View_On_Invalid_Model()
        {
            var newLocationViewModel = new AddLocationViewModel();
            _appController.ModelState.AddModelError("PostalCode", "Required");

            var result = _appController.AddLocation(newLocationViewModel);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.False(viewResult.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void AddLocation_Post_Returns_Redirect_On_Valid_Model()
        {
            var newLocationViewModel = new AddLocationViewModel()
            {
                LocationName = "Test Location",
                Phone = "12343412341",
                AddressLine1 = "123 Test Street",
                City = "Testville",
                State = "Testxas",
                PostalCode = "54321",
                Country = "United States of Tests"
            };

            var result = _appController.AddLocation(newLocationViewModel);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("LocationDetails", redirectResult.ActionName);
        }
    }
}
