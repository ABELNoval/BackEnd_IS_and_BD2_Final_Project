using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Enumerations;

namespace Domain.Interfaces
{
    public interface IDestinyTypeRepository : IRepository<DestinyType>
    {
        /// <summary>
        /// Filtra tipos de destino usando Dynamic LINQ
        /// </summary>
        /// <param name="query">Consulta en formato Dynamic LINQ (ej: "Name.Contains('Warehouse')")</param>
        Task<IEnumerable<DestinyType>> FilterAsync(string query, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene todos los tipos de destino ordenados por ID
        /// </summary>
        Task<IEnumerable<DestinyType>> GetAllOrderedAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Busca tipos de destino por nombre
        /// </summary>
        Task<IEnumerable<DestinyType>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene tipos de destino disponibles para descomisión
        /// </summary>
        Task<IEnumerable<DestinyType>> GetAvailableForDecommissionAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Verifica si un tipo de destino existe por nombre
        /// </summary>
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene tipos de destino por IDs
        /// </summary>
        Task<IEnumerable<DestinyType>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
    }
}