using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITransferRequestRepository : IRepository<TransferRequest>
    {
        /// <summary>
        /// Gets all transfer requests made by a specific requester
        /// </summary>
        Task<IEnumerable<TransferRequest>> GetByRequesterIdAsync(Guid requesterId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all transfer requests targeting specific departments
        /// </summary>
        Task<IEnumerable<TransferRequest>> GetByTargetDepartmentIdsAsync(IEnumerable<Guid> departmentIds, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all pending transfer requests for specific departments
        /// </summary>
        Task<IEnumerable<TransferRequest>> GetPendingByTargetDepartmentIdsAsync(IEnumerable<Guid> departmentIds, CancellationToken cancellationToken = default);

        /// <summary>
        /// Filters transfer requests using Dynamic LINQ
        /// </summary>
        Task<IEnumerable<TransferRequest>> FilterAsync(string query, CancellationToken cancellationToken = default);
    }
}
