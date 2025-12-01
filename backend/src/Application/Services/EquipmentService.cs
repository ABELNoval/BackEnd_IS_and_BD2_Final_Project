using Application.DTOs.Equipment;
using Application.Interfaces.Services;
using Application.Validators.Equipment;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;

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

            var entity = _mapper.Map<Equipment>(dto);
            await _equipmentRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<EquipmentDto>(entity);
        }

        public async Task<EquipmentDto?> UpdateAsync(UpdateEquipmentDto dto, CancellationToken cancellationToken = default)
        {
            // Validar DTO usando FluentValidation
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existing = await _equipmentRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

            _mapper.Map(dto, existing);
            _equipmentRepository.Update(existing);
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

        public async Task<EquipmentDto?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var entity = await _equipmentRepository.GetByIdWithDetailsAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<EquipmentDto>(entity);
        }

        public async Task<IEnumerable<EquipmentDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<EquipmentDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            // Validación básica del nombre
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Equipment name cannot be empty", nameof(name));
            }

            var entity = await _equipmentRepository.GetByNameAsync(name, cancellationToken);
            return entity == null ? null : _mapper.Map<EquipmentDto>(entity);
        }

        public async Task<IEnumerable<EquipmentDto>> GetByEquipmentTypeIdAsync(Guid equipmentTypeId, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (equipmentTypeId == Guid.Empty)
            {
                throw new ArgumentException("Equipment type ID cannot be empty", nameof(equipmentTypeId));
            }

            var entities = await _equipmentRepository.GetByEquipmentTypeIdAsync(equipmentTypeId, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentDto>> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (departmentId == Guid.Empty)
            {
                throw new ArgumentException("Department ID cannot be empty", nameof(departmentId));
            }

            var entities = await _equipmentRepository.GetByDepartmentIdAsync(departmentId, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentDto>> GetByStateAsync(int stateId, CancellationToken cancellationToken = default)
        {
            // Validación del estado (asumiendo que los estados válidos son 0-3 o similar)
            if (stateId < 0 || stateId > 3) // Ajusta el rango según tus estados válidos
            {
                throw new ArgumentException("State ID must be between 0 and 3", nameof(stateId));
            }

            var entities = await _equipmentRepository.GetByStateAsync(stateId, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentDto>> GetByLocationTypeAsync(int locationTypeId, CancellationToken cancellationToken = default)
        {
            // Validación del tipo de ubicación (asumiendo que los tipos válidos son 1-3 o similar)
            if (locationTypeId < 1 || locationTypeId > 3) // Ajusta el rango según tus tipos válidos
            {
                throw new ArgumentException("Location type ID must be between 1 and 3", nameof(locationTypeId));
            }

            var entities = await _equipmentRepository.GetByLocationTypeAsync(locationTypeId, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentDto>> GetByAcquisitionDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            // Validación de fechas
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be later than end date", nameof(startDate));
            }

            var entities = await _equipmentRepository.GetByAcquisitionDateRangeAsync(startDate, endDate, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentDto>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
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

            var entities = await _equipmentRepository.GetAllPagedAsync(page, pageSize, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentDto>> GetOperativeByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (departmentId == Guid.Empty)
            {
                throw new ArgumentException("Department ID cannot be empty", nameof(departmentId));
            }

            var entities = await _equipmentRepository.GetOperativeByDepartmentIdAsync(departmentId, cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentDto>> GetEquipmentUnderMaintenanceAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentRepository.GetEquipmentUnderMaintenanceAsync(cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<IEnumerable<EquipmentDto>> GetWarehouseEquipmentAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _equipmentRepository.GetWarehouseEquipmentAsync(cancellationToken);
            return _mapper.Map<IEnumerable<EquipmentDto>>(entities);
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            // Validación básica del nombre
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Equipment name cannot be empty", nameof(name));
            }

            return await _equipmentRepository.ExistsByNameAsync(name, cancellationToken);
        }

        public async Task<int> CountByStateAsync(int stateId, CancellationToken cancellationToken = default)
        {
            // Validación del estado (asumiendo que los estados válidos son 0-3 o similar)
            if (stateId < 0 || stateId > 3) // Ajusta el rango según tus estados válidos
            {
                throw new ArgumentException("State ID must be between 0 and 3", nameof(stateId));
            }

            return await _equipmentRepository.CountByStateAsync(stateId, cancellationToken);
        }
    }
}