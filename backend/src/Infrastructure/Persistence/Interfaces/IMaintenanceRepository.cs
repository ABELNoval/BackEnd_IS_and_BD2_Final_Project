using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories
{
    public interface IMaintenanceRepository
    {
        Task<Maintenance> GetByIdAsync(int id);
        Task<IEnumerable<Maintenance>> GetAllAsync();
        Task AddAsync(Maintenance maintenance);
        Task UpdateAsync(Maintenance maintenance);
        Task DeleteAsync(int id);
    }
}