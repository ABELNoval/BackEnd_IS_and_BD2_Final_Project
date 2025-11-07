using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class SectionRepository : BaseRepository<Section>, ISectionRepository
{
    public SectionRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Section?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Sections
            .FirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task<IEnumerable<Section>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Sections
            .OrderBy(s => s.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Sections
            .AnyAsync(s => s.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task<IEnumerable<Section>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.Sections
            .Where(s => s.Name.ToLower().Contains(searchTerm.ToLower()))
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetDepartmentCountBySectionIdAsync(Guid sectionId, CancellationToken cancellationToken = default)
    {
        return await _context.Departments
            .CountAsync(d => d.SectionId == sectionId, cancellationToken);
    }

    public async Task<IEnumerable<Section>> GetAllOrderedByNameAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Sections
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasDepartmentsAsync(Guid sectionId, CancellationToken cancellationToken = default)
    {
        return await _context.Departments
            .AnyAsync(d => d.SectionId == sectionId, cancellationToken);
    }
}