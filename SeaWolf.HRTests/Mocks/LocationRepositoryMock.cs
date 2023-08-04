using Microsoft.AspNetCore.Mvc;
using Moq;
using SeaWolf.HR.Models;
using System.Numerics;

namespace SeaWolf.HRTests.Mocks
{
    public class LocationRepositoryMock
    {
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
            var newLocation = new Location()
            {
                LocationName = "Test Location",
                Phone = "12343412341",
                AddressLine1 = "123 Test Street",
                City = "Testville",
                State = "Testxas",
                PostalCode = "54321",
                Country = "United States of Tests"
            };

            var mockLocationRepository = new Mock<ILocationRepository>();
            mockLocationRepository.Setup(repo => repo.AllLocations).Returns(locations);
            // invalid id
            mockLocationRepository.Setup(repo => repo.GetLocationById(99, false))
                .Returns(locations.FirstOrDefault(e => e.LocationId == 99));
            // valid id
            mockLocationRepository.Setup(repo => repo.GetLocationById(1, false)).Returns(locations[0]);
            mockLocationRepository.Setup(repo => repo.GetLocationByName(It.IsAny<string>(), false))
                .Returns(locations[0]);
            mockLocationRepository.Setup(repo => repo.SearchLocations("Waianae"))
                .Returns(locations.Where(e => e.City == "Waianae"));
            mockLocationRepository.Setup(repo => repo.AddLocation(newLocation));
            mockLocationRepository.Setup(repo => repo.Save()).Returns(true);
            return mockLocationRepository;
        }
    }
}
