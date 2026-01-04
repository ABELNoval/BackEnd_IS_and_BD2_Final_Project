using FluentValidation;
using Application.DTOs.Director;

namespace Application.Validators.Director
{
    /// <summary>
    /// Validator for UpdateDirectorDto. Performs existence and uniqueness validations for director update.
    /// </summary>
    public class UpdateDirectorDtoValidator : AbstractValidator<UpdateDirectorDto>
    {
        private readonly Domain.Interfaces.IDirectorRepository _directorRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDirectorDtoValidator"/> class.
        /// </summary>
        /// <param name="directorRepo">The director repository for existence and uniqueness checks.</param>
        public UpdateDirectorDtoValidator(Domain.Interfaces.IDirectorRepository directorRepo)
        {
            _directorRepo = directorRepo;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Director ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Director ID cannot be empty.")
                .MustAsync(DirectorExists).WithMessage("The specified director does not exist.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Director name is required.")
                .MaximumLength(100).WithMessage("Director name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MaximumLength(150).WithMessage("Email cannot exceed 150 characters.")
                .MustAsync(EmailIsUnique).WithMessage("A director with this email already exists.");
        }

        /// <summary>
        /// Checks if the director exists in the database.
        /// </summary>
        private async System.Threading.Tasks.Task<bool> DirectorExists(System.Guid id, System.Threading.CancellationToken ct)
        {
            return await _directorRepo.GetByIdAsync(id, ct) != null;
        }

        /// <summary>
        /// Checks if the director email is unique in the database (excluding the current ID).
        /// </summary>
        private async System.Threading.Tasks.Task<bool> EmailIsUnique(UpdateDirectorDto dto, string email, System.Threading.CancellationToken ct)
        {
            var existing = await _directorRepo.GetByEmailAsync(email, ct);
            return existing == null || existing.Id == dto.Id;
        }
    }
}