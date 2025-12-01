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
        Task<ResponsibleDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<ResponsibleDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<IEnumerable<ResponsibleDto>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<ResponsibleDto?> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ResponsibleDto>> GetResponsiblesWithTransfersAsync(CancellationToken cancellationToken = default);
        Task<int> GetTransferCountAsync(Guid responsibleId, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> IsManagingDepartmentAsync(Guid responsibleId, CancellationToken cancellationToken = default);
    }
}