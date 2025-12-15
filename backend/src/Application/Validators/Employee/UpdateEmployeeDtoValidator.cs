
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Application.DTOs.Employee;
using Domain.Interfaces;

namespace Application.Validators.Employee
{
    /// <summary>
    /// Validator for <see cref="UpdateEmployeeDto"/>.
    /// Ensures all required fields are present and valid, checks existence of employee and department, and uniqueness of email asynchronously.
    /// </summary>
    public class UpdateEmployeeDtoValidator : AbstractValidator<UpdateEmployeeDto>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateEmployeeDtoValidator"/> class.
        /// </summary>
        /// <param name="employeeRepository">Repository for employee existence and uniqueness checks.</param>
        /// <param name="departmentRepository">Repository for department existence checks.</param>
        public UpdateEmployeeDtoValidator(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Employee ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Employee ID cannot be empty.")
                .MustAsync(EmployeeExistsAsync).WithMessage("Employee with the specified ID does not exist.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Employee name is required.")
                .MaximumLength(100).WithMessage("Employee name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Valid email address is required.")
                .MaximumLength(150).WithMessage("Email cannot exceed 150 characters.")
                .MustAsync(EmailIsUniqueAsync).WithMessage("An employee with this email already exists.");

            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage("Department ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Department ID cannot be empty.")
                .MustAsync(DepartmentExistsAsync).WithMessage("Department with the specified ID does not exist.");
        }

        /// <summary>
        /// Checks asynchronously if the employee exists.
        /// </summary>
        private async Task<bool> EmployeeExistsAsync(Guid employeeId, CancellationToken cancellationToken)
        {
            if (employeeId == Guid.Empty) return false;
            var employee = await _employeeRepository.GetByIdAsync(employeeId, cancellationToken);
            return employee != null;
        }

        /// <summary>
        /// Checks asynchronously if the email is unique among employees (excluding the current ID).
        /// </summary>
        private async Task<bool> EmailIsUniqueAsync(UpdateEmployeeDto dto, string email, CancellationToken cancellationToken)
        {
            var existing = await _employeeRepository.FilterAsync($"Email == \"{email}\" && Id != Guid(\"{dto.Id}\")", cancellationToken);
            return existing == null || !existing.GetEnumerator().MoveNext();
        }

        /// <summary>
        /// Checks asynchronously if the department exists.
        /// </summary>
        private async Task<bool> DepartmentExistsAsync(Guid departmentId, CancellationToken cancellationToken)
        {
            if (departmentId == Guid.Empty) return false;
            var department = await _departmentRepository.GetByIdAsync(departmentId, cancellationToken);
            return department != null;
        }
    }
}