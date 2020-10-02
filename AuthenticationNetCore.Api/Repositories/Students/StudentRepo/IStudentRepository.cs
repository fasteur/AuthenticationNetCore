using System;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;

namespace AuthenticationNetCore.Api.Repositories.Students.StudentRepo
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<Student> GetStudentAsync(Guid id);
        Task RemoveProfile(Guid id);
    }
}