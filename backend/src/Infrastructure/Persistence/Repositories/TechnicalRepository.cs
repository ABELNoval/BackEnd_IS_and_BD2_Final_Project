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
    public class TechnicalRepository : BaseRepository<Technical>, ITechnicalRepository
    {
        public TechnicalRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Technical>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            IQueryable<Technical> baseQuery = _context.Technicals;

            if (!string.IsNullOrWhiteSpace(query))
            {
                baseQuery = baseQuery.Where(query);
            }

            return await baseQuery.ToListAsync(cancellationToken);
        }

        public async Task<(IEnumerable<Technical> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            IQueryable<Technical> baseQuery = _context.Technicals;

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

        public async Task<IEnumerable<Technical>> SearchByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Technicals
                .Where(t => t.Name.Contains(name))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Technical>> GetBySpecialtyAsync(string specialty, CancellationToken cancellationToken = default)
        {
            return await _context.Technicals
                .Where(t => t.Specialty.Contains(specialty))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Technical>> GetByMinExperienceAsync(int minExperience, CancellationToken cancellationToken = default)
        {
            return await _context.Technicals
                .Where(t => t.Experience >= minExperience)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Technical>> GetTechnicalsWithAssessmentsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Technicals
                .Where(t => _context.Assessments.Any(a => a.TechnicalId == t.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Technical>> GetTechnicalsWithoutAssessmentsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Technicals
                .Where(t => !_context.Assessments.Any(a => a.TechnicalId == t.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<Dictionary<Guid, decimal>> GetAverageScoreByTechnicalAsync(CancellationToken cancellationToken = default)
        {
            var technicals = await _context.Technicals.ToListAsync(cancellationToken);
            var result = new Dictionary<Guid, decimal>();

            foreach (var technical in technicals)
            {
                var assessments = await _context.Assessments
                    .Where(a => a.TechnicalId == technical.Id)
                    .ToListAsync(cancellationToken);
                
                result[technical.Id] = assessments.Any() 
                    ? assessments.Average(a => a.Score.Value) 
                    : 0;
            }

            return result;
        }

        public async Task<IEnumerable<Technical>> GetTechnicalsWithMaintenancesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Technicals
                .Where(t => _context.Maintenances.Any(m => m.TechnicalId == t.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Technical>> GetTechnicalsWithDecommissionsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Technicals
                .Where(t => _context.EquipmentDecommissions.Any(ed => ed.TechnicalId == t.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Technical>> GetActiveTechnicalsAsync(DateTime? fromDate = null, CancellationToken cancellationToken = default)
        {
            fromDate ??= DateTime.UtcNow.AddDays(-30);

            return await _context.Technicals
                .Where(t => _context.Maintenances.Any(m => m.TechnicalId == t.Id && m.MaintenanceDate >= fromDate) ||
                           _context.EquipmentDecommissions.Any(ed => ed.TechnicalId == t.Id && ed.DecommissionDate >= fromDate))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Technical>> GetByEmailDomainAsync(string domain, CancellationToken cancellationToken = default)
        {
            return await _context.Technicals
                .Where(t => t.Email.Value.EndsWith($"@{domain}"))
                .ToListAsync(cancellationToken);
        }

        public async Task<Dictionary<Guid, int>> GetActivityCountByTechnicalAsync(CancellationToken cancellationToken = default)
        {
            var technicals = await _context.Technicals.ToListAsync(cancellationToken);
            var result = new Dictionary<Guid, int>();

            foreach (var technical in technicals)
            {
                var maintenanceCount = await _context.Maintenances
                    .CountAsync(m => m.TechnicalId == technical.Id, cancellationToken);
                
                var decommissionCount = await _context.EquipmentDecommissions
                    .CountAsync(ed => ed.TechnicalId == technical.Id, cancellationToken);
                
                result[technical.Id] = maintenanceCount + decommissionCount;
            }

            return result;
        }
    }
}