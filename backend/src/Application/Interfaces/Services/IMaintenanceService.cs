using Application.DTOs.Maintenance;
using Domain.Entities;

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
        
        Task<IEnumerable<MaintenanceDto>> FilterAsync(string query, CancellationToken cancellationToken = default);

        /// <summary>
        /// Complete a maintenance - sets end date, marks as completed, and changes equipment to Operative
        /// </summary>
        Task<MaintenanceDto> CompleteAsync(Guid maintenanceId, CancellationToken cancellationToken = default);
    }
}