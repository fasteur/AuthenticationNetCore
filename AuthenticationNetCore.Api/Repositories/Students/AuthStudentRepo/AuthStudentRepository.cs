using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;

namespace AuthenticationNetCore.Api.Repositories.Students.AuthStudentRepo
{
    public class AuthStudentRepository : GenericRepository<Student>, IAuthStudentRepository
    {
        public AuthStudentRepository(DataContext context) : base(context)
        {}

        public async Task<bool> AnyAsync(Expression<Func<Student, bool>> predicate)
        {
            return await context.Students
                .AnyAsync(predicate);
        }

        public async Task<Student> FindOneAsync(Expression<Func<Student, bool>> predicate)
        {
            return await context.Students
                .FirstOrDefaultAsync(predicate);
        }
    }
}