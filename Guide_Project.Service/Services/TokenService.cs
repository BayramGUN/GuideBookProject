using System.Text;
using Guide_Project.Core.DTOs;
using Guide_Project.Core.Models;
using Guide_Project.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Guide_Project.Service.Services;

public class TokenService : ITokenService
{
    public readonly UserManager<UserEntity> _userManager;
    public IConfiguration Configuration { get; set; }

    public TokenService(UserManager<UserEntity> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        Configuration = configuration;
    }

    public async Task<TokenDto> CreateToken(UserEntity userEntity)
    {
        var accessTokenExpiration = DateTime.Now.AddHours(8);
        var refreshTokenExpiration = DateTime.Now.AddHours(2);
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes(Configuration["TokenOptions:SecurityKey"]));
        SigningCredentials credintials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var claims = new List<Claim>() { new Claim("department", userEntity.Department)};
        var userRoles = await _userManager.GetRolesAsync(userEntity);
        userRoles.ToList().ForEach(ctx => {
            claims.Add(new Claim(ClaimTypes.Role, ctx));
        });
        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer: Configuration["TokenOptions:Issuer"],
            audience: Configuration["TokenOptions:Audience"],
            expires: accessTokenExpiration,
            claims: claims,
            notBefore: DateTime.Now,
            signingCredentials: credintials
        );

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.WriteToken(jwtSecurityToken);

        var tokenDto = new TokenDto
        {
            AccessToken = token,
            RefreshToken = CreateRefreshToken(),
            AccessTokenExpiration = accessTokenExpiration,
            RefreshTokenExpiration = refreshTokenExpiration
        };
        
        return tokenDto;
    }
    public string CreateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }
}