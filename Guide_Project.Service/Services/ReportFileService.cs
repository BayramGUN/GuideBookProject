using Guide_Project.Core.DTOs;
using Guide_Project.Core.Models;
using Guide_Project.Core.Services;
using Guide_Project.Core.UnitOfWork;
using Guide_Project.Service.Publishers;
using Guide_Project.SharedLibrary.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Guide_Project.Service.Services;

public class ReportFileService : IReportFileService
{
    private readonly IGenericService<ReportFile, ReportFileDto> _genericService;
    private readonly UserManager<UserEntity> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ReportFileMQPublisher<ReportMessageDto> _rabbitMQPublisher;
    public async Task<Response<ReportFileDto>> CreateReportFile(UserDto userDto)
    {
        var user = await _userManager.FindByEmailAsync(userDto.Email);
        var fileName = $"report-excel-{Guid.NewGuid().ToString().Substring(1, 10)}";

        var reportFile = new ReportFile()
        {
            UserId = user.Id,
            FileName = fileName,
            FileStatus = true
        };
        //await _genericService.AddAsync(reportFile);
        await _unitOfWork.CommitAsync();
        _rabbitMQPublisher.Publish(new ReportMessageDto() {
            FileId = reportFile.Id, UserId = user.Id
        });
        

        return Response<ReportFileDto>.Success(200);
    }
}