using System.Linq.Expressions;
using Core;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    internal DbSet<T> _dbSet;
    public Repository(ApplicationDbContext db)
    {
        _db = db;
        this._dbSet = _db.Set<T>();
    }
    public T Find(int id)
    {
        return _dbSet.Find(id);
    }

    public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null,
        bool isTracking = true)
    {
        IQueryable<T> query = _dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties!=null)
        {
            foreach (var item in includeProperties.Split(new []{','},StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(item);
            }
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (!isTracking)
        {
            query = query.AsNoTracking();
        }

        return query.ToList();
    }

    public async Task<T> FirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null, bool isTracking = true)
    {
        IQueryable<T> query = _dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties!=null)
        {
            foreach (var item in includeProperties.Split(new []{','},StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(item);
            }
        }

        if (!isTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task Add(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task Save()
    {
        await _db.SaveChangesAsync();
    }
}