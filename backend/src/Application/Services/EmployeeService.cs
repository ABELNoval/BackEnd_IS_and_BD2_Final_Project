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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto, CancellationToken cancellationToken = default)
        {
            // 1️⃣ Validar DTO (Application Layer)
            var validator = new CreateEmployeeDtoValidator();
            var validationResult = await validator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors);

            // 2️⃣ Domain valida y puede lanzar InvalidEntityException
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

        public async Task<EmployeeDto?> UpdateAsync(UpdateEmployeeDto dto, CancellationToken cancellationToken = default)
        {
            // Validar
            var validator = new UpdateEmployeeDtoValidator();
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

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var existing = await _employeeRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                throw new EntityNotFoundException(nameof(Employee), id);

            await _employeeRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<EmployeeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var entity = await _employeeRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                throw new EntityNotFoundException(nameof(Employee), id);
            return _mapper.Map<EmployeeDto>(entity);
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _employeeRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<EmployeeDto>>(entities);
        }

        public async Task<IEnumerable<EmployeeDto>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            var entities = await _employeeRepository.FilterAsync(query, cancellationToken);
            return _mapper.Map<IEnumerable<EmployeeDto>>(entities);
        }
    }
}