using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistence
{
    /// <summary>
    /// UnitOfWork that delegates SaveChanges to AppDbContext and provides transaction helpers.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
                return; // already in a transaction

            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitAsync()
        {
            if (_transaction == null)
                return;

            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackAsync()
        {
            if (_transaction == null)
                return;

            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task ExecuteInTransactionAsync(Func<Task> operation, CancellationToken cancellationToken = default)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));

            await BeginTransactionAsync(cancellationToken);
            try
            {
                await operation();
                await CommitAsync();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));

            await BeginTransactionAsync(cancellationToken);
            try
            {
                var result = await operation();
                await CommitAsync();
                return result;
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
        }
    }
}
