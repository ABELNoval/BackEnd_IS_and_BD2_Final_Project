using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class AssessmentRepository : BaseRepository<Assessment>, IAssessmentRepository
    {
        public AssessmentRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Assessment>> GetByTechnicalIdAsync(Guid technicalId)
        {
            return await _context.Assessments
                .Where(a => a.TechnicalId == technicalId)
                .OrderByDescending(a => a.AssessmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Assessment>> GetByDirectorIdAsync(Guid directorId)
        {
            return await _context.Assessments
                .Where(a => a.DirectorId == directorId)
                .OrderByDescending(a => a.AssessmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Assessment>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _context.Assessments
                .Where(a => a.AssessmentDate >= from && a.AssessmentDate <= to)
                .OrderByDescending(a => a.AssessmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Assessment>> GetByScoreRangeAsync(decimal minScore, decimal maxScore)
        {
            return await _context.Assessments
                .Where(a => a.Score != null && a.Score.Value >= minScore && a.Score.Value <= maxScore)
                .OrderByDescending(a => a.AssessmentDate)
                .ToListAsync();
        }

        public async Task<Assessment?> GetLatestByTechnicalAsync(Guid technicalId)
        {
            return await _context.Assessments
                .Where(a => a.TechnicalId == technicalId)
                .OrderByDescending(a => a.AssessmentDate)
                .FirstOrDefaultAsync();
        }
    }
}
