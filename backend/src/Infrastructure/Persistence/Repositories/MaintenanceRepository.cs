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
    public class MaintenanceRepository : BaseRepository<Maintenance>, IMaintenanceRepository
    {
        public MaintenanceRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Maintenance>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            IQueryable<Maintenance> baseQuery = _context.Maintenances;

            if (!string.IsNullOrWhiteSpace(query))
            {
                baseQuery = baseQuery.Where(query);
            }

            return await baseQuery.ToListAsync(cancellationToken);
        }

        public async Task<(IEnumerable<Maintenance> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            IQueryable<Maintenance> baseQuery = _context.Maintenances;

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

        public async Task<IEnumerable<Maintenance>> GetByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default)
        {
            return await _context.Maintenances
                .Where(m => m.EquipmentId == equipmentId)
                .OrderByDescending(m => m.MaintenanceDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Maintenance>> GetByTechnicalIdAsync(Guid technicalId, CancellationToken cancellationToken = default)
        {
            return await _context.Maintenances
                .Where(m => m.TechnicalId == technicalId)
                .OrderByDescending(m => m.MaintenanceDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Maintenance>> GetByMaintenanceTypeIdAsync(int maintenanceTypeId, CancellationToken cancellationToken = default)
        {
            return await _context.Maintenances
                .Where(m => m.MaintenanceTypeId == maintenanceTypeId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Maintenance>> GetByDateRangeAsync(
            DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _context.Maintenances
                .Where(m => m.MaintenanceDate >= startDate && m.MaintenanceDate <= endDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Maintenance>> GetByCostGreaterThanAsync(decimal minCost, CancellationToken cancellationToken = default)
        {
            return await _context.Maintenances
                .Where(m => m.Cost > minCost)
                .ToListAsync(cancellationToken);
        }

        public async Task<Maintenance?> GetLatestMaintenanceForEquipmentAsync(Guid equipmentId, CancellationToken cancellationToken = default)
        {
            return await _context.Maintenances
                .Where(m => m.EquipmentId == equipmentId)
                .OrderByDescending(m => m.MaintenanceDate)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Dictionary<Guid, decimal>> GetTotalMaintenanceCostByEquipmentAsync(CancellationToken cancellationToken = default)
        {
            var result = new Dictionary<Guid, decimal>();
            
            var equipmentIds = await _context.Equipments
                .Select(e => e.Id)
                .ToListAsync(cancellationToken);

            foreach (var equipmentId in equipmentIds)
            {
                var totalCost = await _context.Maintenances
                    .Where(m => m.EquipmentId == equipmentId)
                    .SumAsync(m => m.Cost, cancellationToken);
                
                result[equipmentId] = totalCost;
            }

            return result;
        }

        public async Task<Dictionary<int, int>> GetCountByMaintenanceTypeAsync(CancellationToken cancellationToken = default)
        {
            var result = new Dictionary<int, int>();
            
            // Tipos de mantenimiento (basado en MaintenanceType enum)
            var types = new[] { 1, 2, 3 }; // Preventive, Corrective, Predictive
            
            foreach (var typeId in types)
            {
                var count = await _context.Maintenances
                    .CountAsync(m => m.MaintenanceTypeId == typeId, cancellationToken);
                result[typeId] = count;
            }

            return result;
        }

        public async Task<IEnumerable<Maintenance>> GetMaintenancesByTechnicalInPeriodAsync(
            Guid technicalId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _context.Maintenances
                .Where(m => m.TechnicalId == technicalId)
                .Where(m => m.MaintenanceDate >= startDate && m.MaintenanceDate <= endDate)
                .OrderByDescending(m => m.MaintenanceDate)
                .ToListAsync(cancellationToken);
        }
    }
}