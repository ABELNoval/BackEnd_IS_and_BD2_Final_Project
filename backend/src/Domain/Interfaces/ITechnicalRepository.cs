using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITechnicalRepository : IRepository<Technical>
    {
        /// <summary>
        /// Filtra técnicos usando Dynamic LINQ
        /// </summary>
        /// <param name="query">Consulta en formato Dynamic LINQ (ej: "Name.Contains('John') && Experience > 5")</param>
        Task<IEnumerable<Technical>> FilterAsync(string query, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Filtra técnicos con paginación
        /// </summary>
        Task<(IEnumerable<Technical> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query,
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Busca técnicos por nombre
        /// </summary>
        Task<IEnumerable<Technical>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene técnicos por especialidad
        /// </summary>
        Task<IEnumerable<Technical>> GetBySpecialtyAsync(string specialty, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene técnicos con experiencia mínima
        /// </summary>
        Task<IEnumerable<Technical>> GetByMinExperienceAsync(int minExperience, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene técnicos con evaluaciones (assessments)
        /// </summary>
        Task<IEnumerable<Technical>> GetTechnicalsWithAssessmentsAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene técnicos sin evaluaciones
        /// </summary>
        Task<IEnumerable<Technical>> GetTechnicalsWithoutAssessmentsAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene el promedio de puntuación por técnico
        /// </summary>
        Task<Dictionary<Guid, decimal>> GetAverageScoreByTechnicalAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene técnicos que han realizado mantenimientos
        /// </summary>
        Task<IEnumerable<Technical>> GetTechnicalsWithMaintenancesAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene técnicos que han realizado descomisiones
        /// </summary>
        Task<IEnumerable<Technical>> GetTechnicalsWithDecommissionsAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene técnicos activos (con actividades recientes)
        /// </summary>
        Task<IEnumerable<Technical>> GetActiveTechnicalsAsync(DateTime? fromDate = null, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene técnicos por dominio de email
        /// </summary>
        Task<IEnumerable<Technical>> GetByEmailDomainAsync(string domain, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene el conteo de actividades por técnico (mantenimientos + descomisiones)
        /// </summary>
        Task<Dictionary<Guid, int>> GetActivityCountByTechnicalAsync(CancellationToken cancellationToken = default);
    }
}