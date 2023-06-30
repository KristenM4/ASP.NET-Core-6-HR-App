namespace SeaWolf.HR.Models
{
    public class DbSeeder
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            SeaWolfHRDbContext context =
                applicationBuilder.ApplicationServices.CreateScope
                ().ServiceProvider.GetRequiredService<SeaWolfHRDbContext>();

            if(!context.Locations.Any())
            {
                context.Add(
                    new Location()
                    {
                        LocationName = "Farrington Store",
                        Phone = "8085551234",
                        AddressLine1 = "84-521 Farrington Highway",
                        City = "Waianae",
                        State = "Hawaii",
                        PostalCode = "96792",
                        Country = "US"
                    });
                context.SaveChanges();
            }

            if (!context.Employees.Any())
            {
                var farringtonStore = context.Locations.FirstOrDefault(l => l.LocationName == "Farrington Store");

                context.AddRange(
                new Employee()
                {
                    FirstName = "Bob",
                    LastName = "Evans",
                    DateOfBirth = new DateTime(1948, 05, 30),
                    Email = "bevans@email.com",
                    Phone = "1234567890",
                    Position = "Manager",
                    Location = farringtonStore
                },

                new Employee()
                {
                    FirstName = "Harland",
                    LastName = "Sanders",
                    MiddleName = "David",
                    DateOfBirth = new DateTime(1952, 09, 09),
                    Email = "hsanders@email.com",
                    Phone = "1234567891",
                    Position = "Salesperson",
                    Location = farringtonStore
                },

                new Employee()
                {
                    FirstName = "Ronald",
                    LastName = "McDonald",
                    DateOfBirth = new DateTime(1965, 11, 25),
                    Email = "rmcdonald@email.com",
                    Phone = "1234567892",
                    Position = "Salesperson",
                    Location = farringtonStore
                },

                new Employee()
                {
                    FirstName = "Wendy",
                    LastName = "Thomas",
                    MiddleName = "Lou",
                    DateOfBirth = new DateTime(1961, 09, 14),
                    Email = "wthomas@email.com",
                    Phone = "1234567893",
                    Position = "Cashier",
                    Location = farringtonStore
                },

                new Employee()
                {
                    FirstName = "Forrest",
                    LastName = "Raffel",
                    MiddleName = "Leroy",
                    DateOfBirth = new DateTime(1964, 07, 23),
                    Email = "fraffel@email.com",
                    Phone = "1234567894",
                    Position = "Cleaner",
                    Location = farringtonStore
                });
            }

            context.SaveChanges();
        }
    }
}
