using Application.DTOs.EquipmentType;

namespace Application.Interfaces.Services
{
    public interface IEquipmentTypeService
    {
        Task<EquipmentTypeDto> CreateAsync(CreateEquipmentTypeDto dto, CancellationToken cancellationToken = default);
        Task<EquipmentTypeDto?> UpdateAsync(UpdateEquipmentTypeDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<EquipmentTypeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<EquipmentTypeDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<EquipmentTypeDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<IEnumerable<EquipmentTypeDto>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<IEnumerable<EquipmentTypeDto>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default);
        Task<IEnumerable<EquipmentTypeDto>> GetAllOrderedByNameAsync(CancellationToken cancellationToken = default);
        Task<int> GetEquipmentCountByTypeIdAsync(Guid equipmentTypeId, CancellationToken cancellationToken = default);
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}