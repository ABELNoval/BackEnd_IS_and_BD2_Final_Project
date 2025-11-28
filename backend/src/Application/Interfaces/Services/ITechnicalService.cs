using Application.DTOs.Technical;

namespace Application.Interfaces.Services
{
    public interface ITechnicalService
    {
        Task<TechnicalDto> CreateAsync(CreateTechnicalDto dto, CancellationToken cancellationToken = default);
        Task<TechnicalDto?> UpdateAsync(UpdateTechnicalDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<TechnicalDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TechnicalDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TechnicalDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<TechnicalDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<IEnumerable<TechnicalDto>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<IEnumerable<TechnicalDto>> GetBySpecialtyAsync(string specialty, CancellationToken cancellationToken = default);
        Task<IEnumerable<TechnicalDto>> GetByMinimumExperienceAsync(int minExperience, CancellationToken cancellationToken = default);
        Task<IEnumerable<TechnicalDto>> GetByExperienceRangeAsync(int minExperience, int maxExperience, CancellationToken cancellationToken = default);
        Task<TechnicalDto?> GetByIdWithAssessmentsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TechnicalDto>> GetTechnicalsWithMaintenanceAsync(CancellationToken cancellationToken = default);
        Task<int> GetMaintenanceCountAsync(Guid technicalId, CancellationToken cancellationToken = default);
        Task<int> GetAssessmentCountAsync(Guid technicalId, CancellationToken cancellationToken = default);
        Task<decimal?> GetAverageAssessmentScoreAsync(Guid technicalId, CancellationToken cancellationToken = default);
    }
}