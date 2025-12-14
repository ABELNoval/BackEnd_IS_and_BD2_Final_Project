using Application.DTOs.Maintenance;
using Application.Interfaces.Services;
using Application.Validators.Maintenance;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IMaintenanceRepository _maintenanceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateMaintenanceDto> _createValidator;
        private readonly IValidator<UpdateMaintenanceDto> _updateValidator;

        public MaintenanceService(
            IEquipmentRepository equipmentRepository,
            IMaintenanceRepository maintenanceRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateMaintenanceDto> createValidator,
            IValidator<UpdateMaintenanceDto> updateValidator)
        {
            _equipmentRepository = equipmentRepository;
            _maintenanceRepository = maintenanceRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        /// <summary>
        /// Creates a new maintenance record by calling Equipment.AddMaintenance()
        /// </summary>
        public async Task<MaintenanceDto> CreateAsync(CreateMaintenanceDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Get the aggregate root Equipment
            var equipment = await _equipmentRepository.GetByIdAsync(dto.EquipmentId, cancellationToken);
            if (equipment == null)
                throw new ValidationException($"Equipment with ID {dto.EquipmentId} not found");

            // Equipment creates and manages the Maintenance entity
            equipment.AddMaintenance(
                dto.TechnicalId,
                dto.MaintenanceDate,
                dto.MaintenanceTypeId,
                dto.Cost);

            var maintenance = equipment.Maintenances.Last();

            await _maintenanceRepository.CreateAsync(maintenance);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<MaintenanceDto>(maintenance);
        }

        /// <summary>
        /// Updates a maintenance record
        /// </summary>
        public async Task<MaintenanceDto?> UpdateAsync(UpdateMaintenanceDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existing = await _maintenanceRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

            existing.UpdateBasicInfo(dto.MaintenanceDate, dto.MaintenanceTypeId, dto.Cost);

            await _maintenanceRepository.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<MaintenanceDto>(existing);
        }

        /// <summary>
        /// Deletes a maintenance record
        /// </summary>
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var existing = await _maintenanceRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _maintenanceRepository.DeleteAsync(id, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        /// <summary>
        /// Gets a maintenance record by ID
        /// </summary>
        public async Task<MaintenanceDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var maintenance = await _maintenanceRepository.GetByIdAsync(id, cancellationToken);
            return maintenance == null ? null : _mapper.Map<MaintenanceDto>(maintenance);
        }

        /// <summary>
        /// Gets all maintenance records
        /// </summary>
        public async Task<IEnumerable<MaintenanceDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var maintenances = await _maintenanceRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<MaintenanceDto>>(maintenances);
        }

        public async Task<IEnumerable<MaintenanceDto>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            var entities = await _maintenanceRepository.FilterAsync(query, cancellationToken);
            return _mapper.Map<IEnumerable<MaintenanceDto>>(entities);
        }
    }
}