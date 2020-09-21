using System;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using AuthenticationNetCore.Api.Models;
using AuthenticationNetCore.Api.Models.UserDto;
using AuthenticationNetCore.Api.Repositories;
using AuthenticationNetCore.Api.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationNetCore.Api.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(UserRegisterDto request)
        {
            ServiceResponse<Guid> response = await _authService.Register(
                new User { 
                    Name = request.Name,
                    FirstName = request.FirstName,
                    UserName = request.UserName,
                    Email = request.Email,
                    Role = request.Role
                }, request.Password
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
            ServiceResponse<string> response = await _authService.Login(request.UserName, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}