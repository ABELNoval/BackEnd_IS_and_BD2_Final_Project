using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class EquipmentDecommissionRepository : BaseRepository<EquipmentDecommission>, IEquipmentDecommissionRepository
{
    public EquipmentDecommissionRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<EquipmentDecommission>> GetByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default)
    {
        return await _context.EquipmentDecommissions
            .Where(ed => ed.EquipmentId == equipmentId)
            .OrderByDescending(ed => ed.DecommissionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<EquipmentDecommission>> GetByTechnicalIdAsync(Guid technicalId, CancellationToken cancellationToken = default)
    {
        return await _context.EquipmentDecommissions
            .Where(ed => ed.TechnicalId == technicalId)
            .OrderByDescending(ed => ed.DecommissionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<EquipmentDecommission>> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
    {
        return await _context.EquipmentDecommissions
            .Where(ed => ed.DepartmentId == departmentId)
            .OrderByDescending(ed => ed.DecommissionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<EquipmentDecommission>> GetByDestinyTypeIdAsync(int destinyTypeId, CancellationToken cancellationToken = default)
    {
        return await _context.EquipmentDecommissions
            .Where(ed => ed.DestinyTypeId == destinyTypeId)
            .OrderByDescending(ed => ed.DecommissionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<EquipmentDecommission>> GetByRecipientIdAsync(Guid recipientId, CancellationToken cancellationToken = default)
    {
        return await _context.EquipmentDecommissions
            .Where(ed => ed.RecipientId == recipientId)
            .OrderByDescending(ed => ed.DecommissionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<EquipmentDecommission>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.EquipmentDecommissions
            .Where(ed => ed.DecommissionDate >= startDate && ed.DecommissionDate <= endDate)
            .OrderByDescending(ed => ed.DecommissionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<EquipmentDecommission>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.EquipmentDecommissions
            .OrderByDescending(ed => ed.DecommissionDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<EquipmentDecommission?> GetLatestByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default)
    {
        return await _context.EquipmentDecommissions
            .Where(ed => ed.EquipmentId == equipmentId)
            .OrderByDescending(ed => ed.DecommissionDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> HasDecommissionsAsync(Guid equipmentId, CancellationToken cancellationToken = default)
    {
        return await _context.EquipmentDecommissions
            .AnyAsync(ed => ed.EquipmentId == equipmentId, cancellationToken);
    }
}