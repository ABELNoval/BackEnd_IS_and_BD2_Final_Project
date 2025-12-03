using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class AssessmentRepository : BaseRepository<Assessment>, IAssessmentRepository
    {
        public AssessmentRepository(AppDbContext context) : base(context) { }
        
        public async Task<IEnumerable<Assessment>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            IQueryable<Assessment> baseQuery = _context.Assessments;

            if (!string.IsNullOrWhiteSpace(query))
            {
                try
                {
                    baseQuery = baseQuery.Where(query);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error en Dynamic LINQ query: {query}", ex);
                }
            }

            return (IEnumerable<Assessment>)await baseQuery.Select(a => a.Id).ToListAsync(cancellationToken);
        }

        public async Task<(IEnumerable<Assessment> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            IQueryable<Assessment> baseQuery = _context.Assessments;

            if (!string.IsNullOrWhiteSpace(query))
            {
                baseQuery = baseQuery.Where(query);
            }

            var total = await baseQuery.CountAsync(cancellationToken);
            var pages = (int)Math.Ceiling(total / (double)pageSize);

            var data = await baseQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (data, total, pages);
        }

        public async Task<IEnumerable<Assessment>> GetByTechnicalIdAsync(Guid technicalId, CancellationToken cancellationToken = default)
        {
            return await _context.Assessments
                .Where(a => a.TechnicalId == technicalId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Assessment>> GetByDirectorIdAsync(Guid directorId, CancellationToken cancellationToken = default)
        {
            return await _context.Assessments
                .Where(a => a.DirectorId == directorId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Assessment>> GetByScoreRangeAsync(
            decimal minScore, decimal maxScore, CancellationToken cancellationToken = default)
        {
            return await _context.Assessments
                .Where(a => a.Score.Value >= minScore && a.Score.Value <= maxScore)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Assessment>> GetByDateRangeAsync(
            DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _context.Assessments
                .Where(a => a.AssessmentDate >= startDate && a.AssessmentDate <= endDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<decimal> GetAverageScoreByTechnicalIdAsync(Guid technicalId, CancellationToken cancellationToken = default)
        {
            var assessments = await GetByTechnicalIdAsync(technicalId, cancellationToken);
            return assessments.Any() 
                ? assessments.Average(a => a.Score.Value) 
                : 0;
        }

        public async Task<int> CountAssessmentsByDirectorAsync(Guid directorId, CancellationToken cancellationToken = default)
        {
            return await _context.Assessments
                .CountAsync(a => a.DirectorId == directorId, cancellationToken);
        }

        public async Task<bool> HasLowScoreAssessmentsAsync(Guid technicalId, CancellationToken cancellationToken = default)
        {
            return await _context.Assessments
                .AnyAsync(a => a.TechnicalId == technicalId && a.Score.Value < 60, cancellationToken);
        }
    }
}