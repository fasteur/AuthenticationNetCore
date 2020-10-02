using System;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationNetCore.Api.Repositories.Students.StudentRepo
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        private readonly IHttpContextAccessor _httpContext;

        public StudentRepository(DataContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _httpContext = httpContextAccessor;
        }

        public async Task<Student> GetStudentAsync(Guid id)
        {
            return await context.Students.FirstOrDefaultAsync(u => u.Id == id && id == _httpContext.HttpContext.User.GetUserId());
        }

        public async Task RemoveProfile(Guid id)
        {
            var entity = await context.Teachers.FindAsync(id);
            context.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}