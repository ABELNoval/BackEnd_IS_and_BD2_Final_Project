using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IDirectorRepository : IRepository<Director>
    {
        /// <summary>
        /// Filtra directores usando Dynamic LINQ
        /// </summary>
        /// <param name="query">Consulta en formato Dynamic LINQ (ej: "Email.Value.Contains('@company.com')")</param>
        Task<IEnumerable<Director>> FilterAsync(string query, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Filtra directores con paginación
        /// </summary>
        Task<(IEnumerable<Director> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query,
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Obtiene directores que han realizado evaluaciones
        /// </summary>
        Task<IEnumerable<Director>> GetDirectorsWithAssessmentsAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene el conteo de evaluaciones por director
        /// </summary>
        Task<Dictionary<Guid, int>> GetAssessmentCountByDirectorAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Busca directores por nombre
        /// </summary>
        Task<IEnumerable<Director>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene directores activos (con al menos una evaluación en los últimos 30 días)
        /// </summary>
        Task<IEnumerable<Director>> GetActiveDirectorsAsync(DateTime? fromDate = null, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene directores por dominio de email
        /// </summary>
        Task<IEnumerable<Director>> GetByEmailDomainAsync(string domain, CancellationToken cancellationToken = default);
    }
}