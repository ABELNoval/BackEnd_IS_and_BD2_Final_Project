using Application.DTOs.Equipment;

namespace Application.Interfaces.Services
{
    public interface IEquipmentService
    {
        Task<EquipmentDto> CreateAsync(CreateEquipmentDto dto, CancellationToken cancellationToken = default);
        Task<EquipmentDto?> UpdateAsync(UpdateEquipmentDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<EquipmentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<EquipmentDto?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<EquipmentDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<EquipmentDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<IEnumerable<EquipmentDto>> GetByEquipmentTypeIdAsync(Guid equipmentTypeId, CancellationToken cancellationToken = default);
        Task<IEnumerable<EquipmentDto>> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<EquipmentDto>> GetByStateAsync(int stateId, CancellationToken cancellationToken = default);
        Task<IEnumerable<EquipmentDto>> GetByLocationTypeAsync(int locationTypeId, CancellationToken cancellationToken = default);
        Task<IEnumerable<EquipmentDto>> GetByAcquisitionDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<IEnumerable<EquipmentDto>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<IEnumerable<EquipmentDto>> GetOperativeByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<EquipmentDto>> GetEquipmentUnderMaintenanceAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<EquipmentDto>> GetWarehouseEquipmentAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<int> CountByStateAsync(int stateId, CancellationToken cancellationToken = default);
    }
}