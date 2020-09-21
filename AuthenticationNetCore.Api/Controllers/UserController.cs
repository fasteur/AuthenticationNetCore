using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Models;
using AuthenticationNetCore.Api.Models.UserDto;
using AuthenticationNetCore.Api.Repositories;
using AuthenticationNetCore.Api.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationNetCore.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin, Teacher, Student")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileDto>> GetProfile(Guid id)
        {
            ServiceResponse<UserProfileDto> response = await _userService.GetProfileById(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(await _userService.GetProfileById(id));   
        }

    }
}