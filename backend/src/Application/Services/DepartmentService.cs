using Application.DTOs.Department;
using Application.Interfaces.Services;
using Application.Validators.Department;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateDepartmentDto> _createValidator;
        private readonly IValidator<UpdateDepartmentDto> _updateValidator;

        public DepartmentService(
            IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateDepartmentDto> createValidator,
            IValidator<UpdateDepartmentDto> updateValidator)
        {
            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto, CancellationToken cancellationToken = default)
        {
            // Validar DTO usando FluentValidation
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Department>(dto);
            await _departmentRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<DepartmentDto>(entity);
        }

        public async Task<DepartmentDto?> UpdateAsync(UpdateDepartmentDto dto, CancellationToken cancellationToken = default)
        {
            // Validar DTO usando FluentValidation
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existing = await _departmentRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

            _mapper.Map(dto, existing);
            _departmentRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<DepartmentDto>(existing);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var existing = await _departmentRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _departmentRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<DepartmentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var entity = await _departmentRepository.GetByIdAsync(id, cancellationToken);
            return entity == null ? null : _mapper.Map<DepartmentDto>(entity);
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _departmentRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<DepartmentDto>>(entities);
        }

        public async Task<IEnumerable<DepartmentDto>> GetBySectionIdAsync(Guid sectionId, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (sectionId == Guid.Empty)
            {
                throw new ArgumentException("Section ID cannot be empty", nameof(sectionId));
            }

            var entities = await _departmentRepository.GetBySectionIdAsync(sectionId);
            return _mapper.Map<IEnumerable<DepartmentDto>>(entities);
        }

        public async Task<IEnumerable<DepartmentDto>> GetByResponsibleIdAsync(Guid responsibleId, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (responsibleId == Guid.Empty)
            {
                throw new ArgumentException("Responsible ID cannot be empty", nameof(responsibleId));
            }

            var entities = await _departmentRepository.GetByResponsibleIdAsync(responsibleId);
            return _mapper.Map<IEnumerable<DepartmentDto>>(entities);
        }

        public async Task<DepartmentDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            // Validación básica del nombre
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Department name cannot be empty", nameof(name));
            }

            var entity = await _departmentRepository.GetByNameAsync(name);
            return entity == null ? null : _mapper.Map<DepartmentDto>(entity);
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            // Validación de paginación
            if (pageNumber < 1)
            {
                throw new ArgumentException("Page number must be greater than 0", nameof(pageNumber));
            }

            if (pageSize < 1 || pageSize > 100)
            {
                throw new ArgumentException("Page size must be between 1 and 100", nameof(pageSize));
            }

            var entities = await _departmentRepository.GetAllPagedAsync(pageNumber, pageSize);
            return _mapper.Map<IEnumerable<DepartmentDto>>(entities);
        }
    }
}