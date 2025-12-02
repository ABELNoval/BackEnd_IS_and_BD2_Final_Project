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
    public class EquipmentRepository : BaseRepository<Equipment>, IEquipmentRepository
    {
        public EquipmentRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Equipment>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            IQueryable<Equipment> baseQuery = _context.Equipments;

            if (!string.IsNullOrWhiteSpace(query))
            {
                baseQuery = baseQuery.Where(query);
            }

            return await baseQuery.ToListAsync(cancellationToken);
        }

        public async Task<(IEnumerable<Equipment> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            IQueryable<Equipment> baseQuery = _context.Equipments;

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

        public async Task<IEnumerable<Equipment>> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            return await _context.Equipments
                .Where(e => e.DepartmentId == departmentId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Equipment>> GetByEquipmentTypeIdAsync(Guid equipmentTypeId, CancellationToken cancellationToken = default)
        {
            return await _context.Equipments
                .Where(e => e.EquipmentTypeId == equipmentTypeId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Equipment>> GetByStateIdAsync(int stateId, CancellationToken cancellationToken = default)
        {
            return await _context.Equipments
                .Where(e => e.StateId == stateId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Equipment>> GetByLocationTypeIdAsync(int locationTypeId, CancellationToken cancellationToken = default)
        {
            return await _context.Equipments
                .Where(e => e.LocationTypeId == locationTypeId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Equipment>> GetEquipmentNeedingMaintenanceAsync(CancellationToken cancellationToken = default)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-180);
            
            return await _context.Equipments
                .Where(e => e.StateId != 4) // No disposed
                .Where(e => !_context.Maintenances.Any(m => 
                    m.EquipmentId == e.Id && m.MaintenanceDate >= cutoffDate))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Equipment>> GetOperativeEquipmentAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Equipments
                .Where(e => e.StateId == 1) // Operative state
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Equipment>> GetDecommissionedEquipmentAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Equipments
                .Where(e => e.StateId == 3) // Decommissioned state
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Equipment>> GetDisposedEquipmentAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Equipments
                .Where(e => e.StateId == 4) // Disposed state
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Equipment>> GetByAcquisitionDateRangeAsync(
            DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _context.Equipments
                .Where(e => e.AcquisitionDate >= startDate && e.AcquisitionDate <= endDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Equipment>> SearchByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Equipments
                .Where(e => e.Name.Contains(name))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Equipment>> GetEquipmentWithTransferHistoryAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Equipments
                .Where(e => _context.Transfers.Any(t => t.EquipmentId == e.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Equipment>> GetEquipmentWithMaintenanceHistoryAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Equipments
                .Where(e => _context.Maintenances.Any(m => m.EquipmentId == e.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasPendingMaintenanceAsync(Guid equipmentId, CancellationToken cancellationToken = default)
        {
            var equipment = await GetByIdAsync(equipmentId, cancellationToken);
            if (equipment == null) return false;

            return equipment.StateId == 2;
        }

        public async Task<Dictionary<int, int>> GetCountByStateAsync(CancellationToken cancellationToken = default)
        {
            var result = new Dictionary<int, int>();

            var states = new[] { 1, 2, 3, 4 };
            
            foreach (var stateId in states)
            {
                var count = await _context.Equipments
                    .CountAsync(e => e.StateId == stateId, cancellationToken);
                result[stateId] = count;
            }

            return result;
        }
    }
}