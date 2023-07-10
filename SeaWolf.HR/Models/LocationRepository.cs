using Microsoft.EntityFrameworkCore;

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

        public Location? GetLocationById(int locationId)
        {
            return _context.Locations.FirstOrDefault(l => l.LocationId == locationId);
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
