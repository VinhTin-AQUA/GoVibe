using System.Linq.Expressions;
using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.Infrastructure.Repositories.Common
{
    public interface IQueryRepository<T> where T : Entity
    {
        Task<T?> GetByIdAsync(Guid id, bool tracking = false, params Expression<Func<T, object?>>[] includes);

        Task<List<T>> GetByIdsAsync(IEnumerable<Guid> ids, bool tracking = false, params Expression<Func<T, object?>>[] includes);

        Task<List<T>> GetAllAsync(bool tracking = false, params Expression<Func<T, object?>>[] includes);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object?>>[] includes);

        Task<int> CountAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object?>>[] includes);

        Task<List<T>> GetPagedAsync(int page, int pageSize, params Expression<Func<T, object?>>[] includes);

        Task ExecuteRawSqlAsync(string sql, params object[] parameters);
    }

    public class QueryRepository<T> : IQueryRepository<T> where T : Entity
    {
        protected readonly IDbContextFactory<AppDbContext> _contextFactory;

        public QueryRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<T?> GetByIdAsync(Guid id, bool tracking = false, params Expression<Func<T, object?>>[] includes)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var query = BuildQuery(context, includes);

            if (tracking)
                return await query.FirstOrDefaultAsync(x => x.Id == id);

            return await query.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<T>> GetByIdsAsync(IEnumerable<Guid> ids, bool tracking = false, params Expression<Func<T, object?>>[] includes)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var query = BuildQuery(context, includes);

            if (tracking)
                return await query.Where(x => ids.Contains(x.Id)).ToListAsync();

            return await query.AsNoTracking().Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(bool tracking = false, params Expression<Func<T, object?>>[] includes)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var query = BuildQuery(context, includes);

            if (tracking)
                return await query.ToListAsync();

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object?>>[] includes)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var query = BuildQuery(context, includes);
            return await query.AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object?>>[] includes)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var query = BuildQuery(context, includes);
            return await query.CountAsync(predicate);
        }

        public async Task<List<T>> GetPagedAsync(int page, int pageSize, params Expression<Func<T, object?>>[] includes)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var query = BuildQuery(context, includes);
            return await query
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

        // var orders = BuildQuery(context, x => x.Customer.Address, x => x.OrderItems.Select(i => i.Product));
        public IQueryable<T> BuildQuery(AppDbContext context, params Expression<Func<T, object?>>[] includes)
        {
            IQueryable<T> query = context.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query;
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
                query = query.Select(options.Selector);

            return query;
        }
    }
}