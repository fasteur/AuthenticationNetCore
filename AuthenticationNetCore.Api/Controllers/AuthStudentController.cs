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

        [HttpPost("Register")]
        public async Task<ActionResult> Register(UserRegisterDto request)
        {
            ServiceResponse<Guid> response = await _authStudentService.Register(
                _mapper.Map<Student>(request), request.Password
            );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(UserLoginDto request)
        {
            if (request.Role != Role.Student)
            {
                return BadRequest("User is not a student");
            }
            ServiceResponse<string> response = await _authStudentService.Login(request.UserName, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

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