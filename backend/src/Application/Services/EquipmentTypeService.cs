using Application.DTOs.EquipmentType;
using Application.Interfaces.Services;
using Application.Validators.EquipmentType;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using Application.Exceptions;

namespace Application.Services
{
    public class EquipmentTypeService : IEquipmentTypeService
    {
        private readonly IEquipmentTypeRepository _equipmentTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateEquipmentTypeDto> _createValidator;
        private readonly IValidator<UpdateEquipmentTypeDto> _updateValidator;

        public EquipmentTypeService(
            IEquipmentTypeRepository equipmentTypeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateEquipmentTypeDto> createValidator,
            IValidator<UpdateEquipmentTypeDto> updateValidator)
        {
            _equipmentTypeRepository = equipmentTypeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        /// <summary>
        /// Creates a new equipment type entity after validating the DTO.
        /// </summary>
        /// <param name="dto">The CreateEquipmentTypeDto to validate and create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created EquipmentTypeDto.</returns>
        public async Task<EquipmentTypeDto> CreateAsync(CreateEquipmentTypeDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors);

            var entity = EquipmentType.Create(dto.Name);
            await _equipmentTypeRepository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<EquipmentTypeDto>(entity);
        }

        /// <summary>
        /// Updates an existing equipment type entity after validating the DTO.
        /// </summary>
        /// <param name="dto">The UpdateEquipmentTypeDto to validate and update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated EquipmentTypeDto, or throws EntityNotFoundException if not found.</returns>
        public async Task<EquipmentTypeDto?> UpdateAsync(UpdateEquipmentTypeDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors);

            var existing = await _equipmentTypeRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                throw new EntityNotFoundException(nameof(EquipmentType), dto.Id);

            existing.UpdateName(dto.Name);
            await _equipmentTypeRepository.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<EquipmentTypeDto>(existing);
        }


        /// <summary>
        /// Deletes an equipment type entity by ID.
        /// </summary>
        /// <param name="id">The equipment type ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if deleted, otherwise throws EntityNotFoundException.</returns>
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var existing = await _equipmentTypeRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                throw new EntityNotFoundException(nameof(EquipmentType), id);

            await _equipmentTypeRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Gets an equipment type entity by ID.
        /// </summary>
        /// <param name="id">The equipment type ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The EquipmentTypeDto if found, otherwise throws EntityNotFoundException.</returns>
        public async Task<EquipmentTypeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var entity = await _equipmentTypeRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                throw new EntityNotFoundException(nameof(EquipmentType), id);
            return _mapper.Map<EquipmentTypeDto>(entity);
        }

        public async Task<IEnumerable<EquipmentTypeDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentTypeRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentTypeDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentTypeDto>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentTypeRepository.FilterAsync(query, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentTypeDto>>(entities);
        }
    }
}