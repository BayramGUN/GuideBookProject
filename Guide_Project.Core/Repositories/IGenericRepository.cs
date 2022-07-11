using System.Linq.Expressions;

namespace Guide_Project.Core.Repositories;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(int id);
    Task AddAsync(TEntity entity);
    void Remove(TEntity entity);
    TEntity Update(TEntity entity);
}