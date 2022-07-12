using AutoMapper;
using Guide_Project.Core.DTOs;
using Guide_Project.Core.Models;

namespace Guide_Project.Service.Mapper;

public class DtoMapper : Profile
{
    public DtoMapper()
    {
        CreateMap<UserDto, UserEntity>().ReverseMap();
        CreateMap<CustomerDto, Customer>().ReverseMap();
        CreateMap<CommercialActivityDto, CommercialActivity>().ForMember(
                                            dest => dest.CustomerId, 
                                            opt => opt.Ignore()
                                        );
    }
}