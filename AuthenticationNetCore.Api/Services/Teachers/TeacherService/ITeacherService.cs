using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using AuthenticationNetCore.Api.Models;
using AuthenticationNetCore.Api.Models.ClasseDto;

namespace AuthenticationNetCore.Api.Services.Teachers.TeacherService
{
    public interface ITeacherService
    {
        Task<List<GetClasseDto>> CreateClasse(AddClasseDto name);
        Task<List<GetClasseDto>> GetClasses(Guid id);
        Task<ServiceResWithoutData> AddStudent(AddStudentDto studentDto);
    }
}