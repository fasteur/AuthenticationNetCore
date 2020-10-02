using System;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using AuthenticationNetCore.Api.Models;

namespace AuthenticationNetCore.Api.Services.Teachers.AuthTeacherService
{
    public interface IAuthTeacherService
    {
        Task<ServiceResponse<Guid>> Register(Teacher teacher, string password);
        Task<ServiceResponse<string>> Login(string teacherName, string password);
        Task<ServiceResWithoutData> ConfirmEmail(Guid id);
        Task<bool> TeacherExists(Teacher teacher);
    }
}