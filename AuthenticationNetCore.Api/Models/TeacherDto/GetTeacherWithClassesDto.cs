using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AuthenticationNetCore.Api.Models.ClasseDto;

namespace AuthenticationNetCore.Api.Models.TeacherDto
{
    public class GetTeacherWithClassesDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Role { get; set; }
        public List<GetClasseDto> Classes { get; set; }
    }
}