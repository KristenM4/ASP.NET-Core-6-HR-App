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
    }
}
