using Application.DTOs.EquipmentDecommission;

namespace Application.Interfaces.Services
{
    public interface IEquipmentDecommissionService
    {
        // Crear baja técnica
        Task<EquipmentDecommissionDto> CreateAsync(CreateEquipmentDecommissionDto dto, CancellationToken cancellationToken = default);

        // Actualizar baja técnica
        Task<EquipmentDecommissionDto?> UpdateAsync(UpdateEquipmentDecommissionDto dto, CancellationToken cancellationToken = default);

        // Eliminar baja técnica
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        // Obtener baja técnica por Id
        Task<EquipmentDecommissionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        // Obtener todas las bajas técnicas
        Task<IEnumerable<EquipmentDecommissionDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<EquipmentDecommissionDto>> FilterAsync(string query, CancellationToken cancellationToken = default);
    }
}