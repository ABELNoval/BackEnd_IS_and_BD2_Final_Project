using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IResponsibleRepository : IRepository<Responsible>
    {
        // ✅ MÉTODOS DE FILTRADO DINÁMICO ESPECÍFICOS PARA RESPONSIBLE
        
        /// <summary>
        /// Filtra responsables usando Dynamic LINQ
        /// </summary>
        /// <param name="query">Consulta en formato Dynamic LINQ (ej: "Name.Contains('John')")</param>
        Task<IEnumerable<Responsible>> FilterAsync(string query, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Filtra responsables con paginación
        /// </summary>
        Task<(IEnumerable<Responsible> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query,
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Busca responsables por nombre
        /// </summary>
        Task<IEnumerable<Responsible>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene responsables con departamentos asignados
        /// </summary>
        Task<IEnumerable<Responsible>> GetResponsiblesWithDepartmentsAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene responsables sin departamentos asignados
        /// </summary>
        Task<IEnumerable<Responsible>> GetResponsiblesWithoutDepartmentsAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene el conteo de departamentos por responsable
        /// </summary>
        Task<Dictionary<Guid, int>> GetDepartmentCountByResponsibleAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene responsables por dominio de email
        /// </summary>
        Task<IEnumerable<Responsible>> GetByEmailDomainAsync(string domain, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Verifica si un responsable tiene departamentos asignados
        /// </summary>
        Task<bool> HasAssignedDepartmentsAsync(Guid responsibleId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene responsables activos (con departamentos asignados)
        /// </summary>
        Task<IEnumerable<Responsible>> GetActiveResponsiblesAsync(CancellationToken cancellationToken = default);
    }
}