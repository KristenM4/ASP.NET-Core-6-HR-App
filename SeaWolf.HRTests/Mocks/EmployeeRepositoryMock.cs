using Moq;
using SeaWolf.HR.Models;
using SeaWolf.HRTests.Mocks;

namespace SeaWolf.HR.Mocks
{
    public class EmployeeRepositoryMock
    {
        public static Mock<IEmployeeRepository> GetEmployeeRepository()
        {
            var location = LocationRepositoryMock.GetLocationRepository().Object.GetLocationById(1);
            var employees = new List<Employee>
            {
                new Employee() {FirstName="Bob", LastName="Evans",
                    DateOfBirth=new DateTime(1948, 05, 30), Email="bevans@email.com", Phone="1234567890",
                Position="Manager", Location=location},
                new Employee() {FirstName="Harland", LastName="Sanders", MiddleName="David",
                    DateOfBirth=new DateTime(1952, 09, 09), Email="hsanders@email.com", Phone="1234567891",
                Position="Salesperson", Location=location},
                new Employee() {FirstName="Ronald", LastName="McDonald",
                    DateOfBirth=new DateTime(1965, 11, 25), Email="rmcdonald@email.com", Phone="1234567892",
                Position="Salesperson", Location=location},
                new Employee() {FirstName="Wendy", LastName="Thomas", MiddleName="Lou",
                    DateOfBirth=new DateTime(1961, 09, 14), Email="wthomas@email.com", Phone="1234567893",
                Position="Cashier", Location=location},
                new Employee() {FirstName="Forrest", LastName="Raffel", MiddleName="Leroy",
                    DateOfBirth=new DateTime(1964, 07, 23), Email="fraffel@email.com", Phone="1234567894",
                Position="Cleaner", Location=location}
            };
            var newEmployee = new Employee()
            {
                FirstName = "New",
                LastName = "Employee",
                DateOfBirth = new DateTime(1999, 01, 01),
                Email = "nemployee@email.com",
                Phone = "1234567895",
                Position = "Tester",
                Location=location
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
            mockEmployeeRepository.Setup(repo => repo.AddEmployee(newEmployee));
            mockEmployeeRepository.Setup(repo => repo.Save()).Returns(true);
            return mockEmployeeRepository;
        }
    }
}
