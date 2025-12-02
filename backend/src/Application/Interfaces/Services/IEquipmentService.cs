using Application.DTOs.Equipment;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IEquipmentService
    {
        Task<EquipmentDto> CreateAsync(CreateEquipmentDto dto, CancellationToken cancellationToken = default);
        Task<EquipmentDto?> UpdateAsync(UpdateEquipmentDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<EquipmentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<EquipmentDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<EquipmentDto>> FilterAsync(string query, CancellationToken cancellationToken = default);
    }
}