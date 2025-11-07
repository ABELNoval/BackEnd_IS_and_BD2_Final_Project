using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class TechnicalRepository : BaseRepository<Technical>, ITechnicalRepository
{
    public TechnicalRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Technical?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Technicals
            .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task<Technical?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Technicals
            .FirstOrDefaultAsync(t => t.Email.Value.ToLower() == email.ToLower(), cancellationToken);
    }

    public async Task<IEnumerable<Technical>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Technicals
            .OrderBy(t => t.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Technicals
            .AnyAsync(t => t.Email.Value.ToLower() == email.ToLower(), cancellationToken);
    }

    public async Task<IEnumerable<Technical>> GetBySpecialtyAsync(string specialty, CancellationToken cancellationToken = default)
    {
        return await _context.Technicals
            .Where(t => t.Specialty.ToLower() == specialty.ToLower())
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Technical>> GetByMinimumExperienceAsync(int minExperience, CancellationToken cancellationToken = default)
    {
        return await _context.Technicals
            .Where(t => t.Experience >= minExperience)
            .OrderByDescending(t => t.Experience)
            .ThenBy(t => t.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Technical>> GetByExperienceRangeAsync(int minExperience, int maxExperience, CancellationToken cancellationToken = default)
    {
        return await _context.Technicals
            .Where(t => t.Experience >= minExperience && t.Experience <= maxExperience)
            .OrderByDescending(t => t.Experience)
            .ThenBy(t => t.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Technical?> GetByIdWithAssessmentsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Technicals
            .Include(t => t.Assessments)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Technical>> GetTechnicalsWithMaintenanceAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Technicals
            .Where(t => _context.Maintenances.Any(m => m.TechnicalId == t.Id))
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetMaintenanceCountAsync(Guid technicalId, CancellationToken cancellationToken = default)
    {
        return await _context.Maintenances
            .CountAsync(m => m.TechnicalId == technicalId, cancellationToken);
    }

    public async Task<int> GetAssessmentCountAsync(Guid technicalId, CancellationToken cancellationToken = default)
    {
        return await _context.Assessments
            .CountAsync(a => a.TechnicalId == technicalId, cancellationToken);
    }

    public async Task<decimal?> GetAverageAssessmentScoreAsync(Guid technicalId, CancellationToken cancellationToken = default)
    {
        var hasAssessments = await _context.Assessments
            .AnyAsync(a => a.TechnicalId == technicalId, cancellationToken);

        if (!hasAssessments)
            return null;

        return await _context.Assessments
            .Where(a => a.TechnicalId == technicalId)
            .AverageAsync(a => a.Score.Value, cancellationToken);
    }
}