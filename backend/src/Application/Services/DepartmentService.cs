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
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var entity = Department.Create(dto.Name, dto.SectionId);

            await _departmentRepository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<DepartmentDto>(entity);
        }

        public async Task<DepartmentDto?> UpdateAsync(UpdateDepartmentDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existing = await _departmentRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                return null;

            existing.UpdateBasicInfo(dto.Name, dto.SectionId);

            await _departmentRepository.UpdateAsync(existing);
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

        public async Task<IEnumerable<DepartmentDto>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            var entities = await _departmentRepository.FilterAsync(query, cancellationToken);
            return _mapper.Map<IEnumerable<DepartmentDto>>(entities);
        }
    }
}