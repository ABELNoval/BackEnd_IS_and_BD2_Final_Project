using Application.DTOs.EquipmentType;
using Application.Interfaces.Services;
using Application.Validators.EquipmentType;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;

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

        public async Task<EquipmentTypeDto> CreateAsync(CreateEquipmentTypeDto dto, CancellationToken cancellationToken = default)
        {
            // Validar DTO usando FluentValidation
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<EquipmentType>(dto);
            await _equipmentTypeRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<EquipmentTypeDto>(entity);
        }

        public async Task<EquipmentTypeDto?> UpdateAsync(UpdateEquipmentTypeDto dto, CancellationToken cancellationToken = default)
        {
            // Validar DTO usando FluentValidation
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existing = await _equipmentTypeRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

            _mapper.Map(dto, existing);
            _equipmentTypeRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<EquipmentTypeDto>(existing);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var existing = await _equipmentTypeRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _equipmentTypeRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<EquipmentTypeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var entity = await _equipmentTypeRepository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<EquipmentTypeDto>(entity);
        }

        public async Task<IEnumerable<EquipmentTypeDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentTypeRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentTypeDto>>(entities);
        }

        public async Task<EquipmentTypeDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            // Validación básica del nombre
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Equipment type name cannot be empty", nameof(name));
            }

            var entity = await _equipmentTypeRepository.GetByNameAsync(name, cancellationToken);
            return entity == null ? null : _mapper.Map<EquipmentTypeDto>(entity);
        }

        public async Task<IEnumerable<EquipmentTypeDto>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            // Validación de paginación
            if (page < 1)
            {
                throw new ArgumentException("Page number must be greater than 0", nameof(page));
            }

            if (pageSize < 1 || pageSize > 100)
            {
                throw new ArgumentException("Page size must be between 1 and 100", nameof(pageSize));
            }

            var entities = await _equipmentTypeRepository.GetAllPagedAsync(page, pageSize, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentTypeDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentTypeDto>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            // Validación del término de búsqueda
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                throw new ArgumentException("Search term cannot be empty", nameof(searchTerm));
            }

            // Validación de longitud mínima para búsqueda
            if (searchTerm.Length < 2)
            {
                throw new ArgumentException("Search term must be at least 2 characters", nameof(searchTerm));
            }

            var entities = await _equipmentTypeRepository.SearchByNameAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentTypeDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentTypeDto>> GetAllOrderedByNameAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentTypeRepository.GetAllOrderedByNameAsync(cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentTypeDto>>(entities);
        }

        public async Task<int> GetEquipmentCountByTypeIdAsync(Guid equipmentTypeId, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (equipmentTypeId == Guid.Empty)
            {
                throw new ArgumentException("Equipment type ID cannot be empty", nameof(equipmentTypeId));
            }

            return await _equipmentTypeRepository.GetEquipmentCountByTypeIdAsync(equipmentTypeId, cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            // Validación básica del nombre
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Equipment type name cannot be empty", nameof(name));
            }

            return await _equipmentTypeRepository.ExistsByNameAsync(name, cancellationToken);
        }
    }
}