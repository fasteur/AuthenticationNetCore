using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.Json.Serialization;

namespace AuthenticationNetCore.Api.Models.UserDto
{
    public class UserRegisterDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Password { get; set; }
    }
}