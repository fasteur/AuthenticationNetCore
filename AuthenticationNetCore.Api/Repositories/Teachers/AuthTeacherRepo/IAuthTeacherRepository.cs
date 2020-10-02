using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;

namespace AuthenticationNetCore.Api.Repositories.Teachers.AuthTeacherRepo
{
    public interface IAuthTeacherRepository : IRepository<Teacher>
    {
        Task<bool> AnyAsync(Expression<Func<Teacher, bool>> predicate);
        Task<Teacher> FindOneAsync(Expression<Func<Teacher, bool>> predicate);
    }
}