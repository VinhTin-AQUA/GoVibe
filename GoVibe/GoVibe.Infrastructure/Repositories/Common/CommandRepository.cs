using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.Infrastructure.Repositories.Common
{
    public interface ICommandRepository<T> : IDisposable where T : Entity
    {
        Task AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);

        Task UpdateAsync(T entity);

        Task UpdateRangeAsync(IEnumerable<T> entities);

        Task DeleteAsync(T entity);

        Task DeleteAsync(Guid id);

        Task DeleteRangeAsync(IEnumerable<T> entities);

        Task<int> DeleteRangeAsync(IEnumerable<Guid> ids);

        Task<bool> SaveChangesAsync();
    }
    
    public class CommandRepository<T> : ICommandRepository<T>, IAsyncDisposable where T : Entity
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        private bool disposed;

        public CommandRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public ValueTask DisposeAsync()
        {
            Dispose();
            return ValueTask.CompletedTask;
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            var enumerable = entities.ToList();
            foreach (var entity in enumerable) entity.CreatedAt = entity.UpdatedAt = DateTime.Now;

            await _dbSet.AddRangeAsync(enumerable);
        }
        
        public Task UpdateAsync(T entity)
        {
            entity.UpdatedAt = DateTime.Now;
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }
        
        public Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            var enumerable = entities.ToList();
            foreach (var entity in enumerable) entity.UpdatedAt = DateTime.Now;
            _dbSet.UpdateRange(enumerable);
            return Task.CompletedTask;
        }
        
        public Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }
        
        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null) _dbSet.Remove(entity);
        }

        public Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            return Task.CompletedTask;
        }
        
        public async Task<int> DeleteRangeAsync(IEnumerable<Guid> ids)
        {
            return await _dbSet
                .Where(e => ids.Contains(e.Id))
                .ExecuteDeleteAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
                if (disposing)
                    _context.Dispose();

            disposed = true;
        }
    }
}