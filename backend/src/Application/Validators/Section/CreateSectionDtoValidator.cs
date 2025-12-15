using FluentValidation;
using Application.DTOs.Section;

namespace Application.Validators.Section
{
    /// <summary>
    /// Validator for CreateSectionDto. Performs basic and uniqueness validations for section creation.
    /// </summary>
    public class CreateSectionDtoValidator : AbstractValidator<CreateSectionDto>
    {
        private readonly Domain.Interfaces.ISectionRepository _sectionRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSectionDtoValidator"/> class.
        /// </summary>
        /// <param name="sectionRepo">The section repository for uniqueness checks.</param>
        public CreateSectionDtoValidator(Domain.Interfaces.ISectionRepository sectionRepo)
        {
            _sectionRepo = sectionRepo;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Section name is required.")
                .MaximumLength(100).WithMessage("Section name cannot exceed 100 characters.")
                .MustAsync(NameIsUnique).WithMessage("A section with this name already exists.");
        }

        /// <summary>
        /// Checks if the section name is unique in the database.
        /// </summary>
        private async System.Threading.Tasks.Task<bool> NameIsUnique(string name, System.Threading.CancellationToken ct)
        {
            var existing = await _sectionRepo.FilterAsync($"Name == \"{name}\"", ct);
            return existing == null || !existing.GetEnumerator().MoveNext();
        }
    }
}