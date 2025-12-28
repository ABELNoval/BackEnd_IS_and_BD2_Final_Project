using Application.DTOs.Department;

namespace Application.Interfaces.Services
{
    public interface IDepartmentService
    {
        Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto, CancellationToken cancellationToken = default);
        Task<DepartmentDto?> UpdateAsync(UpdateDepartmentDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<DepartmentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<DepartmentDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<DepartmentDto>> GetBySectionIdAsync(Guid sectionId, CancellationToken cancellationToken = default);
        Task<IEnumerable<DepartmentDto>> FilterAsync(string query, CancellationToken cancellationToken = default);
    }
}