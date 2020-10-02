using System.ComponentModel.DataAnnotations;
namespace AuthenticationNetCore.Api.Models.UserDto
{
    public class UserLoginDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}