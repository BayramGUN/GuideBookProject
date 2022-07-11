using AutoMapper;

namespace Guide_Project.Service.Mapper;

public class ObjectMapper
{
    private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
    {
        var configuration = new MapperConfiguration(config =>
        {
            config.AddProfile<DtoMapper>();
        });
        return configuration.CreateMapper();
    });
    public static IMapper Mapper => lazy.Value;
}