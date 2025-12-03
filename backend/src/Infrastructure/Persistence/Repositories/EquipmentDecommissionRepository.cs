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
    public class EquipmentDecommissionRepository : BaseRepository<EquipmentDecommission>, IEquipmentDecommissionRepository
    {
        public EquipmentDecommissionRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<EquipmentDecommission>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            IQueryable<EquipmentDecommission> baseQuery = _context.EquipmentDecommissions;

            if (!string.IsNullOrWhiteSpace(query))
            {
                baseQuery = baseQuery.Where(query);
            }

            return await baseQuery.ToListAsync(cancellationToken);
        }

        public async Task<(IEnumerable<EquipmentDecommission> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            IQueryable<EquipmentDecommission> baseQuery = _context.EquipmentDecommissions;

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

        public async Task<IEnumerable<EquipmentDecommission>> GetByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default)
        {
            return await _context.EquipmentDecommissions
                .Where(ed => ed.EquipmentId == equipmentId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<EquipmentDecommission>> GetByTechnicalIdAsync(Guid technicalId, CancellationToken cancellationToken = default)
        {
            return await _context.EquipmentDecommissions
                .Where(ed => ed.TechnicalId == technicalId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<EquipmentDecommission>> GetByDestinyTypeIdAsync(int destinyTypeId, CancellationToken cancellationToken = default)
        {
            return await _context.EquipmentDecommissions
                .Where(ed => ed.DestinyTypeId == destinyTypeId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<EquipmentDecommission>> GetByDateRangeAsync(
            DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _context.EquipmentDecommissions
                .Where(ed => ed.DecommissionDate >= startDate && ed.DecommissionDate <= endDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<EquipmentDecommission>> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            return await _context.EquipmentDecommissions
                .Where(ed => ed.DepartmentId == departmentId)
                .ToListAsync(cancellationToken);
        }

        public async Task<Dictionary<int, int>> GetCountByDestinyTypeAsync(CancellationToken cancellationToken = default)
        {
            var result = new Dictionary<int, int>();
            var destinyTypes = new[] { 1, 2, 3 }; // IDs de DestinyType

            foreach (var typeId in destinyTypes)
            {
                var count = await _context.EquipmentDecommissions
                    .CountAsync(ed => ed.DestinyTypeId == typeId, cancellationToken);
                result[typeId] = count;
            }

            return result;
        }

        public async Task<IEnumerable<EquipmentDecommission>> SearchByReasonAsync(string reason, CancellationToken cancellationToken = default)
        {
            return await _context.EquipmentDecommissions
                .Where(ed => ed.Reason.Contains(reason))
                .ToListAsync(cancellationToken);
        }
    }
}