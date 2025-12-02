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
    public class SectionRepository : BaseRepository<Section>, ISectionRepository
    {
        public SectionRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Section>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            IQueryable<Section> baseQuery = _context.Sections;

            if (!string.IsNullOrWhiteSpace(query))
            {
                baseQuery = baseQuery.Where(query);
            }

            return await baseQuery.ToListAsync(cancellationToken);
        }

        public async Task<(IEnumerable<Section> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            IQueryable<Section> baseQuery = _context.Sections;

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

        public async Task<IEnumerable<Section>> SearchByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Sections
                .Where(s => s.Name.Contains(name))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Section>> GetSectionsWithDepartmentsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Sections
                .Where(s => _context.Departments.Any(d => d.SectionId == s.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Section>> GetSectionsWithoutDepartmentsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Sections
                .Where(s => !_context.Departments.Any(d => d.SectionId == s.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<Dictionary<Guid, int>> GetDepartmentCountBySectionAsync(CancellationToken cancellationToken = default)
        {
            var sections = await _context.Sections.ToListAsync(cancellationToken);
            var result = new Dictionary<Guid, int>();

            foreach (var section in sections)
            {
                var count = await _context.Departments
                    .CountAsync(d => d.SectionId == section.Id, cancellationToken);
                result[section.Id] = count;
            }

            return result;
        }

        public async Task<IEnumerable<Section>> GetSectionsWithEquipmentAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Sections
                .Where(s => _context.Departments
                    .Any(d => d.SectionId == s.Id && 
                           _context.Equipments.Any(e => e.DepartmentId == d.Id)))
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Sections
                .AnyAsync(s => s.Name == name, cancellationToken);
        }

        public async Task<IEnumerable<Section>> GetAllOrderedByNameAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Sections
                .OrderBy(s => s.Name)
                .ToListAsync(cancellationToken);
        }
    }
}