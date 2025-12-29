using Domain.Entities;
using Domain.Enumerations;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class TransferRequestRepository : BaseRepository<TransferRequest>, ITransferRequestRepository
    {
        public TransferRequestRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<TransferRequest>> GetByRequesterIdAsync(Guid requesterId, CancellationToken cancellationToken = default)
        {
            return await _context.TransferRequests
                .Where(tr => tr.RequesterId == requesterId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TransferRequest>> GetByTargetDepartmentIdsAsync(IEnumerable<Guid> departmentIds, CancellationToken cancellationToken = default)
        {
            var deptList = departmentIds.ToList();
            return await _context.TransferRequests
                .Where(tr => deptList.Contains(tr.TargetDepartmentId))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TransferRequest>> GetPendingByTargetDepartmentIdsAsync(IEnumerable<Guid> departmentIds, CancellationToken cancellationToken = default)
        {
            var deptList = departmentIds.ToList();
            var pendingStatusId = TransferRequestStatus.Pending.Id;

            return await _context.TransferRequests
                .Where(tr => deptList.Contains(tr.TargetDepartmentId) && tr.StatusId == pendingStatusId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TransferRequest>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            IQueryable<TransferRequest> baseQuery = _context.TransferRequests;

            if (!string.IsNullOrWhiteSpace(query))
            {
                baseQuery = baseQuery.Where(query);
            }

            return await baseQuery.ToListAsync(cancellationToken);
        }
    }
}
