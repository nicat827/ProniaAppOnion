
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

        public void Delete(T entity)
        {
            _table.Remove(entity);
        }

        public IQueryable<T> FilterAndGet(int skip, int limit, Expression<Func<T, bool>> expression, bool isTracking = false)
        {
            IQueryable<T> query = _table;
            query = query.Where(expression);
            if (skip != 0) query = query.Skip(skip);
            if (limit != 0) query = query.Take(limit);
            return isTracking ? query : query.AsNoTracking();
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression, bool isTracking = false, bool showDeleted = false, params string[] includes)
        {
            IQueryable<T> query = _table.Where(expression);
            if (showDeleted) query = query.IgnoreQueryFilters();
            if (includes != null)
            {
                query = TakeIncludes(query, includes);
            }
            return isTracking ? await query.FirstOrDefaultAsync() : await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public IQueryable<T> GetAll(int skip = 0, int limit = 0, bool isTracking = false, bool showDeleted = false, params string[] includes)
        {
            IQueryable<T> query = _table;

            if (skip != 0) query = query.Skip(skip);
            if (limit != 0) query = query.Take(limit);
            if (showDeleted) query = query.IgnoreQueryFilters();

            if (includes != null)
            {
                query = TakeIncludes(query, includes);
            }

            return isTracking ? query : query.AsNoTracking();
        }

        public async Task<T> GetByIdAsync(int id, bool isTracking = false, bool showDeleted = false, params string[] includes)
        {
            IQueryable<T> query = _table;
            query = query.Where(e => e.Id == id);
            if (showDeleted) query = query.IgnoreQueryFilters();

            if (includes != null)
            {
                query = TakeIncludes(query, includes);
            }
            return isTracking ? await query.FirstOrDefaultAsync() : await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public IQueryable<T> OrderAndGet(Expression<Func<T, object>> order, bool isDescending, int skip = 0, int limit = 0, bool isTracking = false, bool showDeleted = false, params string[] includes)
        {
            IQueryable<T> query = _table;
            if (showDeleted) query = query.IgnoreQueryFilters();

            if (!isDescending) query = query.OrderBy(order);
            else query = query.OrderByDescending(order);
            if (skip != 0) query = query.Skip(skip);
            if (limit != 0) query = query.Take(limit);
            if (includes != null)
            {
                query = TakeIncludes(query, includes);
            }
            return isTracking ? query : query.AsNoTracking();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IQueryable<T> SearchAndGet(
            Expression<Func<T, bool>> expression,
            int skip = 0,
            int limit = 0,
            bool isTracking = false,
            params string[] includes)
        {
            IQueryable<T> query = _table;

            query = query.Where(expression);
            if (skip != 0) query = query.Skip(skip);
            if (limit != 0) query = query.Take(limit);
            if (includes != null)
            {
                query = TakeIncludes(query, includes);
            }
            return query;
        }

        public void Update(T entity)
        {
            _table.Update(entity);
        }

        public IQueryable<T> TakeIncludes(IQueryable<T> query, params string[] includes)
        {
            for (int i = 0; i < includes.Length; i++)
            {
                query = query.Include(includes[i]);
            }

            return query;
        }

        public void SoftDelete(T entity)
        {
            
            Update(entity);
        }
    }
}
