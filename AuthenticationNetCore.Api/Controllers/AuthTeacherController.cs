using System;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using AuthenticationNetCore.Api.Models;
using AuthenticationNetCore.Api.Models.Role;
using AuthenticationNetCore.Api.Models.UserDto;
using AuthenticationNetCore.Api.Services.Teachers.AuthTeacherService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationNetCore.Api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AuthTeacherController : ControllerBase
    {
        private readonly IAuthTeacherService _authTeacherService;
        private IMapper _mapper;

        public AuthTeacherController(IAuthTeacherService authTeacherService, IMapper mapper)
        {
            _authTeacherService = authTeacherService;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(UserRegisterDto request)
        {
            ServiceResponse<Guid> response = await _authTeacherService.Register(
                _mapper.Map<Teacher>(request), request.Password
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
            if (request.Role != Role.Teacher)
            {
                return BadRequest("User is not a Teacher!");
            }
            ServiceResponse<string> response = await _authTeacherService.Login(request.UserName, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Confirm/{id}")]
        public async Task<ActionResult> Confirm(Guid id)
        {
            ServiceResWithoutData response = await _authTeacherService.ConfirmEmail(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}