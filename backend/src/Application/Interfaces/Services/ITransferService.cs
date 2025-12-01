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

        // Obtener transferencias por equipo
        Task<IEnumerable<TransferDto>> GetByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default);

        // Obtener transferencias por departamento de origen
        Task<IEnumerable<TransferDto>> GetBySourceDepartmentIdAsync(Guid sourceDepartmentId, CancellationToken cancellationToken = default);

        // Obtener transferencias por departamento destino
        Task<IEnumerable<TransferDto>> GetByTargetDepartmentIdAsync(Guid targetDepartmentId, CancellationToken cancellationToken = default);

        // Obtener transferencias por departamento (origen o destino)
        Task<IEnumerable<TransferDto>> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default);

        // Obtener transferencias por responsable
        Task<IEnumerable<TransferDto>> GetByResponsibleIdAsync(Guid responsibleId, CancellationToken cancellationToken = default);

        // Obtener transferencias por rango de fechas
        Task<IEnumerable<TransferDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

        // Obtener transferencias recientes
        Task<IEnumerable<TransferDto>> GetRecentTransfersAsync(int count, CancellationToken cancellationToken = default);

        // Obtener la última transferencia de un equipo
        Task<TransferDto?> GetLatestByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default);

        // Contar transferencias por equipo
        Task<int> CountByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default);

        // Contar transferencias por departamento
        Task<int> CountByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default);
    }
}
