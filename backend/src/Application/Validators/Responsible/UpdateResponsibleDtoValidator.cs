
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Application.DTOs.Responsible;
using Domain.Interfaces;

namespace Application.Validators.Responsible
{
    /// <summary>
    /// Validator for <see cref="UpdateResponsibleDto"/>.
    /// Ensures all required fields are present and valid, checks existence of responsible and department, and uniqueness of email asynchronously.
    /// </summary>
    public class UpdateResponsibleDtoValidator : AbstractValidator<UpdateResponsibleDto>
    {
        private readonly IResponsibleRepository _responsibleRepository;
        private readonly IDepartmentRepository _departmentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateResponsibleDtoValidator"/> class.
        /// </summary>
        /// <param name="responsibleRepository">Repository for responsible existence and uniqueness checks.</param>
        /// <param name="departmentRepository">Repository for department existence checks.</param>
        public UpdateResponsibleDtoValidator(IResponsibleRepository responsibleRepository, IDepartmentRepository departmentRepository)
        {
            _responsibleRepository = responsibleRepository;
            _departmentRepository = departmentRepository;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Responsible ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Responsible ID cannot be empty.")
                .MustAsync(ResponsibleExistsAsync).WithMessage("Responsible with the specified ID does not exist.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Responsible name is required.")
                .MaximumLength(100).WithMessage("Responsible name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Valid email address is required.")
                .MaximumLength(150).WithMessage("Email cannot exceed 150 characters.")
                .MustAsync(EmailIsUniqueAsync).WithMessage("A responsible with this email already exists.");

            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage("Department ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Department ID cannot be empty.")
                .MustAsync(DepartmentExistsAsync).WithMessage("Department with the specified ID does not exist.");
        }

        /// <summary>
        /// Checks asynchronously if the responsible exists.
        /// </summary>
        private async Task<bool> ResponsibleExistsAsync(Guid responsibleId, CancellationToken cancellationToken)
        {
            if (responsibleId == Guid.Empty) return false;
            var responsible = await _responsibleRepository.GetByIdAsync(responsibleId, cancellationToken);
            return responsible != null;
        }

        /// <summary>
        /// Checks asynchronously if the email is unique among responsibles (excluding the current ID).
        /// </summary>
        private async Task<bool> EmailIsUniqueAsync(UpdateResponsibleDto dto, string email, CancellationToken cancellationToken)
        {
            var existing = await _responsibleRepository.FilterAsync($"Email == \"{email}\" && Id != Guid(\"{dto.Id}\")", cancellationToken);
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