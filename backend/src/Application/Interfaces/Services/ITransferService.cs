using Application.DTOs.Transfer;

namespace Application.Interfaces.Services
{
    public interface ITransferService
    {
        // Crear una transferencia
        Task<TransferDto> CreateAsync(CreateTransferDto dto, CancellationToken cancellationToken = default);

        // Actualizar una transferencia
        Task<TransferDto?> UpdateAsync(UpdateTransferDto dto, CancellationToken cancellationToken = default);

        // Eliminar una transferencia
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        // Obtener una transferencia por Id
        Task<TransferDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        // Obtener todas las transferencias
        Task<IEnumerable<TransferDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TransferDto>> GetByDepartmentIdsAsync(IEnumerable<Guid> departmentIds, CancellationToken cancellationToken = default);
        Task<IEnumerable<TransferDto>> FilterAsync(string query, CancellationToken cancellationToken = default);
    }
}