using ProniaOnion.Domain.Entities.Common;

using System.Linq.Expressions;


namespace ProniaOnion.Application.Abstractions.Repositories.Generic
{
    public interface IGenericRepository<T> where T : BaseEntity, new()
    {
        Task CreateAsync(T entity);
        IQueryable<T> GetAll(
            int? skip = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted=false,
            params string[] includes);

        IQueryable<T> OrderAndGet(
            Expression<Func<T, object>> order,
            bool isDescending,
            int? skip = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes);

        IQueryable<T> SearchAndGet(
            Expression<Func<T, bool>> expression,
            int? skip = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes);
     
        Task<T> GetByIdAsync(
            int id,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes);
        Task<T> Get(
            Expression<Func<T, bool>> expression,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes);


        void Update(T entity);
        void Delete(T entity);
        void SoftDelete(T entity);
        void RevertSoftDelete(T entity);
        Task SaveChangesAsync();
        Task<bool> IsExistEntityAsync(Expression<Func<T, bool>> expression);
    }
}
