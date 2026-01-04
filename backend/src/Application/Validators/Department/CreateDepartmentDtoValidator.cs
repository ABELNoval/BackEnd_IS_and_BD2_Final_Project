
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Application.DTOs.Department;
using Domain.Interfaces;

namespace Application.Validators.Department
{
    /// <summary>
    /// Validator for <see cref="CreateDepartmentDto"/>.
    /// Ensures all required fields are present and valid, and checks existence of related section asynchronously.
    /// </summary>
    public class CreateDepartmentDtoValidator : AbstractValidator<CreateDepartmentDto>
    {
        private readonly ISectionRepository _sectionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateDepartmentDtoValidator"/> class.
        /// </summary>
        /// <param name="sectionRepository">Repository for section existence checks.</param>
        public CreateDepartmentDtoValidator(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Department name is required.")
                .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters.");

            RuleFor(x => x.SectionId)
                .NotEmpty().WithMessage("Section ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Section ID cannot be empty.")
                .MustAsync(SectionExistsAsync).WithMessage("Section with the specified ID does not exist.");
        }

        /// <summary>
        /// Checks asynchronously if the section exists.
        /// </summary>
        private async Task<bool> SectionExistsAsync(Guid sectionId, CancellationToken cancellationToken)
        {
            if (sectionId == Guid.Empty) return false;
            var section = await _sectionRepository.GetByIdAsync(sectionId, cancellationToken);
            return section != null;
        }
    }
}