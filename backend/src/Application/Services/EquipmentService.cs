using Application.DTOs.Equipment;
using Application.Interfaces.Services;
using Application.Validators.Equipment;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
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

        public async Task<EquipmentDto> CreateAsync(CreateEquipmentDto dto, CancellationToken cancellationToken = default)
        {
            // Validar DTO usando FluentValidation
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            Console.WriteLine("Creating Equipment State: " + dto.StateId);
            var entity = _mapper.Map<Equipment>(dto);
            await _equipmentRepository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<EquipmentDto>(entity);
        }

        public async Task<EquipmentDto?> UpdateAsync(UpdateEquipmentDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existing = await _equipmentRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

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


        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var existing = await _equipmentRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _equipmentRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<EquipmentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var entity = await _equipmentRepository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<EquipmentDto>(entity);
        }

        public async Task<IEnumerable<EquipmentDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentDto>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentRepository.FilterAsync(query, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }
    }
}