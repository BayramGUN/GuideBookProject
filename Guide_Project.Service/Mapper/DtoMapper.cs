using AutoMapper;
using Guide_Project.Core.DTOs;
using Guide_Project.Core.Models;

namespace Guide_Project.Service.Mapper;

public class DtoMapper : Profile
{
    public DtoMapper()
    {
        CreateMap<UserDto, UserEntity>().ReverseMap();
    }
}