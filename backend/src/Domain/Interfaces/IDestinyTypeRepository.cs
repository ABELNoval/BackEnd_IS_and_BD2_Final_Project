using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Enumerations;

namespace Domain.Interfaces
{
    public interface IDestinyTypeRepository : IRepository<DestinyType>
    {
        // Obtener por id numérico (las enumeraciones usan int como Id)
        Task<DestinyType?> GetByIdAsync(int id);

        // Obtener por nombre (case-insensitive)
        Task<DestinyType?> GetByNameAsync(string name);

        // Devuelve todas las opciones disponibles
        Task<IEnumerable<DestinyType>> GetAllTypesAsync();

        // Indica si un tipo de destiny requiere department
        Task<bool> RequiresDepartmentAsync(int id);
    }
}