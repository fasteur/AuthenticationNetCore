using AuthenticationNetCore.Api.Data;
using AuthenticationNetCore.Api.Models.ClasseDto;
using AuthenticationNetCore.Api.Models.Role;
using AuthenticationNetCore.Api.Models.StudentDto;
using AuthenticationNetCore.Api.Models.TeacherDto;
using AuthenticationNetCore.Api.Models.UserDto;
using AutoMapper;
using System.Linq;

namespace AuthenticationNetCore.Api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Teachers
            CreateMap<Teacher, UserProfileDto>();
            CreateMap<UserRegisterDto, Teacher>()
                .ForAllMembers(o => o.Condition((dto, t) => dto.Role == Role.Teacher));
            CreateMap<AddStudentDto, StudentClasse>()
                .ForMember(sc => sc.ClasseId, dto => dto.MapFrom(dto => dto.ClassId))
                .ForMember(sc => sc.StudentId, dto => dto.MapFrom(dto => dto.StudentId));
            CreateMap<Teacher, User>();
            CreateMap<Teacher, GetTeacherWithClassesDto>()
                .ForMember(dto => dto.Classes, t => t.MapFrom(t => t.Classes.Select(c => this.GetClasseDtoMapper().Map<GetClasseDto>(c))));
            CreateMap<Teacher, GetTeacherDto>();
            // Students
            CreateMap<Student, UserProfileDto>();
            CreateMap<UserRegisterDto, Student>()
                .ForAllMembers(o => o.Condition((dto, s) => dto.Role == Role.Student));
            CreateMap<Student, User>();
            CreateMap<Student, GetStudentDto>()
                .ForMember(dto => dto.Classes, s => s.MapFrom(s => s.StudentClasses.Select(sc => sc.Classe)));
            // Classes
            CreateMap<Classe, GetClasseDto>()
                .ForMember(dto => dto.Teacher, c => c.MapFrom(c => this.GetTeacherDtoMapper().Map<GetTeacherDto>(c.Teacher)))
                .ForMember(dto => dto.Students, c => c.MapFrom(c => c.Students
                    .Select(s => this.GetStudentWithoutClasseDtoMapper()
                    .Map<GetStudentWithoutClasseDto>(s.Student))));
        }
        private IMapper GetTeacherDtoMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Teacher, GetTeacherDto>();
            }).CreateMapper();
        }
        private IMapper GetStudentWithoutClasseDtoMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Student, GetStudentWithoutClasseDto>();
            }).CreateMapper();
        }

        private IMapper GetClasseDtoMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Classe, GetClasseDto>()
                    .ForMember(dto => dto.Teacher, c => c.MapFrom(c => this.GetTeacherDtoMapper().Map<GetTeacherDto>(c.Teacher)))
                    .ForMember(dto => dto.Students, c => c.MapFrom(c => c.Students
                        .Select(s => this.GetStudentWithoutClasseDtoMapper()
                        .Map<GetStudentWithoutClasseDto>(s.Student))));
            }).CreateMapper();
        }
    }
}