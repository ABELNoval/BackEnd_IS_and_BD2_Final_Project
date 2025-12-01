using FluentValidation;
using Application.DTOs.Responsible;

namespace Application.Validators.Responsible
{
    public class CreateResponsibleDtoValidator : AbstractValidator<CreateResponsibleDto>
    {
        public CreateResponsibleDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Responsible name is required")
                .MaximumLength(100).WithMessage("Responsible name cannot exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Valid email address is required")
                .MaximumLength(150).WithMessage("Email cannot exceed 150 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");
        }
    }
}