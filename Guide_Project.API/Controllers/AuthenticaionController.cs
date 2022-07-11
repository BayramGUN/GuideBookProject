using Guide_Project.Core.DTOs;
using Guide_Project.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Guide_Project.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthenticaionController : CustomController
{
    private readonly IAuthService _authService;

    public AuthenticaionController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccessToken(LoginDto loginDto)
    {
        return ActionResultInstance(await _authService.CreateAccessTokenAsync(loginDto));
    }

    
}

 