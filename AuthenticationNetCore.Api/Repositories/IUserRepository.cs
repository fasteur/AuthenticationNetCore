
using System;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;

namespace AuthenticationNetCore.Api.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserAsync(Guid id);
        Task RemoveProfile(Guid id);
    }
}