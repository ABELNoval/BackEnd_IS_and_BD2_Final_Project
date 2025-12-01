using Application.DTOs.Maintenance;

namespace Application.Interfaces.Services
{
    public interface IMaintenanceService
    {
        // Crear un mantenimiento
        Task<MaintenanceDto> CreateAsync(CreateMaintenanceDto dto, CancellationToken cancellationToken = default);

        // Actualizar un mantenimiento
        Task<MaintenanceDto?> UpdateAsync(UpdateMaintenanceDto dto, CancellationToken cancellationToken = default);

        // Eliminar un mantenimiento
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        // Obtener un mantenimiento por Id
        Task<MaintenanceDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        // Obtener todos los mantenimientos
        Task<IEnumerable<MaintenanceDto>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}