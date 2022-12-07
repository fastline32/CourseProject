using System.Linq.Expressions;

namespace Core.Interfaces;

public interface IRepository <T> where  T : class
{
    T Find(int id);

    IEnumerable<T> GetAll(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = null,
        bool isTracking = true);
    Task<T> FirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null,
            bool isTracking = true);

    Task Add(T entity);
    void Remove(T entity);
    Task Save();
}