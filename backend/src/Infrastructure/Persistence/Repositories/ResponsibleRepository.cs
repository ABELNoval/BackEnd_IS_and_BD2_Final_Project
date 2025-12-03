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
    public class ResponsibleRepository : BaseRepository<Responsible>, IResponsibleRepository
    {
        public ResponsibleRepository(AppDbContext context) : base(context) { }


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

        public async Task<IEnumerable<Responsible>> SearchByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Responsibles
                .Where(r => r.Name.Contains(name))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Responsible>> GetResponsiblesWithDepartmentsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Responsibles
                .Where(r => _context.Departments.Any(d => d.ResponsibleId == r.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Responsible>> GetResponsiblesWithoutDepartmentsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Responsibles
                .Where(r => !_context.Departments.Any(d => d.ResponsibleId == r.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<Dictionary<Guid, int>> GetDepartmentCountByResponsibleAsync(CancellationToken cancellationToken = default)
        {
            var result = new Dictionary<Guid, int>();
            
            var responsibles = await _context.Responsibles.ToListAsync(cancellationToken);

            foreach (var responsible in responsibles)
            {
                var count = await _context.Departments
                    .CountAsync(d => d.ResponsibleId == responsible.Id, cancellationToken);
                result[responsible.Id] = count;
            }

            return result;
        }

        public async Task<IEnumerable<Responsible>> GetByEmailDomainAsync(string domain, CancellationToken cancellationToken = default)
        {
            return await _context.Responsibles
                .Where(r => r.Email.Value.EndsWith($"@{domain}"))
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasAssignedDepartmentsAsync(Guid responsibleId, CancellationToken cancellationToken = default)
        {
            return await _context.Departments
                .AnyAsync(d => d.ResponsibleId == responsibleId, cancellationToken);
        }

        public async Task<IEnumerable<Responsible>> GetActiveResponsiblesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Responsibles
                .Where(r => _context.Departments.Any(d => d.ResponsibleId == r.Id))
                .ToListAsync(cancellationToken);
        }
    }
}