
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Application.DTOs.Employee;
using Domain.Interfaces;

namespace Application.Validators.Employee
{
    /// <summary>
    /// Validator for <see cref="CreateEmployeeDto"/>.
    /// Ensures all required fields are present and valid, checks uniqueness of email, and existence of related department asynchronously.
    /// </summary>
    public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateEmployeeDtoValidator"/> class.
        /// </summary>
        /// <param name="employeeRepository">Repository for employee uniqueness checks.</param>
        /// <param name="departmentRepository">Repository for department existence checks.</param>
        public CreateEmployeeDtoValidator(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Employee name is required.")
                .MaximumLength(100).WithMessage("Employee name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Valid email address is required.")
                .MaximumLength(150).WithMessage("Email cannot exceed 150 characters.")
                .MustAsync(EmailIsUniqueAsync).WithMessage("An employee with this email already exists.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage("Department ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Department ID cannot be empty.")
                .MustAsync(DepartmentExistsAsync).WithMessage("Department with the specified ID does not exist.");
        }

        /// <summary>
        /// Checks asynchronously if the email is unique among employees.
        /// </summary>
        private async Task<bool> EmailIsUniqueAsync(string email, CancellationToken cancellationToken)
        {
            var existing = await _employeeRepository.GetByEmailAsync(email, cancellationToken);
            return existing == null;
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