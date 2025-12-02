using Application.DTOs.Section;

namespace Application.Interfaces.Services
{
    public interface ISectionService
    {
        Task<SectionDto> CreateAsync(CreateSectionDto dto, CancellationToken cancellationToken = default);
        Task<SectionDto?> UpdateAsync(UpdateSectionDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<SectionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<SectionDto>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}