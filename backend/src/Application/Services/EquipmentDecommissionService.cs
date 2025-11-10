using Application.DTOs.EquipmentDecommission;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class EquipmentDecommissionService : IEquipmentDecommissionService
    {
        private readonly ITechnicalDowntimeRepository _decommissionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EquipmentDecommissionService(
            ITechnicalDowntimeRepository decommissionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _decommissionRepository = decommissionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Crear baja técnica
        public async Task<EquipmentDecommissionDto> CreateAsync(CreateEquipmentDecommissionDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<EquipmentDecommission>(dto);
            await _decommissionRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<EquipmentDecommissionDto>(entity);
        }

        // Actualizar baja técnica
        public async Task<EquipmentDecommissionDto?> UpdateAsync(UpdateEquipmentDecommissionDto dto, CancellationToken cancellationToken = default)
        {
            var existing = await _decommissionRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

            _mapper.Map(dto, existing);
            _decommissionRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<EquipmentDecommissionDto>(existing);
        }

        // Eliminar baja técnica
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var existing = await _decommissionRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _decommissionRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        // Obtener baja técnica por Id
        public async Task<EquipmentDecommissionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _decommissionRepository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<EquipmentDecommissionDto>(entity);
        }

        // Obtener todas las bajas técnicas
        public async Task<IEnumerable<EquipmentDecommissionDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _decommissionRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDecommissionDto>>(entities);
        }

        // Obtener bajas técnicas por equipo
        public async Task<IEnumerable<EquipmentDecommissionDto>> GetByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default)
        {
            var entities = await _decommissionRepository.GetByEquipmentIdAsync(equipmentId, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDecommissionDto>>(entities);
        }

        // Obtener bajas técnicas por técnico
        public async Task<IEnumerable<EquipmentDecommissionDto>> GetByTechnicalIdAsync(Guid technicalId, CancellationToken cancellationToken = default)
        {
            var entities = await _decommissionRepository.GetByTechnicalIdAsync(technicalId, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDecommissionDto>>(entities);
        }

        // Obtener bajas técnicas por departamento
        public async Task<IEnumerable<EquipmentDecommissionDto>> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            var entities = await _decommissionRepository.GetByDepartmentIdAsync(departmentId, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDecommissionDto>>(entities);
        }

        // Obtener bajas técnicas por rango de fechas
        public async Task<IEnumerable<EquipmentDecommissionDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var entities = await _decommissionRepository.GetByDateRangeAsync(startDate, endDate, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDecommissionDto>>(entities);
        }

        // Obtener bajas técnicas por tipo de destino
        public async Task<IEnumerable<EquipmentDecommissionDto>> GetByDestinyTypeIdAsync(int destinyTypeId, CancellationToken cancellationToken = default)
        {
            var entities = await _decommissionRepository.GetByDestinyTypeIdAsync(destinyTypeId, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDecommissionDto>>(entities);
        }

        // Obtener bajas técnicas por destinatario
        public async Task<IEnumerable<EquipmentDecommissionDto>> GetByRecipientIdAsync(Guid recipientId, CancellationToken cancellationToken = default)
        {
            var entities = await _decommissionRepository.GetByRecipientIdAsync(recipientId, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDecommissionDto>>(entities);
        }

        // Obtener la última baja técnica de un equipo
        public async Task<EquipmentDecommissionDto?> GetLatestByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default)
        {
            var entity = await _decommissionRepository.GetLatestByEquipmentIdAsync(equipmentId, cancellationToken);
            return entity == null ? null : _mapper.Map<EquipmentDecommissionDto>(entity);
        }

        // Verificar si un equipo tiene bajas técnicas registradas
        public async Task<bool> HasDecommissionsAsync(Guid equipmentId, CancellationToken cancellationToken = default)
        {
            return await _decommissionRepository.HasDecommissionsAsync(equipmentId, cancellationToken);
        }
    }
}
