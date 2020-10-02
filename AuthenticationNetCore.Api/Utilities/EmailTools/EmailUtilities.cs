using System;
using AuthenticationNetCore.Api.Models.Role;
using AuthenticationNetCore.Api.Models.UserDto;
using AuthenticationNetCore.Api.Utilities.TokenTools;
using Microsoft.Extensions.Configuration;

namespace AuthenticationNetCore.Api.Utilities.EmailTools
{
    public static class EmailUtilities
    {
        public static string EmailContentMessage(IConfiguration config, User user, Guid emailCode)
        {
            return $"Hi {user.Name} {user.FirstName} please, confirm your email {GenerateEmailLink(config, user, emailCode)}";
        }

        public static string GenerateEmailLink(IConfiguration config, User user, Guid emailCode)
        {
            var token = TokenUtilities.CreateAccountConfirmToken(config, user, emailCode);
            var path = String.Empty;
            switch (user.Role)
            {   
                case Role.Teacher:
                    path = "AuthTeacher";
                    break;
                case Role.Student:
                    path = "AuthStudent";
                    break;
                case Role.Admin:
                    path = "AuthAdmin";
                    break;
                default:
                    break;
            }
            return new Uri($"http://localhost:5000/{path}/Confirm?userid={user.Id}&code={token}").ToString();
        }
    }
}