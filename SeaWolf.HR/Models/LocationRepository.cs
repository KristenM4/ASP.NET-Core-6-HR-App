﻿using Microsoft.EntityFrameworkCore;

namespace SeaWolf.HR.Models
{
    public class LocationRepository : ILocationRepository
    {
        private readonly SeaWolfHRDbContext _context;

        public LocationRepository(SeaWolfHRDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Location> AllLocations => _context.Locations.OrderBy(l => l.LocationName);

        public void AddLocation(Location location)
        {
            _context.Locations.Add(location);
        }

        public void DeleteLocation(int locationId)
        {
            var location = GetLocationById(locationId);

            if (location != null)
            {
                _context.Locations.Remove(location);
            }
        }

        public Location? GetLocationById(int locationId, bool includeEmployees = false)
        {
            if (includeEmployees)
            {
                return _context.Locations.Include(l => l.Employees).FirstOrDefault(l => l.LocationId == locationId);
            }

            return _context.Locations.FirstOrDefault(l => l.LocationId == locationId);
        }

        public Location? GetLocationByName(string name, bool includeEmployees = false)
        {
            if (includeEmployees)
            {
                return _context.Locations.Include(l => l.Employees).FirstOrDefault(l => l.LocationName == name);
            }

            return _context.Locations.FirstOrDefault(l => l.LocationName == name);
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<Location> SearchLocations(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return _context.Locations
                    .OrderBy(l => l.LocationName);
            }
            else
            {
                return _context.Locations.OrderBy(l => l.LocationName)
                    .Where(l =>
                    l.LocationName.Contains(searchQuery) || l.City.Contains(searchQuery) ||
                    l.Phone.Contains(searchQuery) || l.AddressLine1.Contains(searchQuery));
            }
        }
    }
}
