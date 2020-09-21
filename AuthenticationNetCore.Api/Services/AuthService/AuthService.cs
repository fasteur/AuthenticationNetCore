using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using AuthenticationNetCore.Api.Models;
using AuthenticationNetCore.Api.Models.UserDto;
using AuthenticationNetCore.Api.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationNetCore.Api.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private IAuthRepository _authRepo;
        private readonly IConfiguration _config;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepo = authRepository;
            _config = configuration;
        }
        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _authRepo.FindOneAsync(u => u.UserName.ToLower().Equals(username.ToLower()));
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false; 
                response.Message = "Wrong password.";
            }
            else 
            {
                response.Data = CreateToken(user);
            }
            return response;
        }

        public async Task<ServiceResponse<Guid>> Register(User user, string password)
        {
            ServiceResponse<Guid> response = new ServiceResponse<Guid>();
            if(await UserExists(user))
            {
                response.Success = false;
                response.Message = "UserName or Email already exists.";
                return response;
            }
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _authRepo.AddAsync(user);
            await _authRepo.SaveChangesAsync();
            response.Data = user.Id;

            return response;
        }

        public async Task<bool> UserExists(User user)
        {
            return (await _authRepo.AnyAsync(u => u.UserName.ToLower() == user.UserName || u.Email == user.Email)) ? true : false;
        }

        // PRIVATE METHODS

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var ComputeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                {
                    for (int i = 0; i < ComputeHash.Length; i++)
                    {
                        if (ComputeHash[i] != passwordHash[i])
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Auth:Token"]));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(15),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler  tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}