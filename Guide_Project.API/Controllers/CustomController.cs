using Guide_Project.Core.DTOs;
using Guide_Project.SharedLibrary.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Guide_Project.API.Controllers;

[Route("api/[controller]s")]
[ApiController]
public class CustomController : Controller
{
    public IActionResult ActionResultInstance<T>(Response<T> response) 
        where T : class
    {
        return new ObjectResult(response) { StatusCode = response.Status };
    }
}