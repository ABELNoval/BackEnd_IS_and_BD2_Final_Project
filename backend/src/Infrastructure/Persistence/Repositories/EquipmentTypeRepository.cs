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
    public class EquipmentTypeRepository : BaseRepository<EquipmentType>, IEquipmentTypeRepository
    {
        public EquipmentTypeRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<EquipmentType>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            IQueryable<EquipmentType> baseQuery = _context.EquipmentTypes;

            if (!string.IsNullOrWhiteSpace(query))
            {
                baseQuery = baseQuery.Where(query);
            }

            return await baseQuery.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<EquipmentType>> SearchByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.EquipmentTypes
                .Where(et => et.Name.Contains(name))
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.EquipmentTypes
                .AnyAsync(et => et.Name == name, cancellationToken);
        }

        public async Task<IEnumerable<EquipmentType>> GetWithEquipmentCountAsync(CancellationToken cancellationToken = default)
        {
            var equipmentTypes = await _context.EquipmentTypes.ToListAsync(cancellationToken);
            
            return equipmentTypes;
        }

        public async Task<IEnumerable<EquipmentType>> GetTypesWithoutEquipmentAsync(CancellationToken cancellationToken = default)
        {
            return await _context.EquipmentTypes
                .Where(et => !_context.Equipments.Any(e => e.EquipmentTypeId == et.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<EquipmentType>> GetAllOrderedByNameAsync(CancellationToken cancellationToken = default)
        {
            return await _context.EquipmentTypes
                .OrderBy(et => et.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<EquipmentType>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            var idSet = new HashSet<Guid>(ids);
            return await _context.EquipmentTypes
                .Where(et => idSet.Contains(et.Id))
                .ToListAsync(cancellationToken);
        }
    }
}