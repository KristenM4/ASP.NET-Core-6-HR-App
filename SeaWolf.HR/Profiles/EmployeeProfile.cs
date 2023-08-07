using AutoMapper;
using SeaWolf.HR.Models;
using SeaWolf.HR.ViewModels;

namespace SeaWolf.HR.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<AddEmployeeViewModel, Employee>()
                .ForMember(dest => dest.Location, act => act.Ignore());
            CreateMap<UpdateEmployeeViewModel, Employee>()
                .ForMember(dest => dest.Location, act => act.Ignore());
            CreateMap<Employee, UpdateEmployeeViewModel>()
                .ForMember(dest => dest.Location, act => act.Ignore());
        }
    }
}
