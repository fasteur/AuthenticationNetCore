using System;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Models;
using AuthenticationNetCore.Api.Models.UserDto;
using AuthenticationNetCore.Api.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationNetCore.Api.Controllers
{
    [Authorize]
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
        
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(Guid id)
        {
            ServiceResWithoutData response = await _userService.DeleteProfileById(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}