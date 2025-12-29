using Application.DTOs.Employee;
using Application.Interfaces.Services;
using Application.Validators.Employee;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using FluentValidation;

namespace Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new employee after validating the DTO.
        /// </summary>
        /// <param name="dto">The CreateEmployeeDto to validate and create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created EmployeeDto.</returns>
        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto, CancellationToken cancellationToken = default)
        {
            var validator = new CreateEmployeeDtoValidator(_employeeRepository, _departmentRepository);
            var validationResult = await validator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors);

            var entity = Employee.Create(
                dto.Name,
                Email.Create(dto.Email),
                PasswordHash.Create(dto.Password),
                dto.DepartmentId
            );

            await _employeeRepository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<EmployeeDto>(entity);
        }

        /// <summary>
        /// Updates an existing employee after validating the DTO.
        /// </summary>
        /// <param name="dto">The UpdateEmployeeDto to validate and update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated EmployeeDto.</returns>
        public async Task<EmployeeDto?> UpdateAsync(UpdateEmployeeDto dto, CancellationToken cancellationToken = default)
        {
            var validator = new UpdateEmployeeDtoValidator(_employeeRepository, _departmentRepository);
            var validationResult = await validator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors);

            var existing = await _employeeRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                throw new EntityNotFoundException(nameof(Employee), dto.Id);

            var updatedEmail = existing.Email.Update(dto.Email);
            var updatedPasswordHash = string.IsNullOrWhiteSpace(dto.Password) 
                ? existing.PasswordHash   
                : existing.PasswordHash.Update(dto.Password);

            existing.UpdateBasicInfo(
                dto.Name,
                updatedEmail,
                updatedPasswordHash,
                dto.DepartmentId
            );

            await _employeeRepository.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<EmployeeDto>(existing);
        }

        /// <summary>
        /// Deletes an employee by ID.
        /// </summary>
        /// <param name="id">The employee ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if deleted, otherwise throws EntityNotFoundException.</returns>
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var existing = await _employeeRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                throw new EntityNotFoundException(nameof(Employee), id);

            await _employeeRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Gets an employee by ID.
        /// </summary>
        /// <param name="id">The employee ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The EmployeeDto if found, otherwise throws EntityNotFoundException.</returns>
        public async Task<EmployeeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var entity = await _employeeRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                throw new EntityNotFoundException(nameof(Employee), id);
            return _mapper.Map<EmployeeDto>(entity);
        }

        /// <summary>
        /// Gets all employees.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A collection of EmployeeDto.</returns>
        public async Task<IEnumerable<EmployeeDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _employeeRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<EmployeeDto>>(entities);
        }

        /// <summary>
        /// Gets employees by department IDs.
        /// </summary>
        public async Task<IEnumerable<EmployeeDto>> GetByDepartmentIdsAsync(IEnumerable<Guid> departmentIds, CancellationToken cancellationToken = default)
        {
            if (departmentIds == null || !departmentIds.Any())
                return Enumerable.Empty<EmployeeDto>();

            var entities = await _employeeRepository.GetByDepartmentIdsAsync(departmentIds, cancellationToken);
            return _mapper.Map<IEnumerable<EmployeeDto>>(entities);
        }

        /// <summary>
        /// Filters employees by a query string.
        /// </summary>
        /// <param name="query">The filter query.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A collection of EmployeeDto matching the filter.</returns>
        public async Task<IEnumerable<EmployeeDto>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            var entities = await _employeeRepository.FilterAsync(query, cancellationToken);
            return _mapper.Map<IEnumerable<EmployeeDto>>(entities);
        }
    }
}