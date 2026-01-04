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
    }
}