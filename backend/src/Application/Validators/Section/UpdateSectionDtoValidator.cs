using FluentValidation;
using Application.DTOs.Section;

namespace Application.Validators.Section
{
    /// <summary>
    /// Validator for UpdateSectionDto. Performs existence and uniqueness validations for section update.
    /// </summary>
    public class UpdateSectionDtoValidator : AbstractValidator<UpdateSectionDto>
    {
        private readonly Domain.Interfaces.ISectionRepository _sectionRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSectionDtoValidator"/> class.
        /// </summary>
        /// <param name="sectionRepo">The section repository for existence and uniqueness checks.</param>
        public UpdateSectionDtoValidator(Domain.Interfaces.ISectionRepository sectionRepo)
        {
            _sectionRepo = sectionRepo;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Section ID is required.")
                .MustAsync(SectionExists).WithMessage("The specified section does not exist.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Section name is required.")
                .MaximumLength(100).WithMessage("Section name cannot exceed 100 characters.")
                .MustAsync(NameIsUnique).WithMessage("A section with this name already exists.");
        }

        /// <summary>
        /// Checks if the section exists in the database.
        /// </summary>
        private async System.Threading.Tasks.Task<bool> SectionExists(System.Guid id, System.Threading.CancellationToken ct)
        {
            return await _sectionRepo.GetByIdAsync(id, ct) != null;
        }

        /// <summary>
        /// Checks if the section name is unique in the database (excluding the current ID).
        /// </summary>
        private async System.Threading.Tasks.Task<bool> NameIsUnique(UpdateSectionDto dto, string name, System.Threading.CancellationToken ct)
        {
            var existing = await _sectionRepo.FilterAsync($"Name == \"{name}\" && Id != Guid(\"{dto.Id}\")", ct);
            return existing == null || !existing.GetEnumerator().MoveNext();
        }
    }
}