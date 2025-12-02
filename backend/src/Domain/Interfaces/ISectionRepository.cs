using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISectionRepository : IRepository<Section>
    {
        /// <summary>
        /// Filtra secciones usando Dynamic LINQ
        /// </summary>
        /// <param name="query">Consulta en formato Dynamic LINQ (ej: "Name.Contains('IT')")</param>
        Task<IEnumerable<Section>> FilterAsync(string query, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Filtra secciones con paginación
        /// </summary>
        Task<(IEnumerable<Section> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query,
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Busca secciones por nombre
        /// </summary>
        Task<IEnumerable<Section>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene secciones con departamentos asignados
        /// </summary>
        Task<IEnumerable<Section>> GetSectionsWithDepartmentsAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene secciones sin departamentos asignados
        /// </summary>
        Task<IEnumerable<Section>> GetSectionsWithoutDepartmentsAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene el conteo de departamentos por sección
        /// </summary>
        Task<Dictionary<Guid, int>> GetDepartmentCountBySectionAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene secciones con equipos (a través de departamentos)
        /// </summary>
        Task<IEnumerable<Section>> GetSectionsWithEquipmentAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Verifica si existe una sección con el mismo nombre
        /// </summary>
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene secciones ordenadas por nombre
        /// </summary>
        Task<IEnumerable<Section>> GetAllOrderedByNameAsync(CancellationToken cancellationToken = default);
    }
}