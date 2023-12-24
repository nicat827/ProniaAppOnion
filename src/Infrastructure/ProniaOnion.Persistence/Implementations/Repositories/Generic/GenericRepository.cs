
using Microsoft.EntityFrameworkCore;
using ProniaOnion.Application.Abstractions.Repositories.Generic;
using ProniaOnion.Domain.Entities.Common;
using ProniaOnion.Persistence.DAL;
using System.Linq.Expressions;

namespace ProniaOnion.Persistence.Implementations.Repositories.Generic
{
    internal class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _table;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _table = context.Set<T>();
        }

        public async Task CreateAsync(T entity)
        {
            await _table.AddAsync(entity);
        }
        public IQueryable<T> GetAll(
            int? skip = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes)
        {
            IQueryable<T> query = _table;
            if (showDeleted) query = query.IgnoreQueryFilters();

            if (skip != null) query = query.Skip((int)skip);
            if (limit != null) query = query.Take((int)limit);

            if (includes != null) query = _takeIncludes(query, includes);

            return isTracking ? query : query.AsNoTracking();
        }
        public IQueryable<T> SearchAndGet(
            Expression<Func<T, bool>> expression,
            int? skip = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes)
        {
            IQueryable<T> query = _table;
            if (showDeleted) query = query.IgnoreQueryFilters();
            query = query.Where(expression);
            if (skip != null) query = query.Skip((int)skip);
            if (limit != null) query = query.Take((int)limit);

            if (includes != null) query = _takeIncludes(query, includes);
            return query;
        }



        public IQueryable<T> OrderAndGet(
            Expression<Func<T, object>> order,
            bool isDescending,
            int? skip = null,
            int? limit = null,
            bool isTracking = false,
            bool showDeleted = false,
            params string[] includes)
        {
            IQueryable<T> query = _table;
            if (showDeleted) query = query.IgnoreQueryFilters();

            if (!isDescending) query = query.OrderBy(order);
            else query = query.OrderByDescending(order);

            if (skip != null) query = query.Skip((int)skip);
            if (limit != null) query = query.Take((int)limit);
                
            if (includes != null) query = _takeIncludes(query, includes);

            return isTracking ? query : query.AsNoTracking();
        }
        public async Task<T> GetByIdAsync(int id, bool isTracking = false, bool showDeleted = false, params string[] includes)
        {
            IQueryable<T> query = _table;
            if (showDeleted) query = query.IgnoreQueryFilters();
            query = query.Where(e => e.Id == id);

            if (includes != null)
            {
                query = _takeIncludes(query, includes);
            }
            return isTracking ? await query.FirstOrDefaultAsync() : await query.AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<T> Get(Expression<Func<T, bool>> expression, bool isTracking = false, bool showDeleted = false, params string[] includes)
        {
            IQueryable<T> query = _table;
            if (showDeleted) query = query.IgnoreQueryFilters();
            query = query.Where(expression);
            if (includes != null) query = _takeIncludes(query, includes);

            return isTracking ? await query.FirstOrDefaultAsync() : await query.AsNoTracking().FirstOrDefaultAsync();
        }
        public void Update(T entity)
        {
            _table.Update(entity);
        }
        public void SoftDelete(T entity)
        {
            entity.IsDeleted = true;
        }
        public void RevertSoftDelete(T entity)
        {
            entity.IsDeleted = false;
        }
        public void Delete(T entity)
        {
            _table.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<bool> IsExistEntityAsync(Expression<Func<T, bool>> expression)
        {
            return await _table.AnyAsync(expression);
        }


        private IQueryable<T> _takeIncludes(IQueryable<T> query, params string[] includes)
        {
            for (int i = 0; i < includes.Length; i++)
            {
                query = query.Include(includes[i]);
            }

            return query;
        }

    }
}
