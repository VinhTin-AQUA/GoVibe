using System.Linq.Expressions;
using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.Infrastructure.Repositories.Common
{
    public interface IQueryRepository<T> where T : Entity
    {
        Task<T?> GetByIdAsync(Guid id, bool tracking = false);

        Task<List<T>> GetByIdsAsync(IEnumerable<Guid> ids, bool tracking = false);
        
        Task<List<T>> GetAllAsync(bool tracking = false);
        
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        Task<List<T>> GetPagedAsync(int page, int pageSize);
        
        Task ExecuteRawSqlAsync(string sql, params object[] parameters);

        Task<List<T>> FilterAsync(QueryOptions<T, T> options);
    }
    
    public class QueryRepository<T> : IQueryRepository<T> where T : Entity
    {
        protected readonly IDbContextFactory<AppDbContext> _contextFactory;


        public QueryRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<T?> GetByIdAsync(Guid id, bool tracking = false)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            if (tracking)
                return await context.Set<T>()
                    .FirstOrDefaultAsync(x => x.Id == id);

            return await context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        

        public async Task<List<T>> GetByIdsAsync(IEnumerable<Guid> ids, bool tracking = false)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            if (tracking)
                return await context.Set<T>()
                    .Where(x => ids.Contains(x.Id))
                    .ToListAsync();

            return await context.Set<T>()
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }
        

        public async Task<List<T>> GetAllAsync(bool tracking = false)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            if (tracking)
                return await context.Set<T>()
                    .ToListAsync();

            return await context.Set<T>()
                .AsNoTracking()
                .ToListAsync();
        }
        

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context
                .Set<T>()
                .AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<T>().CountAsync(predicate);
        }
        

        public async Task<List<T>> GetPagedAsync(int page, int pageSize)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.Set<T>()
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task ExecuteRawSqlAsync(string sql, params object[] parameters)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            await context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public async Task<List<T>> FilterAsync(QueryOptions<T, T> options)
        {
            await using var context = await _contextFactory.CreateDbContextAsync(options.CancellationToken);
            IQueryable<T> query = context.Set<T>();
            query = CommonQueries(query, options, false);
            return await query.ToListAsync(options.CancellationToken);
        }
        
        public async Task<T?> Find(QueryOptions<T, T> options)
        {
            await using var context = await _contextFactory.CreateDbContextAsync(options.CancellationToken);
            IQueryable<T> query = context.Set<T>();
            query = CommonQueries(query, options, false);
            return await query.FirstOrDefaultAsync(options.CancellationToken);
        }
        
        private IQueryable<T> CommonQueries(IQueryable<T> query, QueryOptions<T, T> options, bool includeDeleted)
        {
            if (options.Include != null) query = options.Include(query);
            if (options.AsNoTracking) query = query.AsNoTracking();
            if (options.AsSplitQuery) query = query.AsSplitQuery();

            if (includeDeleted)
            {
                if (options.Predicate != null) query = query.Where(options.Predicate);
            }
            else
            {
                query = options.Predicate != null
                    ? query.Where(options.Predicate)
                    : query;
            }

            if (options.OrderBy != null) query = options.OrderBy(query);
            if (options.Skip.HasValue) query = query.Skip(options.Skip.Value);
            if (options.Take.HasValue) query = query.Take(options.Take.Value);

            if (options.Selector != null)
                // Explicitly specify the type arguments for the Select method to resolve CS0411  
                query = query.Select(options.Selector);

            return query;
        }
    }
}