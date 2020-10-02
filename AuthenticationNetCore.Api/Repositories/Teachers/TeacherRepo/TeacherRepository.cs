using System;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationNetCore.Api.Repositories.Teachers.TeacherRepo
{
    public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
    {

        private readonly IHttpContextAccessor _httpContext;

        public TeacherRepository(DataContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _httpContext = httpContextAccessor;
        }

        public async Task<Teacher> GetTeacherAsync(Guid id)
        {
            return await context.Teachers
                .Include(t => t.Classes)
                .ThenInclude(c => c.Students)
                .FirstOrDefaultAsync(u => u.Id == id && id == _httpContext.HttpContext.User.GetUserId());
        }

        public async Task RemoveProfile(Guid id)
        {
            var entity = await context.Teachers.FindAsync(id);
            context.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}