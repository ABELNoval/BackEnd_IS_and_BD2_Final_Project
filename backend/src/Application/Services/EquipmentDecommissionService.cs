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
using Application.Exceptions;

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
        /// Creates a new equipment decommission record by calling Equipment.AddDecommission().
        /// </summary>
        /// <param name="dto">The CreateEquipmentDecommissionDto to validate and create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created EquipmentDecommissionDto.</returns>
        public async Task<EquipmentDecommissionDto> CreateAsync(CreateEquipmentDecommissionDto dto, CancellationToken cancellationToken = default)
        {
            // var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            // if (!validationResult.IsValid)
            //     throw new Application.Exceptions.ValidationException(validationResult.Errors);

            var equipment = await _equipmentRepository.GetByIdAsync(dto.EquipmentId, cancellationToken);
            if (equipment == null)
                throw new EntityNotFoundException(nameof(Equipment), dto.EquipmentId);

            var destinyType = DestinyType.FromId(dto.DestinyTypeId);
            if (destinyType == null)
                throw new EntityNotFoundException(nameof(DestinyType), dto.DestinyTypeId);

            var context = CreateDecommissionContext(destinyType, dto);
            var strategy = DestinationStrategyFactory.Create(destinyType);

            equipment.AddDecommission(
                strategy,
                context,
                dto.TechnicalId,
                dto.Reason);

            var decommission = equipment.Decommissions.Last();
            await _decommissionRepository.CreateAsync(decommission);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<EquipmentDecommissionDto>(decommission);
        }

        /// <summary>
        /// Updates an equipment decommission record after validating the DTO.
        /// </summary>
        /// <param name="dto">The UpdateEquipmentDecommissionDto to validate and update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated EquipmentDecommissionDto, or throws EntityNotFoundException if not found.</returns>
        public async Task<EquipmentDecommissionDto?> UpdateAsync(UpdateEquipmentDecommissionDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors);

            var existing = await _decommissionRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                throw new EntityNotFoundException(nameof(EquipmentDecommission), dto.Id);

            existing.UpdateReason(dto.Reason);
            await _decommissionRepository.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<EquipmentDecommissionDto>(existing);
        }

        /// <summary>
        /// Deletes an equipment decommission record by ID.
        /// </summary>
        /// <param name="id">The equipment decommission ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if deleted, otherwise throws EntityNotFoundException.</returns>
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var existing = await _decommissionRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                throw new EntityNotFoundException(nameof(EquipmentDecommission), id);

            await _decommissionRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Gets an equipment decommission record by ID.
        /// </summary>
        /// <param name="id">The equipment decommission ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The EquipmentDecommissionDto if found, otherwise throws EntityNotFoundException.</returns>
        public async Task<EquipmentDecommissionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var entity = await _decommissionRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                throw new EntityNotFoundException(nameof(EquipmentDecommission), id);
            return _mapper.Map<EquipmentDecommissionDto>(entity);
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

        /// <summary>
        /// Creates a DecommissionContext based on the destiny type.
        /// </summary>
        /// <param name="destinyType">The DestinyType.</param>
        /// <param name="dto">The CreateEquipmentDecommissionDto.</param>
        /// <returns>The DecommissionContext.</returns>
        private DecommissionContext CreateDecommissionContext(DestinyType destinyType, CreateEquipmentDecommissionDto dto)
        {
            return destinyType.Id switch
            {
                2 => DecommissionContext.ForDisposal(),
                1 => DecommissionContext.ForDepartment(
                    (dto.DepartmentId != Guid.Empty) ? dto.DepartmentId : throw new Application.Exceptions.ValidationException(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("DepartmentId", "DepartmentId is required for transfer destiny") }),
                    (dto.RecipientId != Guid.Empty) ? dto.RecipientId : throw new Application.Exceptions.ValidationException(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("RecipientId", "RecipientId is required for transfer destiny") }),
                    dto.DecommissionDate ?? DateTime.UtcNow),
                3 => DecommissionContext.ForWarehouse(
                    (dto.RecipientId != Guid.Empty) ? dto.RecipientId : throw new Application.Exceptions.ValidationException(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("RecipientId", "RecipientId is required for transfer destiny") }),
                    dto.DecommissionDate ?? DateTime.UtcNow),
                _ => throw new Application.Exceptions.ValidationException(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("DestinyTypeId", $"Invalid destiny type ID: {destinyType.Id}") })
            };
        }
    }
}