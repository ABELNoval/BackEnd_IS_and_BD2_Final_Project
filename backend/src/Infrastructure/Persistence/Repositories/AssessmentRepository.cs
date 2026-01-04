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
    public class AssessmentRepository : BaseRepository<Assessment>, IAssessmentRepository
    {
        public AssessmentRepository(AppDbContext context) : base(context) { }
        
        public async Task<IEnumerable<Assessment>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            IQueryable<Assessment> baseQuery = _context.Assessments;

            if (!string.IsNullOrWhiteSpace(query))
            {
                try
                {
                    baseQuery = baseQuery.Where(query);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error en Dynamic LINQ query: {query}", ex);
                }
            }

            return await baseQuery.ToListAsync(cancellationToken);
        }

        public async Task<(IEnumerable<Assessment> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            IQueryable<Assessment> baseQuery = _context.Assessments;

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
    }
}