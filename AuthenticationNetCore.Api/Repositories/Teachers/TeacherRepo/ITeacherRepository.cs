using System;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;

namespace AuthenticationNetCore.Api.Repositories.Teachers.TeacherRepo
{
    public interface ITeacherRepository : IRepository<Teacher>
    {
        Task<Teacher> GetTeacherAsync(Guid id);
        Task RemoveProfile(Guid id);
    }
}