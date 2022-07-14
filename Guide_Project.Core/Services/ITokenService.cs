using Guide_Project.Core.DTOs;
using Guide_Project.Core.Models;

namespace Guide_Project.Core.Services;

public interface ITokenService
{
    Task<TokenDto> CreateToken(UserEntity userEntity);
    string CreateRefreshToken();
}