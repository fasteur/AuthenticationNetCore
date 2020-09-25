using System;
using AuthenticationNetCore.Api.Data;
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
            var path = $"http://localhost:5000/User/Confirm?userid={user.Id}&code={token}";
            return new Uri($"{path}").ToString();
        }
    }
}