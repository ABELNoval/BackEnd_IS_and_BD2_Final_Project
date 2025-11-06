using Domain.Enumerations;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class DestinyTypeRepository : BaseRepository<DestinyType>, IDestinyTypeRepository
    {
        public DestinyTypeRepository(AppDbContext context) : base(context) { }

        public Task<DestinyType?> GetByIdAsync(int id)
        {
            try
            {
                var item = DestinyType.GetAll().FirstOrDefault(d => d.Id == id);
                return Task.FromResult(item);
            }
            catch (Exception)
            {
                return Task.FromResult<DestinyType?>(null);
            }
        }

        public Task<DestinyType?> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Task.FromResult<DestinyType?>(null);

            var item = DestinyType.GetAll().FirstOrDefault(d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(item);
        }

        public Task<IEnumerable<DestinyType>> GetAllTypesAsync()
        {
            var all = DestinyType.GetAll().ToList().AsEnumerable();
            return Task.FromResult(all);
        }

        public Task<bool> RequiresDepartmentAsync(int id)
        {
            var item = DestinyType.GetAll().FirstOrDefault(d => d.Id == id);
            return Task.FromResult(item != null && item.RequiresDepartment);
        }
    }
}