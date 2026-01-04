using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
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
    public class ResponsibleRepository : BaseRepository<Responsible>, IResponsibleRepository
    {
        public ResponsibleRepository(AppDbContext context) : base(context) { }

        public async Task<Responsible?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            // EF Core will use the Value Converter to convert Email.Create(email) to string for comparison
            return await _context.Responsibles
                .FirstOrDefaultAsync(r => r.Email == Email.Create(email), cancellationToken);
        }

        public async Task<IEnumerable<Responsible>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            IQueryable<Responsible> baseQuery = _context.Responsibles;

            if (!string.IsNullOrWhiteSpace(query))
            {
                baseQuery = baseQuery.Where(query);
            }

            return await baseQuery.ToListAsync(cancellationToken);
        }

        public async Task<(IEnumerable<Responsible> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            IQueryable<Responsible> baseQuery = _context.Responsibles;

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