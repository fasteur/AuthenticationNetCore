using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using AuthenticationNetCore.Api.Models;
using AuthenticationNetCore.Api.Models.ClasseDto;
using AuthenticationNetCore.Api.Services.Teachers.TeacherService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationNetCore.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TeacherController : ControllerBase
    {
        private ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost("Classes")]
        public async Task<ActionResult<List<GetClasseDto>>> CreateClasse(AddClasseDto newClasse)
        {
            return Ok(await _teacherService.CreateClasse(newClasse));
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet("Classes/{id}")]
        public async Task<ActionResult<List<GetClasseDto>>> GetClasses(Guid id)
        {
            return Ok(await _teacherService.GetClasses(id));
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost("Student")]
        public async Task<IActionResult> AddStudent(AddStudentDto addStudentDto)
        {
            ServiceResWithoutData response = await _teacherService.AddStudent(addStudentDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
    
        }

    }
}