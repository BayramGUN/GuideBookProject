using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Guide_Project.Core.Services;
using Guide_Project.Core.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Guide_Project.API.Controllers;


[Authorize(Policy = "RequireAdministratorRole")]
[Route("api/[controller]s/[action]")]
[ApiController]
public class ReportFilesController : CustomController
{
    
}