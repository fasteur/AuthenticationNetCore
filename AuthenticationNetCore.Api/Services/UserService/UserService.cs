using System;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using AuthenticationNetCore.Api.Models;
using AuthenticationNetCore.Api.Models.UserDto;
using AuthenticationNetCore.Api.Repositories;
using AutoMapper;

namespace AuthenticationNetCore.Api.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private IMapper _mapper;

        public UserService(IUserRepository  userRepository, IMapper mapper)
        {
            _userRepo = userRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<UserProfileDto>> GetProfileById(Guid id)
        {
            ServiceResponse<UserProfileDto> response = new ServiceResponse<UserProfileDto>();
            try
            {
                User user = await _userRepo.GetUserAsync(id);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User profile not found";
                    return response;
                }
                response.Data = _mapper.Map<UserProfileDto>(user);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResWithoutData> DeleteProfileById(Guid id)
        {
            ServiceResWithoutData res = new ServiceResWithoutData();
            try
            {
                await _userRepo.RemoveProfile(id);
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