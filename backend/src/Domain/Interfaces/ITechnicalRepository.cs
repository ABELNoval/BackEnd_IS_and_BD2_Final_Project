using Domain.Entities;

namespace Domain.Interfaces;

public interface ITechnicalRepository : IRepository<Technical>
{
    /// <summary>
    /// Gets a technical by name (case-insensitive).
    /// </summary>
    Task<Technical?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a technical by email.
    /// </summary>
    Task<Technical?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all technicals with pagination.
    /// </summary>
    Task<IEnumerable<Technical>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a technical with the given email exists.
    /// </summary>
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all technicals by specialty (case-insensitive).
    /// </summary>
    Task<IEnumerable<Technical>> GetBySpecialtyAsync(string specialty, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all technicals with experience greater than or equal to the specified value.
    /// </summary>
    Task<IEnumerable<Technical>> GetByMinimumExperienceAsync(int minExperience, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all technicals with experience in a range.
    /// </summary>
    Task<IEnumerable<Technical>> GetByExperienceRangeAsync(int minExperience, int maxExperience, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a technical with all assessments included.
    /// </summary>
    Task<Technical?> GetByIdWithAssessmentsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all technicals who have performed maintenance.
    /// </summary>
    Task<IEnumerable<Technical>> GetTechnicalsWithMaintenanceAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets count of maintenance records performed by a technical.
    /// </summary>
    Task<int> GetMaintenanceCountAsync(Guid technicalId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets count of assessments received by a technical.
    /// </summary>
    Task<int> GetAssessmentCountAsync(Guid technicalId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets average assessment score for a technical.
    /// </summary>
    Task<decimal?> GetAverageAssessmentScoreAsync(Guid technicalId, CancellationToken cancellationToken = default);
}