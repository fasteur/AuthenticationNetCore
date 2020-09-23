using System.ComponentModel.DataAnnotations;
using AuthenticationNetCore.Api.Utilities.RegexTools;

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
        public bool EmailIsValid { 
            get
            {
                return RegexUtilities.IsValidEmail(this.Email);
            }
        }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Password { get; set; }
    }
}