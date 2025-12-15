using System;
using FluentValidation;
using Application.DTOs.Technical;

namespace Application.Validators.Technical
{
    /// <summary>
    /// Validator for CreateTechnicalDto. Performs basic and uniqueness validations for technical creation.
    /// </summary>
    public class CreateTechnicalDtoValidator : AbstractValidator<CreateTechnicalDto>
    {
        private readonly Domain.Interfaces.ITechnicalRepository _technicalRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTechnicalDtoValidator"/> class.
        /// </summary>
        /// <param name="technicalRepo">The technical repository for uniqueness checks.</param>
        public CreateTechnicalDtoValidator(Domain.Interfaces.ITechnicalRepository technicalRepo)
        {
            _technicalRepo = technicalRepo;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Technical name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MaximumLength(150).WithMessage("Email cannot exceed 150 characters.")
                .MustAsync(EmailIsUnique).WithMessage("A technical with this email already exists.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.Experience)
                .GreaterThanOrEqualTo(0).WithMessage("Experience cannot be negative.")
                .LessThanOrEqualTo(50).WithMessage("Experience cannot exceed 50 years.");

            RuleFor(x => x.Specialty)
                .NotEmpty().WithMessage("Specialty is required.")
                .MaximumLength(100).WithMessage("Specialty cannot exceed 100 characters.")
                .Matches("^[a-zA-Z\\s]+$").WithMessage("Specialty can only contain letters and spaces.");
        }

        /// <summary>
        /// Checks if the technical email is unique in the database.
        /// </summary>
        private async System.Threading.Tasks.Task<bool> EmailIsUnique(string email, System.Threading.CancellationToken ct)
        {
            var existing = await _technicalRepo.FilterAsync($"Email == \"{email}\"", ct);
            return existing == null || !existing.GetEnumerator().MoveNext();
        }
    }
}