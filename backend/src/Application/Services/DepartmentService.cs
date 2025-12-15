using Application.DTOs.Department;
using Application.Interfaces.Services;
using Application.Validators.Department;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentService(
            IDepartmentRepository departmentRepository,
            ISectionRepository sectionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _sectionRepository = sectionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new department after validating the DTO.
        /// </summary>
        /// <param name="dto">The CreateDepartmentDto to validate and create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created DepartmentDto.</returns>
        public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto, CancellationToken cancellationToken = default)
        {
            var validator = new CreateDepartmentDtoValidator(_sectionRepository);
            var validationResult = await validator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors);

            var entity = Department.Create(dto.Name, dto.SectionId);
            await _departmentRepository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<DepartmentDto>(entity);
        }

        /// <summary>
        /// Updates an existing department after validating the DTO.
        /// </summary>
        /// <param name="dto">The UpdateDepartmentDto to validate and update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated DepartmentDto.</returns>
        public async Task<DepartmentDto?> UpdateAsync(UpdateDepartmentDto dto, CancellationToken cancellationToken = default)
        {
            var validator = new UpdateDepartmentDtoValidator(_departmentRepository, _sectionRepository);
            var validationResult = await validator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors);

            var existing = await _departmentRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                throw new EntityNotFoundException(nameof(Department), dto.Id);

            existing.UpdateBasicInfo(dto.Name, dto.SectionId);
            await _departmentRepository.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<DepartmentDto>(existing);
        }


        /// <summary>
        /// Deletes a department by ID.
        /// </summary>
        /// <param name="id">The department ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if deleted, otherwise throws EntityNotFoundException.</returns>
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var existing = await _departmentRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                throw new EntityNotFoundException(nameof(Department), id);

            await _departmentRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Gets a department by ID.
        /// </summary>
        /// <param name="id">The department ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The DepartmentDto if found, otherwise throws EntityNotFoundException.</returns>
        public async Task<DepartmentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var entity = await _departmentRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                throw new EntityNotFoundException(nameof(Department), id);
            return _mapper.Map<DepartmentDto>(entity);
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