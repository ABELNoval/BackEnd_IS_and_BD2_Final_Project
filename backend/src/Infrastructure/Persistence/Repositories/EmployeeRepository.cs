using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context) { }

        public async Task<Employee?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            var trimmed = name.Trim();
            return await _context.Employees
                .Where(e => e.Name.ToLower() == trimmed.ToLower())
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Employee?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;

            var trimmed = email.Trim();
            return await _context.Employees
                .Where(e => e.Email.Value.ToLower() == trimmed.ToLower())
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Employee>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            return await _context.Employees
                .OrderBy(e => e.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            var trimmed = email.Trim();
            return await _context.Employees
                .AnyAsync(e => e.Email.Value.ToLower() == trimmed.ToLower(), cancellationToken);
        }
    }
}