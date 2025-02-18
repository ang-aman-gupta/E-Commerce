using AutoMapper;
using UserService.DTO;
using UserService.Model;

namespace UserService.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        { 
            CreateMap<UpdateProfileDTO, User>().ReverseMap();
            CreateMap<UserGetDTO, User>().ReverseMap();
            CreateMap<UserCreateDTO, User>().ReverseMap();
            CreateMap<UserAddressDto,UserAddress>().ReverseMap();
        
        }
    }
}
