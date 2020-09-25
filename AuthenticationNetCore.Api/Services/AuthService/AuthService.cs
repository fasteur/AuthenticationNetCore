using System;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using AuthenticationNetCore.Api.Models;
using AuthenticationNetCore.Api.Repositories;
using AuthenticationNetCore.Api.Utilities.EmailTools;
using AuthenticationNetCore.Api.Utilities.HashFunctionTools;
using AuthenticationNetCore.Api.Utilities.TokenTools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace AuthenticationNetCore.Api.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private IAuthRepository _authRepo;
        private readonly IConfiguration _config;
        private IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContext;
        public AuthService(
            IAuthRepository authRepository,
            IConfiguration configuration,
            IEmailSender emailSender,
            IHttpContextAccessor httpContextAccessor)
        {
            _authRepo = authRepository;
            _config = configuration;
            _emailSender = emailSender;
            _httpContext = httpContextAccessor;
        }
        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _authRepo.FindOneAsync(u => u.UserName.ToLower().Equals(username.ToLower()) && u.EmaiIsValid);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else if (!HashUtilities.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password.";
            }
            else
            {
                response.Data = TokenUtilities.CreateToken(_config, user);
            }
            return response;
        }

        public async Task<ServiceResponse<Guid>> Register(User user, string password)
        {
            ServiceResponse<Guid> response = new ServiceResponse<Guid>();
            if (await UserExists(user))
            {
                response.Success = false;
                response.Message = "UserName or Email already exists.";
                return response;
            };
            HashUtilities.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var emailCode = Guid.NewGuid();
            HashUtilities.CreatePasswordHash(emailCode.ToString(), out byte[] emailCodeHash, out byte[] emailCodeSalt);
            user.EmailCodeHash = emailCodeHash;
            user.EmailCodeSalt = emailCodeSalt;

            await _authRepo.AddAsync(user);
            await _authRepo.SaveChangesAsync();
            response.Data = user.Id;
            await _emailSender.SendEmailAsync("asteur.florian@gmail.com", "Confim email", EmailUtilities.EmailContentMessage(_config, user, emailCode));
            return response;
        }
        public async Task<bool> UserExists(User user)
        {
            return (await _authRepo.AnyAsync(u => u.UserName.ToLower() == user.UserName || u.Email == user.Email)) ? true : false;
        }

        public async Task<ServiceResWithoutData> ConfirmEmail(Guid id)
        {
            var res = new ServiceResWithoutData();
            try
            {
                var claimPrincipal = _httpContext.HttpContext.User;
                var claimUserId = claimPrincipal.GetUserId();
                var claimCode = claimPrincipal.GetConfirmCode().ToString();
                var user = await _authRepo.FindOneAsync(u => u.Id == id && claimUserId == id);
                if (user == null || !HashUtilities.VerifyPasswordHash(claimCode, user.EmailCodeHash, user.EmailCodeSalt))
                {
                    res.Success = false;
                    res.Message = "Email address is not confirmed!";
                }
                else if(user.EmaiIsValid)
                {
                    res.Success = false;
                    res.Message = "Email is aldready confirmed!";
                }
                else
                {
                    user.EmaiIsValid = true;
                    await _authRepo.SaveChangesAsync();
                    res.Success = true;
                    res.Message = "Email address is confirmed!";
                }
            }
            catch (Exception ex)
            {
                res.Success = false; 
                res.Message = ex.Message;
            }

            return res;
        }
    }
}