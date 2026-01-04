
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Application.DTOs.Department;
using Domain.Interfaces;

namespace Application.Validators.Department
{
    /// <summary>
    /// Validator for <see cref="UpdateDepartmentDto"/>.
    /// Ensures all required fields are present and valid, and checks existence of related department and section asynchronously.
    /// </summary>
    public class UpdateDepartmentDtoValidator : AbstractValidator<UpdateDepartmentDto>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISectionRepository _sectionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDepartmentDtoValidator"/> class.
        /// </summary>
        /// <param name="departmentRepository">Repository for department existence checks.</param>
        /// <param name="sectionRepository">Repository for section existence checks.</param>
        public UpdateDepartmentDtoValidator(IDepartmentRepository departmentRepository, ISectionRepository sectionRepository)
        {
            _departmentRepository = departmentRepository;
            _sectionRepository = sectionRepository;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Department ID is required.")
                .MustAsync(DepartmentExistsAsync).WithMessage("Department with the specified ID does not exist.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Department name is required.")
                .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters.");

            RuleFor(x => x.SectionId)
                .NotEmpty().WithMessage("Section ID is required.")
                .MustAsync(SectionExistsAsync).WithMessage("Section with the specified ID does not exist.");
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