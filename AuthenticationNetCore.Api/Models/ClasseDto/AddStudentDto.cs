using System;
using System.Collections.Generic;

namespace AuthenticationNetCore.Api.Models.ClasseDto
{
    public class AddStudentDto
    {
        public Guid ClassId { get; set; }
        public Guid StudentId { get; set; }
    }
}