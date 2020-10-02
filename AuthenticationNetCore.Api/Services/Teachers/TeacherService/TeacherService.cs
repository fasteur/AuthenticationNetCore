using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using AuthenticationNetCore.Api.Models;
using AuthenticationNetCore.Api.Models.ClasseDto;
using AuthenticationNetCore.Api.Repositories.ClasseRepo;
using AuthenticationNetCore.Api.Repositories.Teachers.TeacherRepo;
using Microsoft.AspNetCore.Http;
using System.Linq;
using AutoMapper;
using AuthenticationNetCore.Api.Repositories.Students.StudentRepo;

namespace AuthenticationNetCore.Api.Services.Teachers.TeacherService
{
    public class TeacherService : ITeacherService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IClasseRepository _classeRepo;
        private readonly ITeacherRepository _teacherRepo;
        private readonly IStudentRepository _studentRepo;
        private readonly IMapper _mapper;

        public TeacherService(
            IHttpContextAccessor httpContextAccessor,
            IClasseRepository classeRepository,
            ITeacherRepository teacherRepository,
            IStudentRepository studentRepository,
            IMapper mapper)
        {
            _httpContext = httpContextAccessor;
            _classeRepo = classeRepository;
            _teacherRepo = teacherRepository;
            _studentRepo = studentRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResWithoutData> AddStudent(AddStudentDto studentsDto)
        {
            ServiceResWithoutData res = new ServiceResWithoutData();
            try
            {
                var teacherId = _httpContext.HttpContext.User.GetUserId();
                var teacher = await _teacherRepo.GetTeacherAsync(teacherId);
                var student = await _studentRepo.GetAsync(studentsDto.StudentId);
                var classe = teacher.Classes.FirstOrDefault(c => c.Id == studentsDto.ClassId);

                classe.Students.Add(_mapper.Map<StudentClasse>(studentsDto));
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.Message;
            }
            return res;
        }

        public async Task<List<GetClasseDto>> CreateClasse(AddClasseDto classeDto)
        {
            var userId = _httpContext.HttpContext.User.GetUserId();
            var teacher = await _teacherRepo.GetTeacherAsync(userId);
            var newClasse = new Classe()
            {
                Name = classeDto.Name,
                Teacher = teacher
            };
            await _classeRepo.AddAsync(newClasse);
            await _classeRepo.SaveChangesAsync();

            return teacher.Classes.Select(c => _mapper.Map<GetClasseDto>(c)).ToList();
        }

        public async Task<List<GetClasseDto>> GetClasses(Guid id)
        {
            var teacher = await _teacherRepo.GetTeacherAsync(id);
            return teacher.Classes.Select(c => _mapper.Map<GetClasseDto>(c)).ToList();
        }

    }
}