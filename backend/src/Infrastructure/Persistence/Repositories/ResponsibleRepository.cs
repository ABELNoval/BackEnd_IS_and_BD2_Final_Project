using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ResponsibleRepository : BaseRepository<Responsible>, IResponsibleRepository
{
    public ResponsibleRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Responsible?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Responsibles
            .FirstOrDefaultAsync(r => r.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task<Responsible?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Responsibles
            .FirstOrDefaultAsync(r => r.Email.Value.ToLower() == email.ToLower(), cancellationToken);
    }

    public async Task<IEnumerable<Responsible>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Responsibles
            .OrderBy(r => r.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Responsibles
            .AnyAsync(r => r.Email.Value.ToLower() == email.ToLower(), cancellationToken);
    }

    public async Task<Responsible?> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
    {
        var department = await _context.Departments
            .FirstOrDefaultAsync(d => d.Id == departmentId, cancellationToken);

        if (department?.ResponsibleId == null)
            return null;

        return await _context.Responsibles
            .FirstOrDefaultAsync(r => r.Id == department.ResponsibleId, cancellationToken);
    }

    public async Task<bool> IsManagingDepartmentAsync(Guid responsibleId, CancellationToken cancellationToken = default)
    {
        return await _context.Departments
            .AnyAsync(d => d.ResponsibleId == responsibleId, cancellationToken);
    }

    public async Task<IEnumerable<Responsible>> GetResponsiblesWithTransfersAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Responsibles
            .Where(r => _context.Transfers.Any(t => t.ResponsibleId == r.Id))
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTransferCountAsync(Guid responsibleId, CancellationToken cancellationToken = default)
    {
        return await _context.Transfers
            .CountAsync(t => t.ResponsibleId == responsibleId, cancellationToken);
    }
}