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
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Department>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            IQueryable<Department> baseQuery = _context.Departments;

            if (!string.IsNullOrWhiteSpace(query))
            {
                baseQuery = baseQuery.Where(query);
            }

            return await baseQuery.ToListAsync(cancellationToken);
        }

        public async Task<(IEnumerable<Department> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            IQueryable<Department> baseQuery = _context.Departments;

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

        public async Task<IEnumerable<Department>> GetBySectionIdAsync(Guid sectionId, CancellationToken cancellationToken = default)
        {
            return await _context.Departments
                .Where(d => d.SectionId == sectionId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Department>> GetByResponsibleIdAsync(Guid responsibleId, CancellationToken cancellationToken = default)
        {
            return await _context.Departments
                .Where(d => d.ResponsibleId == responsibleId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Department>> SearchByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Departments
                .Where(d => d.Name.Contains(name))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Department>> GetDepartmentsWithEquipmentAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Departments
                .Where(d => _context.Equipments.Any(e => e.DepartmentId == d.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Department>> GetDepartmentsWithoutEquipmentAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Departments
                .Where(d => !_context.Equipments.Any(e => e.DepartmentId == d.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<Dictionary<Guid, int>> GetEquipmentCountByDepartmentAsync(CancellationToken cancellationToken = default)
        {
            var departments = await _context.Departments.ToListAsync(cancellationToken);
            var result = new Dictionary<Guid, int>();

            foreach (var department in departments)
            {
                var count = await _context.Equipments
                    .CountAsync(e => e.DepartmentId == department.Id, cancellationToken);
                result[department.Id] = count;
            }

            return result;
        }

        public async Task<bool> ExistsWithSameNameAndSectionAsync(string name, Guid sectionId, CancellationToken cancellationToken = default)
        {
            return await _context.Departments
                .AnyAsync(d => d.Name == name && d.SectionId == sectionId, cancellationToken);
        }
    }
}