using System.Linq.Expressions;
using Guide_Project.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Guide_Project.Data.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> 
    where TEntity : class
{
    private readonly AppDbContext _context; 
    //private readonly DbSet<TEntity> _context.Set<TEntity>();

    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
    }
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }
    public async Task<TEntity> GetByIdAsync(int id)
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        if (entity is null)
            throw new ArgumentNullException("Argument is null");
        
        return entity;
    }
    
    public void Remove(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }
    public void RemoveAll()
    {
        _context.Set<TEntity>().ToList().ForEach(ctx => _context.Set<TEntity>().Remove(ctx));
    }
    public TEntity Update(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;

        return entity;
    }
} 