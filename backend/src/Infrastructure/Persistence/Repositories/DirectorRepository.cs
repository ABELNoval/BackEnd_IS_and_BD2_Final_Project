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
    public class DirectorRepository : BaseRepository<Director>, IDirectorRepository
    {
        public DirectorRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Director>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            IQueryable<Director> baseQuery = _context.Directors;

            if (!string.IsNullOrWhiteSpace(query))
            {
                baseQuery = baseQuery.Where(query);
            }

            return await baseQuery.ToListAsync(cancellationToken);
        }

        public async Task<(IEnumerable<Director> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            IQueryable<Director> baseQuery = _context.Directors;

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

        public async Task<IEnumerable<Director>> GetDirectorsWithAssessmentsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Directors
                .Where(d => _context.Assessments.Any(a => a.DirectorId == d.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<Dictionary<Guid, int>> GetAssessmentCountByDirectorAsync(CancellationToken cancellationToken = default)
        {
            var directors = await _context.Directors.ToListAsync(cancellationToken);
            var result = new Dictionary<Guid, int>();

            foreach (var director in directors)
            {
                var count = await _context.Assessments
                    .CountAsync(a => a.DirectorId == director.Id, cancellationToken);
                result[director.Id] = count;
            }

            return result;
        }

        public async Task<IEnumerable<Director>> SearchByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Directors
                .Where(d => d.Name.Contains(name))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Director>> GetActiveDirectorsAsync(DateTime? fromDate = null, CancellationToken cancellationToken = default)
        {
            fromDate ??= DateTime.UtcNow.AddDays(-30);

            return await _context.Directors
                .Where(d => _context.Assessments
                    .Any(a => a.DirectorId == d.Id && a.AssessmentDate >= fromDate))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Director>> GetByEmailDomainAsync(string domain, CancellationToken cancellationToken = default)
        {
            return await _context.Directors
                .Where(d => d.Email.Value.EndsWith($"@{domain}"))
                .ToListAsync(cancellationToken);
        }
    }
}