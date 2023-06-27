namespace SeaWolf.HR.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        public IEnumerable<Employee> AllEmployees =>
            new List<Employee>
            {
                new Employee() {FirstName="Bob", LastName="Evans",
                    DateOfBirth=new DateTime(1948, 05, 30), Email="bevans@email.com", Phone="1234567890",
                Position="Manager"},
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

        public Employee? GetEmployeeById(int employeeId)
        {
            return AllEmployees.FirstOrDefault(e => e.EmployeeId == employeeId);
        }
    }
}
