using System;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using AuthenticationNetCore.Api.Models;

namespace AuthenticationNetCore.Api.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<Guid>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<ServiceResWithoutData> ConfirmEmail(Guid id);
        Task<bool> UserExists(User user);
    }
}