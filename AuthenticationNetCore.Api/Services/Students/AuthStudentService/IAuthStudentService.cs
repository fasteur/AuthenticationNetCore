using System;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using AuthenticationNetCore.Api.Models;

namespace AuthenticationNetCore.Api.Services.Students.AuthStudentService
{
    public interface IAuthStudentService
    {
        Task<ServiceResponse<Guid>> Register(Student teacher, string password);
        Task<ServiceResponse<string>> Login(string teacherName, string password);
        Task<ServiceResWithoutData> ConfirmEmail(Guid id);
        Task<bool> StudentExists(Student teacher);
    }
}