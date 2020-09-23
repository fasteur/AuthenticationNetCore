using AuthenticationNetCore.Api.Data;
using AuthenticationNetCore.Api.Models.UserDto;
using AutoMapper;

namespace AuthenticationNetCore.Api
{
    public class AutoMapperProfile : Profile
    {
        
        public AutoMapperProfile()
        {
            CreateMap<User, UserProfileDto>();
            CreateMap<UserRegisterDto, User>();
        }
    }
}