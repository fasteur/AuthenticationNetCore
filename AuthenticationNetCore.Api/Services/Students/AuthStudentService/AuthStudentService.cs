using System;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using AuthenticationNetCore.Api.Models;
using AuthenticationNetCore.Api.Models.UserDto;
using AuthenticationNetCore.Api.Repositories.Students.AuthStudentRepo;
using AuthenticationNetCore.Api.Utilities.EmailTools;
using AuthenticationNetCore.Api.Utilities.HashFunctionTools;
using AuthenticationNetCore.Api.Utilities.TokenTools;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace AuthenticationNetCore.Api.Services.Students.AuthStudentService
{
    public class AuthStudentService : IAuthStudentService
    {
        private IAuthStudentRepository _authStudentRepo;
        private readonly IConfiguration _config;
        private IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public AuthStudentService(
            IAuthStudentRepository authStudentRepository,
            IConfiguration configuration,
            IEmailSender emailSender,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _authStudentRepo = authStudentRepository;
            _config = configuration;
            _emailSender = emailSender;
            _httpContext = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<string>> Login(string studentName, string password)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            Student student = await _authStudentRepo.FindOneAsync(s => s.UserName.ToLower().Equals(studentName.ToLower()) && s.EmaiIsValid);
            if (student == null)
            {
                response.Success = false;
                response.Message = "Student not found.";
            }
            else if (!HashUtilities.VerifyPasswordHash(password, student.PasswordHash, student.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password.";
            }
            else
            {
                var user = _mapper.Map<User>(student);
                response.Data = TokenUtilities.CreateToken(_config, user);
            }
            return response;
        }

        public async Task<ServiceResponse<Guid>> Register(Student student, string password)
        {
            ServiceResponse<Guid> response = new ServiceResponse<Guid>();
            if (await StudentExists(student))
            {
                response.Success = false;
                response.Message = "StudentName or Email already exists.";
                return response;
            };
            HashUtilities.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            student.PasswordHash = passwordHash;
            student.PasswordSalt = passwordSalt;

            var emailCode = Guid.NewGuid();
            HashUtilities.CreatePasswordHash(emailCode.ToString(), out byte[] emailCodeHash, out byte[] emailCodeSalt);
            student.EmailCodeHash = emailCodeHash;
            student.EmailCodeSalt = emailCodeSalt;

            await _authStudentRepo.AddAsync(student);
            await _authStudentRepo.SaveChangesAsync();
            response.Data = student.Id;
            var user = _mapper.Map<User>(student);
            await _emailSender.SendEmailAsync("asteur.florian@gmail.com", "Confim email", EmailUtilities.EmailContentMessage(_config, user, emailCode));
            return response;
        }
        public async Task<bool> StudentExists(Student student)
        {
            return (await _authStudentRepo.AnyAsync(t => t.Name.ToLower() == student.Name || t.Email == student.Email)) ? true : false;
        }

        public async Task<ServiceResWithoutData> ConfirmEmail(Guid id)
        {
            var res = new ServiceResWithoutData();
            try
            {
                var claimPrincipal = _httpContext.HttpContext.User;
                var claimStudentId = claimPrincipal.GetUserId();
                var claimCode = claimPrincipal.GetConfirmCode().ToString();
                var user = await _authStudentRepo.FindOneAsync(u => u.Id == id && claimStudentId == id);
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
                    await _authStudentRepo.SaveChangesAsync();
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