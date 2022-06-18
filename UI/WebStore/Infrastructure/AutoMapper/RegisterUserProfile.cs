using AutoMapper;
using WebStore.Domain.Entities.Identity;
using WebStore.ViewModels;

namespace WebStore.Infrastructure.AutoMapper
{
    public class RegisterUserProfile : Profile
    {
        public RegisterUserProfile()
        {
            CreateMap<RegisterUserViewModel, User>()
                .ForMember(user => user.UserName, opt => opt.MapFrom(model => model.UserName))
                .ForMember(user => user.FirstName, opt => opt.MapFrom(model => model.FirstName))
                .ForMember(user => user.LastName, opt => opt.MapFrom(model => model.LastName))
                .ForMember(user => user.Email, opt => opt.MapFrom(model => model.Email))
                .ForMember(user => user.PhoneNumber, opt => opt.MapFrom(model => model.Phone))
                .ForMember(user => user.Birthday, opt => opt.MapFrom(model => model.Birthday))
                .ForMember(user => user.AboutMySelf, opt => opt.MapFrom(model => model.AboutMySelf));
        }
    }
}
