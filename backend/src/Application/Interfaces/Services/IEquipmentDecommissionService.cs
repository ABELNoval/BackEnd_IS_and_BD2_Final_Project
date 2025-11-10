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

        // Obtener bajas técnicas por equipo
        Task<IEnumerable<EquipmentDecommissionDto>> GetByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default);

        // Obtener bajas técnicas por técnico
        Task<IEnumerable<EquipmentDecommissionDto>> GetByTechnicalIdAsync(Guid technicalId, CancellationToken cancellationToken = default);

        // Obtener bajas técnicas por departamento
        Task<IEnumerable<EquipmentDecommissionDto>> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default);

        // Obtener bajas técnicas por rango de fechas
        Task<IEnumerable<EquipmentDecommissionDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

        // Obtener bajas técnicas por tipo de destino
        Task<IEnumerable<EquipmentDecommissionDto>> GetByDestinyTypeIdAsync(int destinyTypeId, CancellationToken cancellationToken = default);

        // Obtener bajas técnicas por destinatario
        Task<IEnumerable<EquipmentDecommissionDto>> GetByRecipientIdAsync(Guid recipientId, CancellationToken cancellationToken = default);

        // Obtener la última baja técnica de un equipo
        Task<EquipmentDecommissionDto?> GetLatestByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default);

        // Verificar si un equipo tiene bajas técnicas registradas
        Task<bool> HasDecommissionsAsync(Guid equipmentId, CancellationToken cancellationToken = default);
    }
}
