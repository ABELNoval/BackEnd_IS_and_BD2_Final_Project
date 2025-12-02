using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        /// <summary>
        /// Filtra departamentos usando Dynamic LINQ
        /// </summary>
        /// <param name="query">Consulta en formato Dynamic LINQ (ej: "Name.Contains('IT') && SectionId != null")</param>
        Task<IEnumerable<Department>> FilterAsync(string query, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Filtra departamentos con paginación
        /// </summary>
        Task<(IEnumerable<Department> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query,
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Obtiene departamentos por ID de sección
        /// </summary>
        Task<IEnumerable<Department>> GetBySectionIdAsync(Guid sectionId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene departamentos por ID del responsable
        /// </summary>
        Task<IEnumerable<Department>> GetByResponsibleIdAsync(Guid responsibleId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Busca departamentos por nombre (case-insensitive)
        /// </summary>
        Task<IEnumerable<Department>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene departamentos que tienen equipos asignados
        /// </summary>
        Task<IEnumerable<Department>> GetDepartmentsWithEquipmentAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene departamentos sin equipos asignados
        /// </summary>
        Task<IEnumerable<Department>> GetDepartmentsWithoutEquipmentAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene el conteo de equipos por departamento
        /// </summary>
        Task<Dictionary<Guid, int>> GetEquipmentCountByDepartmentAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Verifica si existe un departamento con el mismo nombre en la misma sección
        /// </summary>
        Task<bool> ExistsWithSameNameAndSectionAsync(string name, Guid sectionId, CancellationToken cancellationToken = default);
    }
}