using Application.DTOs.Responsible;

namespace Application.Interfaces.Services
{
    public interface IResponsibleService
    {
        Task<ResponsibleDto> CreateAsync(CreateResponsibleDto dto, CancellationToken cancellationToken = default);
        Task<ResponsibleDto?> UpdateAsync(UpdateResponsibleDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ResponsibleDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<ResponsibleDto>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}