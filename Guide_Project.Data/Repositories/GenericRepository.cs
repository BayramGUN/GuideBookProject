using System.Linq.Expressions;
using Guide_Project.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Guide_Project.Data.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly DbContext _context; 
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(DbContext context, DbSet<TEntity> dbSet)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }
    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }
    public async Task<TEntity> GetByIdAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is null)
            throw new ArgumentNullException("Argument is null");
        
        return entity;
    }
    
    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
    public TEntity Update(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;

        return entity;
    }
} 