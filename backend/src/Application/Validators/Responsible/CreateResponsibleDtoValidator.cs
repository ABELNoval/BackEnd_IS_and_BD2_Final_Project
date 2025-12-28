
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Application.DTOs.Responsible;
using Domain.Interfaces;

namespace Application.Validators.Responsible
{
    /// <summary>
    /// Validator for <see cref="CreateResponsibleDto"/>.
    /// Ensures all required fields are present and valid, checks uniqueness of email, and existence of related department asynchronously.
    /// </summary>
    public class CreateResponsibleDtoValidator : AbstractValidator<CreateResponsibleDto>
    {
        private readonly IResponsibleRepository _responsibleRepository;
        private readonly IDepartmentRepository _departmentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateResponsibleDtoValidator"/> class.
        /// </summary>
        /// <param name="responsibleRepository">Repository for responsible uniqueness checks.</param>
        /// <param name="departmentRepository">Repository for department existence checks.</param>
        public CreateResponsibleDtoValidator(IResponsibleRepository responsibleRepository, IDepartmentRepository departmentRepository)
        {
            _responsibleRepository = responsibleRepository;
            _departmentRepository = departmentRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Responsible name is required.")
                .MaximumLength(100).WithMessage("Responsible name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Valid email address is required.")
                .MaximumLength(150).WithMessage("Email cannot exceed 150 characters.")
                .MustAsync(EmailIsUniqueAsync).WithMessage("A responsible with this email already exists.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage("Department ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Department ID cannot be empty.")
                .MustAsync(DepartmentExistsAsync).WithMessage("Department with the specified ID does not exist.");
        }

        /// <summary>
        /// Checks asynchronously if the email is unique among responsibles.
        /// </summary>
        private async Task<bool> EmailIsUniqueAsync(string email, CancellationToken cancellationToken)
        {
            var existing = await _responsibleRepository.GetByEmailAsync(email, cancellationToken);
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