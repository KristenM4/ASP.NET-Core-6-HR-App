namespace SeaWolf.HR.Models
{
    public interface ILocationRepository
    {
        IEnumerable<Location> AllLocations { get; }
        Location? GetLocationById(int locationId);
        IEnumerable<Location> SearchLocations(string searchQuery);
    }
}
