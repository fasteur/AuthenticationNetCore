using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using AuthenticationNetCore.Api.Models.UserDto;

namespace AuthenticationNetCore.Api.Repositories
{
    public interface IAuthRepository : IRepository<User>
    {
        Task<bool> AnyAsync(Expression<Func<User, bool>> predicate);
        Task<User> FindOneAsync(Expression<Func<User, bool>> predicate);
    }
}