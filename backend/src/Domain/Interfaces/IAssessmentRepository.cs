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
    }
}