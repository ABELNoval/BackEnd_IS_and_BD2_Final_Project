using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class TransferRepository : BaseRepository<Transfer>, ITransferRepository
{
    public TransferRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Transfer>> GetByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Transfers
            .Where(t => t.EquipmentId == equipmentId)
            .OrderByDescending(t => t.TransferDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transfer>> GetBySourceDepartmentIdAsync(Guid sourceDepartmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Transfers
            .Where(t => t.SourceDepartmentId == sourceDepartmentId)
            .OrderByDescending(t => t.TransferDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transfer>> GetByTargetDepartmentIdAsync(Guid targetDepartmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Transfers
            .Where(t => t.TargetDepartmentId == targetDepartmentId)
            .OrderByDescending(t => t.TransferDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transfer>> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Transfers
            .Where(t => t.SourceDepartmentId == departmentId || t.TargetDepartmentId == departmentId)
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

    public async Task<IEnumerable<Transfer>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Transfers
            .Where(t => t.TransferDate >= startDate && t.TransferDate <= endDate)
            .OrderByDescending(t => t.TransferDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transfer>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Transfers
            .OrderByDescending(t => t.TransferDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Transfer?> GetLatestByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Transfers
            .Where(t => t.EquipmentId == equipmentId)
            .OrderByDescending(t => t.TransferDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transfer>> GetByDepartmentPairAsync(Guid sourceDepartmentId, Guid targetDepartmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Transfers
            .Where(t => t.SourceDepartmentId == sourceDepartmentId && t.TargetDepartmentId == targetDepartmentId)
            .OrderByDescending(t => t.TransferDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Transfers
            .CountAsync(t => t.EquipmentId == equipmentId, cancellationToken);
    }

    public async Task<int> CountByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Transfers
            .CountAsync(t => t.SourceDepartmentId == departmentId || t.TargetDepartmentId == departmentId, cancellationToken);
    }

    public async Task<IEnumerable<Transfer>> GetRecentTransfersAsync(int count, CancellationToken cancellationToken = default)
    {
        return await _context.Transfers
            .OrderByDescending(t => t.TransferDate)
            .Take(count)
            .ToListAsync(cancellationToken);
    }
}