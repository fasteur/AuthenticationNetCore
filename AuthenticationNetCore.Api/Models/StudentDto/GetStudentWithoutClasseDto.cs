using System;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationNetCore.Api.Models.StudentDto
{
    public class GetStudentWithoutClasseDto
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
    }
}