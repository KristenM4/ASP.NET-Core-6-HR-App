using AutoMapper;
using SeaWolf.HR.ViewModels;

namespace SeaWolf.HR.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, AddEmployeeViewModel>();
        }
    }
}
