namespace SeaWolf.HR.Models
{
    public interface ILocationRepository
    {
        IEnumerable<Location> AllLocations { get; }
        Location? GetLocationById(int locationId, bool includeEmployees = false);
        Location? GetLocationByName(string name, bool includeEmployees = false);
        IEnumerable<Location> SearchLocations(string searchQuery);
        void AddLocation(Location location);
        void DeleteLocation(int locationId);
        bool Save();
    }
}
