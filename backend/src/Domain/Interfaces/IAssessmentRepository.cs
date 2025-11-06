using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAssessmentRepository : IRepository<Assessment>
    {
        // Métodos específicos de Assessment
        Task<IEnumerable<Assessment>> GetByTechnicalIdAsync(Guid technicalId);
        Task<IEnumerable<Assessment>> GetByDirectorIdAsync(Guid directorId);
        Task<IEnumerable<Assessment>> GetByDateRangeAsync(DateTime from, DateTime to);
        Task<IEnumerable<Assessment>> GetByScoreRangeAsync(decimal minScore, decimal maxScore);
        Task<Assessment?> GetLatestByTechnicalAsync(Guid technicalId);
    }
}
