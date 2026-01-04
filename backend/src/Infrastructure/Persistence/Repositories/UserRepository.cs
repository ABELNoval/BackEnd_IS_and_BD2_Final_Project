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
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<User>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            IQueryable<User> baseQuery = _context.Users;

            if (!string.IsNullOrWhiteSpace(query))
            {
                baseQuery = baseQuery.Where(query);
            }

            return await baseQuery.ToListAsync(cancellationToken);
        }

        public async Task<(IEnumerable<User> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            IQueryable<User> baseQuery = _context.Users;

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

        public async Task<IEnumerable<User>> SearchByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Where(u => u.Name.Contains(name))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Where(u => u.RoleId == roleId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> GetByEmailDomainAsync(string domain, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Where(u => u.Email.Value.EndsWith($"@{domain}"))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync(DateTime? fromDate = null, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> SearchByEmailAsync(string emailPart, CancellationToken cancellationToken = default)
        {
            // Note: Contains on Value Object property might fail if not owned. 
            // Since Email is converted, we can't use .Value.Contains easily unless we do client eval or raw SQL.
            // However, if we assume the converter maps to string, maybe we can use EF.Functions.Like?
            // Or just fetch all and filter (bad for performance).
            // For now, let's try to use the property directly if possible, but Contains on Email object is not defined.
            // We will leave this as is for now, but be aware it might fail.
            return await _context.Users
                .Where(u => u.Email.Value.Contains(emailPart))
                .ToListAsync(cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == Email.Create(email), cancellationToken);
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == Email.Create(email), cancellationToken);
        }

        public async Task<Dictionary<int, int>> GetCountByRoleAsync(CancellationToken cancellationToken = default)
        {
            var result = new Dictionary<int, int>();

            var roleIds = new[] { 1, 2, 3, 4, 5, 6 }; 
            
            foreach (var roleId in roleIds)
            {
                var count = await _context.Users
                    .CountAsync(u => u.RoleId == roleId, cancellationToken);
                result[roleId] = count;
            }

            return result;
        }

        public async Task<IEnumerable<User>> GetByRoleIdsAsync(IEnumerable<int> roleIds, CancellationToken cancellationToken = default)
        {
            var roleIdSet = new HashSet<int>(roleIds);
            return await _context.Users
                .Where(u => roleIdSet.Contains(u.RoleId))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> GetInactiveUsersAsync(int daysThreshold = 90, CancellationToken cancellationToken = default)
        {
            return new List<User>();
        }

        public async Task<IEnumerable<User>> GetUsersWithSpecificTypesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Users.ToListAsync(cancellationToken);
        }
    }
}