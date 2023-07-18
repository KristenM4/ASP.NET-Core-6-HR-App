﻿using Microsoft.AspNetCore.Identity;

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
