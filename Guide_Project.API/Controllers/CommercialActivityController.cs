using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Guide_Project.Core.Services;
using Guide_Project.Core.DTOs;
using Guide_Project.Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace Guide_Project.API.Controllers;
[Authorize]
[Route("api/[controller]s/[action]")]
[ApiController]
public class CommercialActivityController : CustomController
{
    private readonly IGenericService<CommercialActivity, CommercialActivityDto> _commercialActivityService;

    public CommercialActivityController(IGenericService<CommercialActivity, CommercialActivityDto> commercialActivityService)
    {
        _commercialActivityService = commercialActivityService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCommercialActivities()
    {
        return ActionResultInstance(await _commercialActivityService.GetAllAsync());
    }

    [HttpPost]
    public async Task<IActionResult> CreateCommercialActivity(CommercialActivityDto CommercialActivityDto)
    {
        return ActionResultInstance(await _commercialActivityService.AddAsync(CommercialActivityDto));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCommercialActivity(CommercialActivityDto CommercialActivityDto)
    {
        return ActionResultInstance(await _commercialActivityService.Update(CommercialActivityDto, CommercialActivityDto.Id));
    }

    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCommercialActivity(int id)
    {
        return ActionResultInstance(await _commercialActivityService.RemoveById(id));
    }

    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpDelete]
    public async Task<IActionResult> DeleteAllCommercialActivities()
    {
        return ActionResultInstance(await _commercialActivityService.RemoveAll());
    }
}