using System;
using System.Security.Claims;
using AuthenticationNetCore.Api.Data;

namespace AuthenticationNetCore.Api
{
    public static class Extensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
        public static Guid GetConfirmCode(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirst(ClaimTypes.AuthenticationMethod).Value);
        }
    }
}