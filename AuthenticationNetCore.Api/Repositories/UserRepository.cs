using System;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace AuthenticationNetCore.Api.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly IHttpContextAccessor _httpContext;

        public UserRepository(DataContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _httpContext = httpContextAccessor;
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Id == id && id == _httpContext.HttpContext.User.GetUserId());
        }

        public async Task RemoveProfile(Guid id)
        {
            var entity = await context.Users.FindAsync(id);
            context.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}