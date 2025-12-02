using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Filtra usuarios usando Dynamic LINQ
        /// </summary>
        /// <param name="query">Consulta en formato Dynamic LINQ (ej: "Name.Contains('John') && RoleId == 3")</param>
        Task<IEnumerable<User>> FilterAsync(string query, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Filtra usuarios con paginación
        /// </summary>
        Task<(IEnumerable<User> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query,
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Busca usuarios por nombre
        /// </summary>
        Task<IEnumerable<User>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene usuarios por rol
        /// </summary>
        Task<IEnumerable<User>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene usuarios por dominio de email
        /// </summary>
        Task<IEnumerable<User>> GetByEmailDomainAsync(string domain, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene usuarios activos (creados en los últimos X días o con actividad)
        /// </summary>
        Task<IEnumerable<User>> GetActiveUsersAsync(DateTime? fromDate = null, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene usuarios con email que contiene texto específico
        /// </summary>
        Task<IEnumerable<User>> SearchByEmailAsync(string emailPart, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Verifica si existe un usuario con el email especificado
        /// </summary>
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene el conteo de usuarios por rol
        /// </summary>
        Task<Dictionary<int, int>> GetCountByRoleAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene usuarios por múltiples roles
        /// </summary>
        Task<IEnumerable<User>> GetByRoleIdsAsync(IEnumerable<int> roleIds, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene usuarios que no han tenido actividad reciente
        /// </summary>
        Task<IEnumerable<User>> GetInactiveUsersAsync(int daysThreshold = 90, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene usuarios con sus tipos específicos (Technical, Director, etc.)
        /// </summary>
        Task<IEnumerable<User>> GetUsersWithSpecificTypesAsync(CancellationToken cancellationToken = default);
    }
}