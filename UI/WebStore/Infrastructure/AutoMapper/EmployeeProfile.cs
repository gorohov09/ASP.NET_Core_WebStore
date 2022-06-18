using AutoMapper;
using WebStore.Domain.Entities;
using WebStore.ViewModels;

namespace WebStore.Infrastructure.Automapper
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeViewModel>()
                .ReverseMap();
        }
    }
}
