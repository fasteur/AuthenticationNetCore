using System;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using AuthenticationNetCore.Api.Models;
using AuthenticationNetCore.Api.Models.UserDto;
using AuthenticationNetCore.Api.Repositories.Teachers.AuthTeacherRepo;
using AuthenticationNetCore.Api.Utilities.EmailTools;
using AuthenticationNetCore.Api.Utilities.HashFunctionTools;
using AuthenticationNetCore.Api.Utilities.TokenTools;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace AuthenticationNetCore.Api.Services.Teachers.AuthTeacherService
{
    public class AuthTeacherService : IAuthTeacherService
    {
        private IAuthTeacherRepository _authTeacherRepo;
        private readonly IConfiguration _config;
        private IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public AuthTeacherService(
            IAuthTeacherRepository authTeacherRepository,
            IConfiguration configuration,
            IEmailSender emailSender,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _authTeacherRepo = authTeacherRepository;
            _config = configuration;
            _emailSender = emailSender;
            _httpContext = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<string>> Login(string teacherName, string password)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            Teacher teacher = await _authTeacherRepo.FindOneAsync(t => t.UserName.ToLower().Equals(teacherName.ToLower()) && t.EmaiIsValid);
            if (teacher == null)
            {
                response.Success = false;
                response.Message = "Teacher not found.";
            }
            else if (!HashUtilities.VerifyPasswordHash(password, teacher.PasswordHash, teacher.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password.";
            }
            else
            {
                var user = _mapper.Map<User>(teacher);
                response.Data = TokenUtilities.CreateToken(_config, user);
            }
            return response;
        }

        public async Task<ServiceResponse<Guid>> Register(Teacher teacher, string password)
        {
            ServiceResponse<Guid> response = new ServiceResponse<Guid>();
            if (await TeacherExists(teacher))
            {
                response.Success = false;
                response.Message = "TeacherName or Email already exists.";
                return response;
            };
            HashUtilities.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            teacher.PasswordHash = passwordHash;
            teacher.PasswordSalt = passwordSalt;

            var emailCode = Guid.NewGuid();
            HashUtilities.CreatePasswordHash(emailCode.ToString(), out byte[] emailCodeHash, out byte[] emailCodeSalt);
            teacher.EmailCodeHash = emailCodeHash;
            teacher.EmailCodeSalt = emailCodeSalt;

            await _authTeacherRepo.AddAsync(teacher);
            await _authTeacherRepo.SaveChangesAsync();
            response.Data = teacher.Id;
            var user = _mapper.Map<User>(teacher);
            await _emailSender.SendEmailAsync("asteur.florian@gmail.com", "Confim email", EmailUtilities.EmailContentMessage(_config, user, emailCode));
            return response;
        }
        public async Task<bool> TeacherExists(Teacher teacher)
        {
            return (await _authTeacherRepo.AnyAsync(t => t.Name.ToLower() == teacher.Name || t.Email == teacher.Email)) ? true : false;
        }

        public async Task<ServiceResWithoutData> ConfirmEmail(Guid id)
        {
            var res = new ServiceResWithoutData();
            try
            {
                var claimPrincipal = _httpContext.HttpContext.User;
                var claimTeacherId = claimPrincipal.GetUserId();
                var claimCode = claimPrincipal.GetConfirmCode().ToString();
                var user = await _authTeacherRepo.FindOneAsync(u => u.Id == id && claimTeacherId == id);
                if (user == null || !HashUtilities.VerifyPasswordHash(claimCode, user.EmailCodeHash, user.EmailCodeSalt))
                {
                    res.Success = false;
                    res.Message = "Email address is not confirmed!";
                }
                else if(user.EmaiIsValid)
                {
                    res.Success = false;
                    res.Message = "Email is aldready confirmed!";
                }
                else
                {
                    user.EmaiIsValid = true;
                    await _authTeacherRepo.SaveChangesAsync();
                    res.Success = true;
                    res.Message = "Email address is confirmed!";
                }
            }
            catch (Exception ex)
            {
                res.Success = false; 
                res.Message = ex.Message;
            }

            return res;
        }


    }
}