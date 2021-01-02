using System;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using AuthenticationNetCore.Api.Models;
using AuthenticationNetCore.Api.Models.Role;
using AuthenticationNetCore.Api.Models.UserDto;
using AuthenticationNetCore.Api.Services.Students.AuthStudentService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationNetCore.Api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AuthStudentController : ControllerBase
    {
        private readonly IAuthStudentService _authStudentService;
        private IMapper _mapper;

        public AuthStudentController(IAuthStudentService authStudentService, IMapper mapper)
        {
            _authStudentService = authStudentService;
            _mapper = mapper;
        }

        /// <summary>
        /// Register new Student.
        /// </summary>
        /// <returns>Return globally unique identifier of the new student</returns>
        [HttpPost("Register")]
        public async Task<ActionResult<Guid>> Register(UserRegisterDto userRegisterDto)
        {
            ServiceResponse<Guid> response = await _authStudentService.Register(
                _mapper.Map<Student>(userRegisterDto), userRegisterDto.Password
            );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Login Student.
        /// </summary>
        /// <returns>Return globally unique identifier of the new student</returns>
        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(UserLoginDto userLoginDto)
        {
            if (userLoginDto.Role != Role.Student)
            {
                return BadRequest("User is not a student");
            }
            ServiceResponse<string> response = await _authStudentService.Login(userLoginDto.UserName, userLoginDto.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        // /// <include file='AuthStudentControllerDoc.xml' path='docs/members[@name="post"]/Confirm/*'/>
        /// <summary>
        /// Confirm register by email.
        /// </summary>
        /// <remarks>
        /// A link with the user id and a token code was send to the email address. 
        /// This link should return back to the website
        /// Ex:  "http://localhost:5000/AuthStudent/Confirm?userid=615917cf-f9c4-4313-26dd-08d866073d03&code=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI2MTU5MTdjZi1mOWM0LTQzMTMtMjZkZC0wOGQ4NjYwNzNkMDMiLCJhdXRobWV0aG9kIjoiYTEzNzUzYTQtNmE4Yy00NzUxLWFhNGYtOWZkYTQ1Yjg1YTIyIiwibmJmIjoxNjAxNTU2MDU0LCJleHAiOjE2MDE1NTk2NTQsImlhdCI6MTYwMTU1NjA1NH0.UtEOQI0J-rQEL8OPmGyLl6954C1bk5d863zBscl0oDg"
        /// </remarks>
        /// <param name="id">User data to be login</param>
        /// <returns>Return globally unique identifier of the new student</returns>
        [HttpPost("Confirm/{id}")]
        public async Task<ActionResult> Confirm(Guid id)
        {
            ServiceResWithoutData response = await _authStudentService.ConfirmEmail(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}