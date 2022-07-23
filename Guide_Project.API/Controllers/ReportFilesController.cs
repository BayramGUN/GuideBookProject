using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Guide_Project.Core.Services;
using Guide_Project.Core.DTOs;
using Guide_Project.Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace Guide_Project.API.Controllers;


[Authorize(Policy = "RequireAdministratorRole")]
[Route("api/[controller]s/[action]")]
[ApiController]
public class ReportFilesController : CustomController
{
    private readonly IGenericService<ReportFile, ReportFileDto> _reportFileService;

    public ReportFilesController(IGenericService<ReportFile, ReportFileDto> reportFileService)
    {
        _reportFileService = reportFileService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllReportFiles()
    {
        return ActionResultInstance(await _reportFileService.GetAllAsync());
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReportFile(int id)
    {
        return ActionResultInstance(await _reportFileService.RemoveById(id));
    }

}