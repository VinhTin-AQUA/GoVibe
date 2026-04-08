using GoVibe.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace GoVibe.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
    
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task BeginTransactionAsync()
        {
            _transaction ??= await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction == null) throw new InvalidOperationException("The transaction has not been started.");

            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync(); 
                _transaction = null;              
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction == null) return;

            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                await _transaction.DisposeAsync(); 
                _transaction = null;       
            }
        }

        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        protected virtual void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();

            if (!disposing) return;

            _transaction?.Dispose();
            _context.Dispose();
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}