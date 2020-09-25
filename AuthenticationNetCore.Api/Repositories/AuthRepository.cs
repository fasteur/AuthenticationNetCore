using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;

namespace AuthenticationNetCore.Api.Repositories
{
    public class AuthRepository : GenericRepository<User>, IAuthRepository
    {
        public AuthRepository(DataContext context) : base(context)
        {}

        public async Task<bool> AnyAsync(Expression<Func<User, bool>> predicate)
        {
            return await context.Users
                .AnyAsync(predicate);
        }

        public async Task<User> FindOneAsync(Expression<Func<User, bool>> predicate)
        {
            return await context.Users
                .FirstOrDefaultAsync(predicate);
        }
    }
}