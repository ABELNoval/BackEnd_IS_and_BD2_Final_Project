using FluentValidation;
using Application.DTOs.Technical;

namespace Application.Validators.Technical
{
    public class UpdateTechnicalDtoValidator : AbstractValidator<UpdateTechnicalDto>
    {
        public UpdateTechnicalDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Technical ID is required")
                .NotEqual(Guid.Empty).WithMessage("Technical ID cannot be empty");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Technical name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Valid email address is required")
                .MaximumLength(150).WithMessage("Email cannot exceed 150 characters");

            RuleFor(x => x.Experience)
                .GreaterThanOrEqualTo(0).WithMessage("Experience cannot be negative")
                .LessThanOrEqualTo(50).WithMessage("Experience cannot exceed 50 years");

            RuleFor(x => x.Specialty)
                .NotEmpty().WithMessage("Specialty is required")
                .MaximumLength(100).WithMessage("Specialty cannot exceed 100 characters")
                .Matches("^[a-zA-Z\\s]+$").WithMessage("Specialty can only contain letters and spaces");
        }
    }
}