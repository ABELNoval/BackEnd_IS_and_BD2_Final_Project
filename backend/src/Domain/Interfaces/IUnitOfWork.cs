using System;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
	/// <summary>
	/// Unit of Work abstraction exposing SaveChanges and transaction control.
	/// </summary>
	public interface IUnitOfWork
	{
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

		Task BeginTransactionAsync(CancellationToken cancellationToken = default);
		Task CommitAsync();
		Task RollbackAsync();

		/// <summary>
		/// Executes the provided operation inside a database transaction. Commits if the operation
		/// completes successfully (and SaveChanges is called inside or automatically), otherwise rolls back.
		/// </summary>
		Task ExecuteInTransactionAsync(Func<Task> operation, CancellationToken cancellationToken = default);

		/// <summary>
		/// Executes the provided operation inside a database transaction and returns the result.
		/// </summary>
		Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default);
	}
}

