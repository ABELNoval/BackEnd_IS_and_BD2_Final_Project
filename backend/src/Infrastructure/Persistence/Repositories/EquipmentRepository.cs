using Domain.Entities;
using Domain.Enumerations;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class EquipmentRepository : BaseRepository<Equipment>, IEquipmentRepository
{
    public EquipmentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Equipment?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Equipments
            .FirstOrDefaultAsync(e => e.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task<IEnumerable<Equipment>> GetByEquipmentTypeIdAsync(Guid equipmentTypeId, CancellationToken cancellationToken = default)
    {
        return await _context.Equipments
            .Where(e => e.EquipmentTypeId == equipmentTypeId)
            .OrderBy(e => e.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Equipment>> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Equipments
            .Where(e => e.DepartmentId == departmentId)
            .OrderBy(e => e.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Equipment>> GetByStateAsync(int stateId, CancellationToken cancellationToken = default)
    {
        return await _context.Equipments
            .Where(e => e.State.Id == stateId)
            .OrderBy(e => e.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Equipment>> GetByLocationTypeAsync(int locationTypeId, CancellationToken cancellationToken = default)
    {
        return await _context.Equipments
            .Where(e => e.LocationType.Id == locationTypeId)
            .OrderBy(e => e.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Equipment>> GetByAcquisitionDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Equipments
            .Where(e => e.AcquisitionDate >= startDate && e.AcquisitionDate <= endDate)
            .OrderByDescending(e => e.AcquisitionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Equipment>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Equipments
            .OrderBy(e => e.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Equipment?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Equipments
            .Include(e => e.Decommissions)
            .Include(e => e.Transfers)
            .Include(e => e.Maintenances)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Equipment>> GetOperativeByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Equipments
            .Where(e => e.DepartmentId == departmentId && e.State.Id == EquipmentState.Operative.Id)
            .OrderBy(e => e.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Equipment>> GetEquipmentUnderMaintenanceAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Equipments
            .Where(e => e.State.Id == EquipmentState.UnderMaintenance.Id)
            .OrderBy(e => e.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Equipment>> GetWarehouseEquipmentAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Equipments
            .Where(e => e.LocationType.Id == LocationType.Warehouse.Id)
            .OrderBy(e => e.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Equipments
            .AnyAsync(e => e.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task<int> CountByStateAsync(int stateId, CancellationToken cancellationToken = default)
    {
        return await _context.Equipments
            .CountAsync(e => e.State.Id == stateId, cancellationToken);
    }
}