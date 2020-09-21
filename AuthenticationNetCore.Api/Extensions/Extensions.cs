using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Linq;
namespace AuthenticationNetCore.Api
{
    public static class Extensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}