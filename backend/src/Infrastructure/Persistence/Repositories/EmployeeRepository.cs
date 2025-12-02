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
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Employee>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            IQueryable<Employee> baseQuery = _context.Employees;

            if (!string.IsNullOrWhiteSpace(query))
            {
                baseQuery = baseQuery.Where(query);
            }

            return await baseQuery.ToListAsync(cancellationToken);
        }

        public async Task<(IEnumerable<Employee> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            IQueryable<Employee> baseQuery = _context.Employees;

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

        public async Task<IEnumerable<Employee>> SearchByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .Where(e => e.Name.Contains(name))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Employee>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .Where(e => e.RoleId == roleId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesWithoutDepartmentResponsibilityAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .Where(e => !_context.Departments.Any(d => d.ResponsibleId == e.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Employee>> GetByEmailDomainAsync(string domain, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .Where(e => e.Email.Value.EndsWith($"@{domain}"))
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .AnyAsync(e => e.Email.Value == email, cancellationToken);
        }

        public async Task<IEnumerable<Employee>> GetByCreationDateRangeAsync(
            DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .ToListAsync(cancellationToken); 
        }
    }
}