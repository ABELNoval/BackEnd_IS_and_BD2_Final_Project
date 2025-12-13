using Application.DTOs.EquipmentDecommission;
using Application.Interfaces.Services;
using Application.Validators.EquipmentDecommission;
using AutoMapper;
using Domain.Entities;
using Domain.Enumerations;
using Domain.Interfaces;
using Domain.Strategies;
using Domain.ValueObjects;
using FluentValidation;

namespace Application.Services
{
    public class EquipmentDecommissionService : IEquipmentDecommissionService
    {
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IEquipmentDecommissionRepository _decommissionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateEquipmentDecommissionDto> _createValidator;
        private readonly IValidator<UpdateEquipmentDecommissionDto> _updateValidator;

        public EquipmentDecommissionService(
            IEquipmentRepository equipmentRepository,
            IEquipmentDecommissionRepository decommissionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateEquipmentDecommissionDto> createValidator,
            IValidator<UpdateEquipmentDecommissionDto> updateValidator)
        {
            _equipmentRepository = equipmentRepository;
            _decommissionRepository = decommissionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        /// <summary>
        /// Creates a new equipment decommission record by calling Equipment.AddDecommission()
        /// Follows CQS pattern: creates context (query-like), validates, then applies (command-like)
        /// </summary>
        public async Task<EquipmentDecommissionDto> CreateAsync(CreateEquipmentDecommissionDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Get the aggregate root Equipment
            var equipment = await _equipmentRepository.GetByIdAsync(dto.EquipmentId, cancellationToken);
            if (equipment == null)
                throw new ValidationException($"Equipment with ID {dto.EquipmentId} not found");

            // Step 1: Create context based on destiny type (validates common data)
            var destinyType = DestinyType.FromId(dto.DestinyTypeId);
            if (destinyType == null)
                throw new ValidationException($"Invalid destiny type ID: {dto.DestinyTypeId}");

            var context = CreateDecommissionContext(destinyType, dto);

            // Step 2: Create the destination strategy (factory pattern)
            var strategy = DestinationStrategyFactory.Create(destinyType);

            // Step 3: Equipment coordinates the decommission process
            equipment.AddDecommission(
                strategy,
                context,
                dto.TechnicalId,
                dto.Reason);

            // Step 4: Save the aggregate root, which includes the new decommission
            await _equipmentRepository.UpdateAsync(equipment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Return the last decommission added
            var decommission = equipment.Decommissions.Last();
            return _mapper.Map<EquipmentDecommissionDto>(decommission);
        }

        /// <summary>
        /// Updates an equipment decommission record
        /// </summary>
        public async Task<EquipmentDecommissionDto?> UpdateAsync(UpdateEquipmentDecommissionDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existing = await _decommissionRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

            existing.UpdateReason(dto.Reason);

            await _decommissionRepository.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<EquipmentDecommissionDto>(existing);
        }

        /// <summary>
        /// Deletes an equipment decommission record
        /// </summary>
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var existing = await _decommissionRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _decommissionRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Gets an equipment decommission record by ID
        /// </summary>
        public async Task<EquipmentDecommissionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var entity = await _decommissionRepository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<EquipmentDecommissionDto>(entity);
        }

        /// <summary>
        /// Gets all equipment decommission records
        /// </summary>
        public async Task<IEnumerable<EquipmentDecommissionDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _decommissionRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDecommissionDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentDecommissionDto>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            var entities = await _decommissionRepository.FilterAsync(query, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDecommissionDto>>(entities);
        }

        #region Helper Methods

        /// <summary>
        /// Creates a DecommissionContext based on the destiny type
        /// </summary>
        private DecommissionContext CreateDecommissionContext(DestinyType destinyType, CreateEquipmentDecommissionDto dto)
        {
            return destinyType.Id switch
            {
                1 => DecommissionContext.ForDisposal(dto.ResponsibleId ?? Guid.Empty, dto.TransferDate),
                2 => DecommissionContext.ForDepartment(
                    dto.TargetDepartmentId ?? throw new ValidationException("Target department is required for transfer destiny"),
                    dto.ResponsibleId ?? Guid.Empty,
                    dto.TransferDate),
                3 => DecommissionContext.ForDepartment(
                    dto.TargetDepartmentId ?? throw new ValidationException("Target department is required for department destiny"),
                    dto.ResponsibleId ?? Guid.Empty,
                    dto.TransferDate),
                4 => DecommissionContext.ForWarehouse(dto.ResponsibleId ?? Guid.Empty, dto.TransferDate),
                _ => throw new ValidationException($"Invalid destiny type ID: {destinyType.Id}")
            };
        }

        #endregion
    }
}