using AutoMapper;
using SeaWolf.HR.Models;
using SeaWolf.HR.ViewModels;

namespace SeaWolf.HR.Profiles
{
    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            CreateMap<AddLocationViewModel, Location>();
            CreateMap<Location, GetAllLocationsViewModel>();
            CreateMap<UpdateLocationViewModel, Location>();
        }
    }
}
