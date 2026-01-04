using FluentValidation;
using Application.DTOs.Technical;

namespace Application.Validators.Technical
{
    /// <summary>
    /// Validator for UpdateTechnicalDto. Performs existence and uniqueness validations for technical update.
    /// </summary>
    public class UpdateTechnicalDtoValidator : AbstractValidator<UpdateTechnicalDto>
    {
        private readonly Domain.Interfaces.ITechnicalRepository _technicalRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTechnicalDtoValidator"/> class.
        /// </summary>
        /// <param name="technicalRepo">The technical repository for existence and uniqueness checks.</param>
        public UpdateTechnicalDtoValidator(Domain.Interfaces.ITechnicalRepository technicalRepo)
        {
            _technicalRepo = technicalRepo;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Technical ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Technical ID cannot be empty.")
                .MustAsync(TechnicalExists).WithMessage("The specified technical does not exist.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Technical name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MaximumLength(150).WithMessage("Email cannot exceed 150 characters.")
                .MustAsync(EmailIsUnique).WithMessage("A technical with this email already exists.");

            RuleFor(x => x.Experience)
                .GreaterThanOrEqualTo(0).WithMessage("Experience cannot be negative.")
                .LessThanOrEqualTo(50).WithMessage("Experience cannot exceed 50 years.");

            RuleFor(x => x.Specialty)
                .NotEmpty().WithMessage("Specialty is required.")
                .MaximumLength(100).WithMessage("Specialty cannot exceed 100 characters.")
                .Matches("^[a-zA-Z\\s]+$").WithMessage("Specialty can only contain letters and spaces.");
        }

        /// <summary>
        /// Checks if the technical exists in the database.
        /// </summary>
        private async System.Threading.Tasks.Task<bool> TechnicalExists(System.Guid id, System.Threading.CancellationToken ct)
        {
            return await _technicalRepo.GetByIdAsync(id, ct) != null;
        }

        /// <summary>
        /// Checks if the technical email is unique in the database (excluding the current ID).
        /// </summary>
        private async System.Threading.Tasks.Task<bool> EmailIsUnique(UpdateTechnicalDto dto, string email, System.Threading.CancellationToken ct)
        {
            var existing = await _technicalRepo.GetByEmailAsync(email, ct);
            return existing == null || existing.Id == dto.Id;
        }
    }
}