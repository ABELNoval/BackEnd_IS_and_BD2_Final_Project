using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAssessmentRepository : IRepository<Assessment>
    {       
        /// <summary>
        /// Filtra assessments usando Dynamic LINQ
        /// </summary>
        /// <param name="query">Consulta en formato Dynamic LINQ (ej: "TechnicalId == Guid('...') && Score.Value > 80")</param>
        Task<IEnumerable<Assessment>> FilterAsync(string query, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Filtra assessments con paginación
        /// </summary>
        Task<(IEnumerable<Assessment> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query,
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Obtiene assessments por ID del técnico
        /// </summary>
        Task<IEnumerable<Assessment>> GetByTechnicalIdAsync(Guid technicalId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene assessments por ID del director
        /// </summary>
        Task<IEnumerable<Assessment>> GetByDirectorIdAsync(Guid directorId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene assessments por rango de puntuación
        /// </summary>
        Task<IEnumerable<Assessment>> GetByScoreRangeAsync(
            decimal minScore, 
            decimal maxScore, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Obtiene assessments por rango de fechas
        /// </summary>
        Task<IEnumerable<Assessment>> GetByDateRangeAsync(
            DateTime startDate, 
            DateTime endDate, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Obtiene el promedio de puntuaciones de un técnico
        /// </summary>
        Task<decimal> GetAverageScoreByTechnicalIdAsync(Guid technicalId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Cuenta los assessments realizados por un director
        /// </summary>
        Task<int> CountAssessmentsByDirectorAsync(Guid directorId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Verifica si un técnico tiene al menos una evaluación con puntuación baja (< 60)
        /// </summary>
        Task<bool> HasLowScoreAssessmentsAsync(Guid technicalId, CancellationToken cancellationToken = default);
    }
}