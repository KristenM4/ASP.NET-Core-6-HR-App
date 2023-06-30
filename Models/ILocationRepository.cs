namespace SeaWolf.HR.Models
{
    public interface ILocationRepository
    {
        IEnumerable<Location> AllLocations { get; }
    }
}
