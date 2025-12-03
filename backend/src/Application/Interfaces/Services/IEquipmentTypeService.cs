using Application.DTOs.EquipmentType;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IEquipmentTypeService
    {
        Task<EquipmentTypeDto> CreateAsync(CreateEquipmentTypeDto dto, CancellationToken cancellationToken = default);
        Task<EquipmentTypeDto?> UpdateAsync(UpdateEquipmentTypeDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<EquipmentTypeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<EquipmentTypeDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<EquipmentTypeDto>> FilterAsync(string query, CancellationToken cancellationToken = default);
    }
}