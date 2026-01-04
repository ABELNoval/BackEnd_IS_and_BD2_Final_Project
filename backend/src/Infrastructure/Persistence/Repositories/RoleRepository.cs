using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Infrastructure.Persistence.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Role>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            var allRoles = Role.GetAll();
            var queryable = allRoles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                queryable = queryable.Where(query);
            }

            return queryable.ToList();
        }


        public async Task<IEnumerable<Role>> GetAllOrderedAsync(CancellationToken cancellationToken = default)
        {
            return Role.GetAll().OrderBy(r => r.Id).ToList();
        }

        public async Task<IEnumerable<Role>> SearchByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return Role.GetAll()
                .Where(r => r.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return Role.GetAll()
                .Any(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IEnumerable<Role>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
        {
            var idSet = new HashSet<int>(ids);
            return Role.GetAll()
                .Where(r => idSet.Contains(r.Id))
                .ToList();
        }

        public async Task<IEnumerable<Role>> GetAssignableRolesAsync(CancellationToken cancellationToken = default)
        {
            return Role.GetAll()
                .Where(r => r.Id != 1) 
                .ToList();
        }

        public async Task<Dictionary<int, int>> GetUserCountByRoleAsync(CancellationToken cancellationToken = default)
        {
            var result = new Dictionary<int, int>();
            var roles = Role.GetAll();

            foreach (var role in roles)
            {
                var count = await _context.Users
                    .CountAsync(u => u.RoleId == role.Id, cancellationToken);
                result[role.Id] = count;
            }

            return result;
        }
    }
}