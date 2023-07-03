using Microsoft.AspNetCore.Mvc;
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
                _mockLocationRepository.Object);
        }

        [Fact]
        public void Index_Returns_Employees_List()
        {
            // act
            var result = _appController.Index();

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
    }
}
