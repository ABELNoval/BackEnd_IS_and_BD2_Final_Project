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
        Task<IEnumerable<EmployeeDto>> FilterAsync(string query, CancellationToken cancellationToken = default);
    }
}