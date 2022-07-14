using System.Security.Claims;
using Guide_Project.Core.DTOs;
using Guide_Project.Core.Models;
using Guide_Project.Core.Repositories;
using Guide_Project.Core.Services;
using Guide_Project.Core.UnitOfWork;
using Guide_Project.SharedLibrary.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Guide_Project.Service.Services;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<UserEntity> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(ITokenService tokenService, UserManager<UserEntity> 
        userManager, IUnitOfWork unitOfWork)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Response<TokenDto>> CreateAccessTokenAsync(LoginDto loginDto)
    {
        if(loginDto is null) 
            throw new ArgumentNullException(nameof(loginDto));

        var user = await _userManager.FindByNameAsync(loginDto.UserName);

        if(user is null) 
            return Response<TokenDto>.FailWithMessage("Username or password is incorrect! Please try again", 400, true);

        if(!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            return Response<TokenDto>.FailWithMessage("Password or username is uncorrect! Please try again", 400, true);
        
        var token = await _tokenService.CreateToken(user);

       
        await _unitOfWork.CommitAsync();

        return Response<TokenDto>.SuccessWithData(token, 200);
    }
}