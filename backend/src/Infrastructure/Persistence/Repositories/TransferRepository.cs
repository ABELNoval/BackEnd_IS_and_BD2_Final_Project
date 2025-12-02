using Domain.Entities;
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
    public class TransferRepository : BaseRepository<Transfer>, ITransferRepository
    {
        public TransferRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Transfer>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            IQueryable<Transfer> baseQuery = _context.Transfers;

            if (!string.IsNullOrWhiteSpace(query))
            {
                baseQuery = baseQuery.Where(query);
            }

            return await baseQuery.ToListAsync(cancellationToken);
        }

        public async Task<(IEnumerable<Transfer> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            IQueryable<Transfer> baseQuery = _context.Transfers;

            if (!string.IsNullOrWhiteSpace(query))
            {
                baseQuery = baseQuery.Where(query);
            }

            var total = await baseQuery.CountAsync(cancellationToken);
            var pages = (int)Math.Ceiling(total / (double)pageSize);

            var data = await baseQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (data, total, pages);
        }

        public async Task<IEnumerable<Transfer>> GetByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default)
        {
            return await _context.Transfers
                .Where(t => t.EquipmentId == equipmentId)
                .OrderByDescending(t => t.TransferDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Transfer>> GetBySourceDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            return await _context.Transfers
                .Where(t => t.SourceDepartmentId == departmentId)
                .OrderByDescending(t => t.TransferDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Transfer>> GetByTargetDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            return await _context.Transfers
                .Where(t => t.TargetDepartmentId == departmentId)
                .OrderByDescending(t => t.TransferDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Transfer>> GetByResponsibleIdAsync(Guid responsibleId, CancellationToken cancellationToken = default)
        {
            return await _context.Transfers
                .Where(t => t.ResponsibleId == responsibleId)
                .OrderByDescending(t => t.TransferDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Transfer>> GetByDateRangeAsync(
            DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _context.Transfers
                .Where(t => t.TransferDate >= startDate && t.TransferDate <= endDate)
                .OrderByDescending(t => t.TransferDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Transfer>> GetTransfersInvolvingDepartmentAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            return await _context.Transfers
                .Where(t => t.SourceDepartmentId == departmentId || t.TargetDepartmentId == departmentId)
                .OrderByDescending(t => t.TransferDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Transfer>> GetEquipmentTransferHistoryAsync(Guid equipmentId, CancellationToken cancellationToken = default)
        {
            return await _context.Transfers
                .Where(t => t.EquipmentId == equipmentId)
                .OrderBy(t => t.TransferDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<Dictionary<Guid, int>> GetOutgoingTransferCountByDepartmentAsync(CancellationToken cancellationToken = default)
        {
            var departments = await _context.Departments.ToListAsync(cancellationToken);
            var result = new Dictionary<Guid, int>();

            foreach (var department in departments)
            {
                var count = await _context.Transfers
                    .CountAsync(t => t.SourceDepartmentId == department.Id, cancellationToken);
                result[department.Id] = count;
            }

            return result;
        }

        public async Task<Dictionary<Guid, int>> GetIncomingTransferCountByDepartmentAsync(CancellationToken cancellationToken = default)
        {
            var departments = await _context.Departments.ToListAsync(cancellationToken);
            var result = new Dictionary<Guid, int>();

            foreach (var department in departments)
            {
                var count = await _context.Transfers
                    .CountAsync(t => t.TargetDepartmentId == department.Id, cancellationToken);
                result[department.Id] = count;
            }

            return result;
        }

        public async Task<Transfer?> GetLatestTransferForEquipmentAsync(Guid equipmentId, CancellationToken cancellationToken = default)
        {
            return await _context.Transfers
                .Where(t => t.EquipmentId == equipmentId)
                .OrderByDescending(t => t.TransferDate)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Transfer>> GetRecentTransfersAsync(int days = 30, CancellationToken cancellationToken = default)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return await _context.Transfers
                .Where(t => t.TransferDate >= cutoffDate)
                .OrderByDescending(t => t.TransferDate)
                .ToListAsync(cancellationToken);
        }
    }
}