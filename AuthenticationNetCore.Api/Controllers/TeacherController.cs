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

        // --- POST ---
        
        /// <summary>
        /// Create new student's class.
        /// </summary>
        /// <returns>Return list of the teacher's class</returns>
        [Authorize(Roles = "Teacher")]
        [HttpPost("Classes")]
        public async Task<ActionResult<List<GetClasseDto>>> CreateClasse(AddClasseDto newClasse)
        {
            return Ok(await _teacherService.CreateClasse(newClasse));
        }

        /// <summary>
        /// Add student to a class.
        /// </summary>
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
    
        // --- GET ---

        /// <summary>
        /// Get teacher's classes.
        /// </summary>
        /// <param name="teacherId">Guid of the teacher</param>
        /// <returns>Return list of the teacher's class</returns>
        [Authorize(Roles = "Teacher")]
        [HttpGet("Classes/{id}")]
        public async Task<ActionResult<List<GetClasseDto>>> GetClasses(Guid teacherId)
        {
            return Ok(await _teacherService.GetClasses(teacherId));
        }

        // --- DELETE ---

        [Authorize(Roles = "Teacher")]
        [HttpDelete("Classes")]
        public async Task<IActionResult> DeleteClasse(DeleteClasseDto deleteClasseDto)
        {
            ServiceResWithoutData response = await _teacherService.RemoveClasse(deleteClasseDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}