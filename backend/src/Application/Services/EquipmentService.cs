using Application.DTOs.Equipment;
using Application.Interfaces.Services;
using Application.Validators.Equipment;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using Application.Exceptions;
using Domain.ValueObjects;

namespace Application.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateEquipmentDto> _createValidator;
        private readonly IValidator<UpdateEquipmentDto> _updateValidator;

        public EquipmentService(
            IEquipmentRepository equipmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateEquipmentDto> createValidator,
            IValidator<UpdateEquipmentDto> updateValidator)
        {
            _equipmentRepository = equipmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        /// <summary>
        /// Creates a new equipment entity after validating the DTO.
        /// </summary>
        /// <param name="dto">The CreateEquipmentDto to validate and create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created EquipmentDto.</returns>
        public async Task<EquipmentDto> CreateAsync(CreateEquipmentDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors);
            var entity = _mapper.Map<Equipment>(dto);
            await _equipmentRepository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<EquipmentDto>(entity);
        }

        /// <summary>
        /// Updates an existing equipment entity after validating the DTO.
        /// </summary>
        /// <param name="dto">The UpdateEquipmentDto to validate and update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated EquipmentDto, or null if not found.</returns>
        public async Task<EquipmentDto?> UpdateAsync(UpdateEquipmentDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors);

            var existing = await _equipmentRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                throw new EntityNotFoundException(nameof(Equipment), dto.Id);

            existing.Update(
                dto.Name,
                dto.AcquisitionDate,
                dto.EquipmentTypeId,
                dto.DepartmentId,
                dto.StateId,
                dto.LocationTypeId
            );

            await _equipmentRepository.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<EquipmentDto>(existing);
        }


        /// <summary>
        /// Deletes an equipment entity by ID.
        /// </summary>
        /// <param name="id">The equipment ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if deleted, false otherwise.</returns>
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var existing = await _equipmentRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                throw new EntityNotFoundException(nameof(Equipment), id);

            await _equipmentRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Gets an equipment entity by ID.
        /// </summary>
        /// <param name="id">The equipment ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The EquipmentDto if found, otherwise throws EntityNotFoundException.</returns>
        public async Task<EquipmentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var entity = await _equipmentRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                throw new EntityNotFoundException(nameof(Equipment), id);
            return _mapper.Map<EquipmentDto>(entity);
        }

        public async Task<IEnumerable<EquipmentDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentDto>> GetByDepartmentIdsAsync(IEnumerable<Guid> departmentIds, CancellationToken cancellationToken = default)
        {
            if (departmentIds == null || !departmentIds.Any())
                return Enumerable.Empty<EquipmentDto>();

            var ids = string.Join(",", departmentIds.Select(id => $"\"{id}\""));
            var query = $"DepartmentId in ({ids})";
            
            var entities = await _equipmentRepository.FilterAsync(query, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentDto>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentRepository.FilterAsync(query, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }
    }
}