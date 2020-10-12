using System;
using System.Collections.Generic;
using AuthenticationNetCore.Api.Models.StudentDto;
using AuthenticationNetCore.Api.Models.TeacherDto;

namespace AuthenticationNetCore.Api.Models.ClasseDto
{
    public class GetClasseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public GetTeacherDto Teacher { get; set; }
        public List<GetStudentWithoutClasseDto> Students { get; set; }
    }
}