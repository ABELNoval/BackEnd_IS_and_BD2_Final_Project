using FluentValidation;
using Application.DTOs.Director;

namespace Application.Validators.Director
{
    /// <summary>
    /// Validator for CreateDirectorDto. Performs basic and uniqueness validations for director creation.
    /// </summary>
    public class CreateDirectorDtoValidator : AbstractValidator<CreateDirectorDto>
    {
        private readonly Domain.Interfaces.IDirectorRepository _directorRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateDirectorDtoValidator"/> class.
        /// </summary>
        /// <param name="directorRepo">The director repository for uniqueness checks.</param>
        public CreateDirectorDtoValidator(Domain.Interfaces.IDirectorRepository directorRepo)
        {
            _directorRepo = directorRepo;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Director name is required.")
                .MaximumLength(100).WithMessage("Director name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MaximumLength(150).WithMessage("Email cannot exceed 150 characters.")
                .MustAsync(EmailIsUnique).WithMessage("A director with this email already exists.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
        }

        /// <summary>
        /// Checks if the director email is unique in the database.
        /// </summary>
        private async System.Threading.Tasks.Task<bool> EmailIsUnique(string email, System.Threading.CancellationToken ct)
        {
            var existing = await _directorRepo.GetByEmailAsync(email, ct);
            return existing == null;
        }
    }
}