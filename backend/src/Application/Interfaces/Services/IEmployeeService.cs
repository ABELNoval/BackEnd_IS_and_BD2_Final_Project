using Application.DTOs.Employee;

namespace Application.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto, CancellationToken cancellationToken = default);
        Task<EmployeeDto?> UpdateAsync(UpdateEmployeeDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<EmployeeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<EmployeeDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<EmployeeDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<EmployeeDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<IEnumerable<EmployeeDto>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}