using Microsoft.AspNetCore.Mvc;
using SeaWolf.HR.Controllers;
using SeaWolf.HR.ViewModels;
using SeaWolf.HRTests.Mocks;

namespace SeaWolf.HRTests.Controllers
{
    public class AppControllerTests
    {
        [Fact]
        public void Index_Returns_Employees_List()
        {
            //setup
            var mockEmployeeRepository = RepositoryMocks.GetEmployeeRepository();
            var mockLocationRepository = RepositoryMocks.GetLocationRepository();

            var appController = new AppController(mockEmployeeRepository.Object,
                mockLocationRepository.Object);

            //act
            var result = appController.Index();

            //assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var employeeListViewModel = Assert.IsAssignableFrom<EmployeeListViewModel>
                (viewResult.ViewData.Model);
            Assert.Equal(5, employeeListViewModel.Employees.Count());
        }
    }
}
