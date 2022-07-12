using AutoMapper.Internal.Mappers;
using Guide_Project.Core.Repositories;
using Guide_Project.Core.Services;
using Guide_Project.Core.UnitOfWork;
using Guide_Project.Service.Mapper;
using Guide_Project.SharedLibrary.DTOs;

namespace Guide_Project.Service.Services;

public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<TEntity> _genericRepository;

    public GenericService(IUnitOfWork unitOfWork, IGenericRepository<TEntity> genericRepository)
    {
        _unitOfWork = unitOfWork;
        _genericRepository = genericRepository;
    }

    public async Task<Response<TDto>> AddAsync(TDto entity)
    {
        var newEntity = ObjectMapper.Mapper.Map<TEntity>(entity);
        await _genericRepository.AddAsync(newEntity);
        await _unitOfWork.CommitAsync();
        return Response<TDto>.SuccessWithData(ObjectMapper.Mapper.Map<TDto>(newEntity), 200);
    }

    public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
    {
        var getEntities = await _genericRepository.GetAllAsync();

        return Response<IEnumerable<TDto>>.SuccessWithData(ObjectMapper.Mapper.Map<List<TDto>>(getEntities), 200);
    }

    public async Task<Response<TDto>> GetByIdAsync(int id)
    {
        var entity = await _genericRepository.GetByIdAsync(id);
        if(entity is null)
            return Response<TDto>.FailWithMessage("Data not found", 404, true);
        return Response<TDto>.SuccessWithData(ObjectMapper.Mapper.Map<TDto>(entity), 200);
    }

    public async Task<Response<EmptyDto>> RemoveAll()
    {
        var allExistEntities = await _genericRepository.GetAllAsync();
        if(allExistEntities is null) 
            return Response<EmptyDto>.FailWithMessage("No data found in db", 404, true);
        _genericRepository.RemoveAll();
        await _unitOfWork.CommitAsync();
        return Response<EmptyDto>.Success(204);
    }

    public async Task<Response<EmptyDto>> RemoveById(int id)
    {
        var isExistEntity = await _genericRepository.GetByIdAsync(id);

        if (isExistEntity is null)    
            return Response<EmptyDto>.FailWithMessage("Id not found", 404, true);
    

        _genericRepository.Remove(isExistEntity);
        await _unitOfWork.CommitAsync();

        return Response<EmptyDto>.Success(204);
    }

    public async Task<Response<EmptyDto>> Update(TDto entity, int id)
    {
        var isExistEntity = await _genericRepository.GetByIdAsync(id);

        if (isExistEntity is null)    
            return Response<EmptyDto>.FailWithMessage("Id not found", 404, true);
    
        var update = ObjectMapper.Mapper.Map<TEntity>(entity);
        _genericRepository.Update(update);

        await _unitOfWork.CommitAsync();

        return Response<EmptyDto>.Success(204);
    }
} 
