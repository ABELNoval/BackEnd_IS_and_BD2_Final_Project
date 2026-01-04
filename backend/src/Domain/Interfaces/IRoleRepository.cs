using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        /// <summary>
        /// Filtra roles usando Dynamic LINQ
        /// </summary>
        /// <param name="query">Consulta en formato Dynamic LINQ (ej: "Id > 2")</param>
        Task<IEnumerable<Role>> FilterAsync(string query, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene todos los roles ordenados por ID
        /// </summary>
        Task<IEnumerable<Role>> GetAllOrderedAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Busca roles por nombre
        /// </summary>
        Task<IEnumerable<Role>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Verifica si existe un rol con el nombre especificado
        /// </summary>
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene roles por IDs
        /// </summary>
        Task<IEnumerable<Role>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene roles disponibles para asignación de usuarios
        /// </summary>
        Task<IEnumerable<Role>> GetAssignableRolesAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene el conteo de usuarios por rol
        /// </summary>
        Task<Dictionary<int, int>> GetUserCountByRoleAsync(CancellationToken cancellationToken = default);
    }
}