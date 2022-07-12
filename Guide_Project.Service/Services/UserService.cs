using AutoMapper.Internal.Mappers;
using Guide_Project.Core.DTOs;
using Guide_Project.Core.Models;
using Guide_Project.Core.Services;
using Guide_Project.Core.UnitOfWork;
using Guide_Project.Service.Mapper;
using Guide_Project.SharedLibrary.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Guide_Project.Service.Services;

public class UserService : IUserService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly RoleManager<RoleEntity> _roleManager;
    private readonly IUnitOfWork _unitOfWork;

    public IConfiguration Configuration { get; set; }
    public UserService(UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager,
        IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        Configuration = configuration;
        _unitOfWork = unitOfWork;
    }
    public async Task<Response<UserDto>> CreateUserAsync(SignupDto signupDto)
    {
        var user = new UserEntity 
        { 
            UserName = signupDto.UserName,
            Email = signupDto.Email,
            Department = signupDto.Department 
        }; 
        
        var result = await _userManager.CreateAsync(user, signupDto.Password); 
        await _userManager.AddToRoleAsync(user, "editor"); 
        if(!result.Succeeded)
        {
            var errors = result.Errors.Select(ctx => ctx.Description).ToList();

            return Response<UserDto>.Fail(new ErrorDto(errors, true), 400);
        }
        await _roleManager.CreateAsync(new() { Name = "editor" });
        return Response<UserDto>.SuccessWithData(ObjectMapper.Mapper.Map<UserDto>(user), 200);
    }
    public async Task<Response<UserDto>> DefaultAdminAccaunt()
    {
        var admin = new UserEntity
        {
            UserName = Configuration["Admin:Name"],
            Email = Configuration["Admin:Email"],
            Department = "administrator"
        }; 
        var adminSignup = new SignupDto
        {
            UserName = Configuration["Admin:Name"],
            Password = Configuration["Admin:SecretPassword"],
        };
        var result = await _userManager.CreateAsync(admin, adminSignup.Password); 
        await _userManager.AddToRoleAsync(admin, "admin"); 
        if(!result.Succeeded)
        {
            var errors = result.Errors.Select(ctx => ctx.Description).ToList();

            return Response<UserDto>.Fail(new ErrorDto(errors, true), 400);
        }
        await _roleManager.CreateAsync(new() { Name = "admin" });
        await _unitOfWork.CommitAsync();

        return Response<UserDto>.SuccessWithData(ObjectMapper.Mapper.Map<UserDto>(admin), 200);
    }
}