using Guide_Project.Core.DTOs;
using Guide_Project.SharedLibrary.DTOs;

namespace Guide_Project.Core.Services;

public interface IAuthService
{
    Task<Response<TokenDto>> CreateAccessTokenAsync(LoginDto loginDto);
}