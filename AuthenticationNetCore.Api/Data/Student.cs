using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationNetCore.Api.Data
{
    public class Student
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        
        [EmailAddress]
        [Required]
        public bool EmaiIsValid { get; set; } = false;
        public byte[] EmailCodeHash { get; set; }
        public byte[] EmailCodeSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; }

        public List<StudentClasse> StudentClasses { get; set; }
        
    }
}