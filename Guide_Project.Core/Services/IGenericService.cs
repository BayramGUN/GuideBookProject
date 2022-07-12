using Guide_Project.SharedLibrary.DTOs;

namespace Guide_Project.Core.Services;
public interface IGenericService<TEntity, TDto> 
    where TEntity : class 
    where TDto : class
{
    Task<Response<TDto>> GetByIdAsync(int id);
    Task<Response<TDto>> AddAsync(TDto entity);
    Task<Response<EmptyDto>> RemoveById(int id);
    Task<Response<EmptyDto>> RemoveAll();
    Task<Response<EmptyDto>> Update(TDto entity, int id);
    Task<Response<IEnumerable<TDto>>> GetAllAsync();
}