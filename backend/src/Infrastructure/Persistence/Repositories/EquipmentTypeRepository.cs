using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class EquipmentTypeRepository : BaseRepository<EquipmentType>, IEquipmentTypeRepository
{
    public EquipmentTypeRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<EquipmentType?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.EquipmentTypes
            .FirstOrDefaultAsync(et => et.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task<IEnumerable<EquipmentType>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.EquipmentTypes
            .OrderBy(et => et.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.EquipmentTypes
            .AnyAsync(et => et.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task<IEnumerable<EquipmentType>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.EquipmentTypes
            .Where(et => et.Name.ToLower().Contains(searchTerm.ToLower()))
            .OrderBy(et => et.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetEquipmentCountByTypeIdAsync(Guid equipmentTypeId, CancellationToken cancellationToken = default)
    {
        return await _context.Equipments
            .CountAsync(e => e.EquipmentTypeId == equipmentTypeId, cancellationToken);
    }

    public async Task<IEnumerable<EquipmentType>> GetAllOrderedByNameAsync(CancellationToken cancellationToken = default)
    {
        return await _context.EquipmentTypes
            .OrderBy(et => et.Name)
            .ToListAsync(cancellationToken);
    }
}