using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;

namespace AuthenticationNetCore.Api.Repositories.Students.AuthStudentRepo
{
    public interface IAuthStudentRepository : IRepository<Student>
    {
        Task<bool> AnyAsync(Expression<Func<Student, bool>> predicate);
        Task<Student> FindOneAsync(Expression<Func<Student, bool>> predicate);
    }
}