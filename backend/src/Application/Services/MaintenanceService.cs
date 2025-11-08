using Application.DTOs.Maintenance;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IMaintenanceRepository _maintenanceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MaintenanceService(
            IMaintenanceRepository maintenanceRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _maintenanceRepository = maintenanceRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Obtener todos
        public async Task<IEnumerable<MaintenanceDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var maintenances = await _maintenanceRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<MaintenanceDto>>(maintenances);
        }

        // Obtener por Id
        public async Task<MaintenanceDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var maintenance = await _maintenanceRepository.GetByIdAsync(id, cancellationToken);
            return maintenance is null ? null : _mapper.Map<MaintenanceDto>(maintenance);
        }

        // Obtener por técnico
        public async Task<IEnumerable<MaintenanceDto>> GetByTechnicalIdAsync(Guid technicalId, CancellationToken cancellationToken = default)
        {
            var maintenances = await _maintenanceRepository.GetByTechnicalIdAsync(technicalId, cancellationToken);
            return _mapper.Map<IEnumerable<MaintenanceDto>>(maintenances);
        }

        // Obtener por equipo
        public async Task<IEnumerable<MaintenanceDto>> GetByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default)
        {
            var maintenances = await _maintenanceRepository.GetByEquipmentIdAsync(equipmentId, cancellationToken);
            return _mapper.Map<IEnumerable<MaintenanceDto>>(maintenances);
        }

        // Crear mantenimiento
        public async Task<MaintenanceDto> CreateAsync(CreateMaintenanceDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<Maintenance>(dto);
            await _maintenanceRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<MaintenanceDto>(entity);
        }

        // Actualizar mantenimiento
        public async Task<MaintenanceDto?> UpdateAsync(UpdateMaintenanceDto dto, CancellationToken cancellationToken = default)
        {
            var existing = await _maintenanceRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing is null)
                return null;

            _mapper.Map(dto, existing);
            _maintenanceRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<MaintenanceDto>(existing);
        }

        // Eliminar mantenimiento
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _maintenanceRepository.DeleteAsync(id, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
    }
}
