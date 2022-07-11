using Guide_Project.Core.Services;


namespace Guide_Project.Service.Services;

public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> 
    where TEntity : class
    where TDto : class
{
    
} 
