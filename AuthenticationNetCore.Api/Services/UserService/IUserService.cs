using System;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Models;
using AuthenticationNetCore.Api.Models.UserDto;

namespace AuthenticationNetCore.Api.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<UserProfileDto>> GetProfileById (Guid id);
    }
}