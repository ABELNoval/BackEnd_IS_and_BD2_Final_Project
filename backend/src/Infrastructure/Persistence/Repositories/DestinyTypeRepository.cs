using Domain.Enumerations;
using Domain.Interfaces;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class DestinyTypeRepository : BaseRepository<DestinyType>, IDestinyTypeRepository
    {
        public DestinyTypeRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<DestinyType>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            var allTypes = DestinyType.GetAll();
            var queryable = allTypes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                queryable = queryable.Where(query);
            }

            return queryable.ToList(); 
        }

        public async Task<IEnumerable<DestinyType>> GetAllOrderedAsync(CancellationToken cancellationToken = default)
        {
            return DestinyType.GetAll().OrderBy(dt => dt.Id).ToList();
        }

        public async Task<IEnumerable<DestinyType>> SearchByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return DestinyType.GetAll()
                .Where(dt => dt.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public async Task<IEnumerable<DestinyType>> GetAvailableForDecommissionAsync(CancellationToken cancellationToken = default)
        {
            // Tipos de destino válidos para descomisión
            return DestinyType.GetAll()
                .Where(dt => dt.Id != 0) 
                .ToList();
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return DestinyType.GetAll()
                .Any(dt => dt.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IEnumerable<DestinyType>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
        {
            var idSet = new HashSet<int>(ids);
            return DestinyType.GetAll()
                .Where(dt => idSet.Contains(dt.Id))
                .ToList();
        }
    }
}