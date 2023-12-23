using ProniaOnion.Domain.Entities.Common;

using System.Linq.Expressions;


namespace ProniaOnion.Application.Abstractions.Repositories.Generic
{
    public interface IGenericRepository<T> where T : BaseEntity, new()
    {
        IQueryable<T> GetAll(int skip = 0, int limit = 0, bool isTracking = false,bool showDeleted=false, params string[] includes);

        IQueryable<T> OrderAndGet(Expression<Func<T, object>> order, bool isDescending, int skip = 0, int limit = 0, bool isTracking = false, bool showDeleted = false, params string[] includes);

        IQueryable<T> SearchAndGet(Expression<Func<T, bool>> expression, int skip = 0, int limit = 0, bool isTracking = false, params string[] includes);
        IQueryable<T> FilterAndGet(int skip, int limit, Expression<Func<T, bool>> expression, bool isTracking = false);
        Task<T> GetByIdAsync(int id, bool isTracking = false, bool showDeleted = false, params string[] includes);
        Task<T> Get(Expression<Func<T, bool>> expression, bool isTracking = false, bool showDeleted = false, params string[] includes);

        Task CreateAsync(T entity);

        void Update(T entity);
        void Delete(T entity);
        void SoftDelete(T entity);
        Task SaveChangesAsync();
    }
}
