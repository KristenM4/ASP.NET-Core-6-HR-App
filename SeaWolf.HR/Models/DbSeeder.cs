using Microsoft.AspNetCore.Identity;

namespace SeaWolf.HR.Models
{
    public class DbSeeder
    {
        public static async Task SeedAsync(IApplicationBuilder applicationBuilder)
        {
            SeaWolfHRDbContext context =
                applicationBuilder.ApplicationServices.CreateScope
                ().ServiceProvider.GetRequiredService<SeaWolfHRDbContext>();

            UserManager<HRUser> userManager =
                applicationBuilder.ApplicationServices.CreateScope
                ().ServiceProvider.GetRequiredService<UserManager<HRUser>>();

            HRUser user = await userManager.FindByEmailAsync("HR@example.com");
            if (user == null)
            {
                user = new HRUser()
                {
                    FirstName = "HR",
                    LastName = "Department",
                    Email = "HR@example.com",
                    UserName = "HR@example.com"
                };
                var result = await userManager.CreateAsync(user, "Testp@ss123");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("User creation unsuccessful in seeder");
                }
            }

            if(!context.Locations.Any())
            {
                context.AddRange(
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
                    },
                    new Location()
                    {
                        LocationName = "SeaWolf HQ",
                        Phone = "1800555152",
                        AddressLine1 = "4132 Mookaula St",
                        City = "Honolulu",
                        State = "Hawaii",
                        PostalCode = "96817",
                        Country = "US"
                    },
                    new Location()
                    {
                        LocationName = "Honolulu Store",
                        Phone = "9025555678",
                        AddressLine1 = "90-3 Kahala Ave",
                        City = "Honolulu",
                        State = "Hawaii",
                        PostalCode = "96816",
                        Country = "US"
                    },
                    new Location()
                    {
                        LocationName = "Haleiwa Store",
                        Phone = "8085550099",
                        AddressLine1 = "63-10 Kamehameha Hwy",
                        City = "Haleiwa",
                        State = "Hawaii",
                        PostalCode = "96712",
                        Country = "US"
                    });
                context.SaveChanges();
            }

            if (!context.Employees.Any())
            {
                var farringtonStore = context.Locations.FirstOrDefault(l => l.LocationName == "Farrington Store");
                var seawolfwarehouse = context.Locations.FirstOrDefault(l => l.LocationName == "SeaWolf Warehouse");
                var seawolfHQ = context.Locations.FirstOrDefault(l => l.LocationName == "SeaWolf HQ");

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
                },
                new Employee()
                {
                    FirstName = "Deborah",
                    LastName = "Jones",
                    DateOfBirth = new DateTime(1984, 06, 01),
                    Email = "djones@email.com",
                    Phone = "1234567895",
                    Position = "Head Supervisor",
                    Location = seawolfwarehouse
                },
                new Employee()
                {
                    FirstName = "Juan",
                    LastName = "Lopez",
                    MiddleName = "Roberto",
                    DateOfBirth = new DateTime(1989, 01, 02),
                    Email = "jlopez@email.com",
                    Phone = "1234567896",
                    Position = "Assistant Supervisor",
                    Location = seawolfwarehouse
                },
                new Employee()
                {
                    FirstName = "Steven",
                    LastName = "Smith",
                    DateOfBirth = new DateTime(1991, 12, 12),
                    Email = "ssmith@email.com",
                    Phone = "1234567897",
                    Position = "Delivery Driver",
                    Location = seawolfwarehouse
                },
                new Employee()
                {
                    FirstName = "Patricia",
                    LastName = "Parker",
                    MiddleName = "Ann",
                    DateOfBirth = new DateTime(1995, 08, 17),
                    Email = "pparker@email.com",
                    Phone = "1234567898",
                    Position = "Delivery Driver",
                    Location = seawolfwarehouse
                },
                new Employee()
                {
                    FirstName = "Felicia",
                    LastName = "Patel",
                    DateOfBirth = new DateTime(1988, 05, 29),
                    Email = "fpatel@email.com",
                    Phone = "1234567899",
                    Position = "Warehouse Operative",
                    Location = seawolfwarehouse
                },
                new Employee()
                {
                    FirstName = "Leonardo",
                    MiddleName = "Lorenzo",
                    LastName = "Da Vinci",
                    DateOfBirth = new DateTime(1997, 12, 14),
                    Email = "ldavinci@email.com",
                    Phone = "1234567810",
                    Position = "Warehouse Operative",
                    Location = seawolfwarehouse
                },
                new Employee()
                {
                    FirstName = "Charles",
                    LastName = "Lee",
                    DateOfBirth = new DateTime(1989, 09, 09),
                    Email = "clee@email.com",
                    Phone = "1234567811",
                    Position = "Warehouse Operative",
                    Location = seawolfwarehouse
                },
                new Employee()
                {
                    FirstName = "Sara",
                    MiddleName = "Lee",
                    LastName = "Jones",
                    DateOfBirth = new DateTime(1993, 01, 21),
                    Email = "sjones@email.com",
                    Phone = "1234567812",
                    Position = "Warehouse Operative",
                    Location = seawolfwarehouse
                },
                new Employee()
                {
                    FirstName = "Philippa",
                    LastName = "Colwyn",
                    DateOfBirth = new DateTime(1957, 05, 20),
                    Email = "pcolwyn@email.com",
                    Phone = "1234567813",
                    Position = "Chief Executive Officer",
                    Location = seawolfHQ
                },
                new Employee()
                {
                    FirstName = "Jackson",
                    MiddleName = "Roberts",
                    LastName = "Nicolao",
                    DateOfBirth = new DateTime(1961, 08, 02),
                    Email = "jnicolao@email.com",
                    Phone = "1234567814",
                    Position = "Chief Financial Officer",
                    Location = seawolfHQ
                },
                new Employee()
                {
                    FirstName = "Hugo",
                    LastName = "Vlado",
                    DateOfBirth = new DateTime(1979, 03, 27),
                    Email = "hvlado@email.com",
                    Phone = "1234567815",
                    Position = "Chief Operating Officer",
                    Location = seawolfHQ
                },
                new Employee()
                {
                    FirstName = "Evelyn",
                    LastName = "Bud",
                    DateOfBirth = new DateTime(1974, 10, 25),
                    Email = "ebud@email.com",
                    Phone = "1234567816",
                    Position = "Chief Marketing Officer",
                    Location = seawolfHQ
                }
                );
            }

            context.SaveChanges();
        }
    }
}
