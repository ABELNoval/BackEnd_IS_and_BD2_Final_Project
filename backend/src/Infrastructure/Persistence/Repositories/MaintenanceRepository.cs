using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class MaintenanceRepository : BaseRepository<Maintenance>, IMaintenanceRepository
{
    public MaintenanceRepository(AppDbContext context) : base(context)
    {
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
            .OrderByDescending(m => m.MaintenanceDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Maintenance>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Maintenances
            .Where(m => m.MaintenanceDate >= startDate && m.MaintenanceDate <= endDate)
            .OrderByDescending(m => m.MaintenanceDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Maintenance>> GetByCostRangeAsync(decimal minCost, decimal maxCost, CancellationToken cancellationToken = default)
    {
        return await _context.Maintenances
            .Where(m => m.Cost >= minCost && m.Cost <= maxCost)
            .OrderByDescending(m => m.Cost)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Maintenance>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Maintenances
            .OrderByDescending(m => m.MaintenanceDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Maintenance?> GetLatestByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Maintenances
            .Where(m => m.EquipmentId == equipmentId)
            .OrderByDescending(m => m.MaintenanceDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalCostByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Maintenances
            .Where(m => m.EquipmentId == equipmentId)
            .SumAsync(m => m.Cost, cancellationToken);
    }

    public async Task<decimal> GetTotalCostByTechnicalIdAsync(Guid technicalId, CancellationToken cancellationToken = default)
    {
        return await _context.Maintenances
            .Where(m => m.TechnicalId == technicalId)
            .SumAsync(m => m.Cost, cancellationToken);
    }

    public async Task<int> CountByMaintenanceTypeIdAsync(int maintenanceTypeId, CancellationToken cancellationToken = default)
    {
        return await _context.Maintenances
            .CountAsync(m => m.MaintenanceTypeId == maintenanceTypeId, cancellationToken);
    }

    public async Task<IEnumerable<Maintenance>> GetByTechnicalAndDateRangeAsync(Guid technicalId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Maintenances
            .Where(m => m.TechnicalId == technicalId && 
                       m.MaintenanceDate >= startDate && 
                       m.MaintenanceDate <= endDate)
            .OrderByDescending(m => m.MaintenanceDate)
            .ToListAsync(cancellationToken);
    }
}