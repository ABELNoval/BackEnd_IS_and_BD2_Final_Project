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
    }
}