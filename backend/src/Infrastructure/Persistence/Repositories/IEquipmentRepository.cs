using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories
{
    public interface IEquipmentRepository
    {
        Task<Equipment> GetByIdAsync(int id);
        Task<IEnumerable<Equipment>> GetAllAsync();
        Task AddAsync(Equipment equipment);
        Task UpdateAsync(Equipment equipment);
        Task DeleteAsync(int id);
    }
}