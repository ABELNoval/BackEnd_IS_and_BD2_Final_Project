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
        Task<SectionDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<IEnumerable<SectionDto>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<IEnumerable<SectionDto>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default);
        Task<IEnumerable<SectionDto>> GetAllOrderedByNameAsync(CancellationToken cancellationToken = default);
        Task<int> GetDepartmentCountBySectionIdAsync(Guid sectionId, CancellationToken cancellationToken = default);
        Task<bool> HasDepartmentsAsync(Guid sectionId, CancellationToken cancellationToken = default);
    }
}