using SeaWolf.HR.Models;

namespace SeaWolf.HR.ViewModels
{
    public class LocationListViewModel
    {
        public IEnumerable<Location> Locations { get; set; }
        public LocationListViewModel(IEnumerable<Location> locations)
        {
            Locations = locations;
        }
    }
}
