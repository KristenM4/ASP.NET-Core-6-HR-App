using Microsoft.AspNetCore.Mvc;
using Moq;
using SeaWolf.HR.Models;

namespace SeaWolf.HRTests.Mocks
{
    public class RepositoryMocks
    {
        public static Mock<IEmployeeRepository> GetEmployeeRepository()
        {
            var employees = new List<Employee>
            {
                new Employee() {FirstName="Bob", LastName="Evans",
                    DateOfBirth=new DateTime(1948, 05, 30), Email="bevans@email.com", Phone="1234567890",
                Position="Manager" },
                new Employee() {FirstName="Harland", LastName="Sanders", MiddleName="David",
                    DateOfBirth=new DateTime(1952, 09, 09), Email="hsanders@email.com", Phone="1234567891",
                Position="Salesperson"},
                new Employee() {FirstName="Ronald", LastName="McDonald",
                    DateOfBirth=new DateTime(1965, 11, 25), Email="rmcdonald@email.com", Phone="1234567892",
                Position="Salesperson"},
                new Employee() {FirstName="Wendy", LastName="Thomas", MiddleName="Lou",
                    DateOfBirth=new DateTime(1961, 09, 14), Email="wthomas@email.com", Phone="1234567893",
                Position="Cashier"},
                new Employee() {FirstName="Forrest", LastName="Raffel", MiddleName="Leroy",
                    DateOfBirth=new DateTime(1964, 07, 23), Email="fraffel@email.com", Phone="1234567894",
                Position="Cleaner"}
            };

            var mockEmployeeRepository = new Mock<IEmployeeRepository>();
            mockEmployeeRepository.Setup(repo => repo.AllEmployees).Returns(employees);
            // invalid id
            mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(99))
                .Returns(employees.FirstOrDefault(e => e.EmployeeId == 99));
            // valid id
            mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(1)).Returns(employees[0]);
            mockEmployeeRepository.Setup(repo => repo.SearchEmployees("Salesperson"))
                .Returns(employees.Where(e => e.Position == "Salesperson"));
            return mockEmployeeRepository;
        }

        public static Mock<ILocationRepository> GetLocationRepository()
        {
            var locations = new List<Location>()
            {
                new Location()
                    {
                        LocationName = "Farrington Store",
                        Phone = "8085551234",
                        AddressLine1 = "84-521 Farrington Highway",
                        City = "Waianae",
                        State = "Hawaii",
                        PostalCode = "96792",
                        Country = "US"
                    },
                new Location()
                    {
                        LocationName = "SeaWolf Warehouse",
                        Phone = "8085555678",
                        AddressLine1 = "72 Seashell Road",
                        City = "Waipahu",
                        State = "Hawaii",
                        PostalCode = "96797",
                        Country = "US"
                    }
            };

            var mockLocationRepository = new Mock<ILocationRepository>();
            mockLocationRepository.Setup(repo => repo.AllLocations).Returns(locations);
            // invalid id
            mockLocationRepository.Setup(repo => repo.GetLocationById(99))
                .Returns(locations.FirstOrDefault(e => e.LocationId == 99));
            // valid id
            mockLocationRepository.Setup(repo => repo.GetLocationById(1)).Returns(locations[0]);

            return mockLocationRepository;
        }
    }
}
