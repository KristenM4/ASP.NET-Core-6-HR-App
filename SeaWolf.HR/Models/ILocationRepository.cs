namespace SeaWolf.HR.Models
{
    public interface ILocationRepository
    {
        IEnumerable<Location> AllLocations { get; }
        Location? GetLocationById(int locationId);
        Location? GetLocationByName(string name);
        IEnumerable<Location> SearchLocations(string searchQuery);
        void AddLocation(Location location);
        bool Save();
    }
}
