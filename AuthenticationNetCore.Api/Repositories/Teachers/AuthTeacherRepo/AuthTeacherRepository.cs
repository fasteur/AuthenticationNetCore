using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;

namespace AuthenticationNetCore.Api.Repositories.Teachers.AuthTeacherRepo
{
    public class AuthTeacherRepository : GenericRepository<Teacher>, IAuthTeacherRepository
    {
        public AuthTeacherRepository(DataContext context) : base(context)
        {}

        public async Task<bool> AnyAsync(Expression<Func<Teacher, bool>> predicate)
        {
            return await context.Teachers
                .AnyAsync(predicate);
        }

        public async Task<Teacher> FindOneAsync(Expression<Func<Teacher, bool>> predicate)
        {
            return await context.Teachers
                .FirstOrDefaultAsync(predicate);
        }
    }
}