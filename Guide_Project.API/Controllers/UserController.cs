using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Guide_Project.Core.Services;
using Guide_Project.Core.DTOs;

namespace Guide_Project.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UserController : CustomController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Signup(SignupDto signupDto)
    {
        return ActionResultInstance(await _userService.CreateUserAsync(signupDto));
    }
    [HttpPost]
    public async Task<IActionResult> CreateAdmin()
    {
        var admin = await _userService.DefaultAdminAccaunt();
        return ActionResultInstance(admin);
    }
    
}