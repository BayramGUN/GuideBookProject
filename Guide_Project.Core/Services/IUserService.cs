using Guide_Project.Core.DTOs;
using Guide_Project.SharedLibrary.DTOs;

namespace Guide_Project.Core.Services;

public interface IUserService
{
    Task<Response<UserDto>> CreateUserAsync(SignupDto signupDto);
    Task<Response<UserDto>> DefaultAdminAccaunt();
}