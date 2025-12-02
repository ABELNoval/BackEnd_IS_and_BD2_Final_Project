using FluentValidation;
using Application.DTOs.Responsible;

namespace Application.Validators.Responsible
{
    public class UpdateResponsibleDtoValidator : AbstractValidator<UpdateResponsibleDto>
    {
        public UpdateResponsibleDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Responsible ID is required")
                .NotEqual(Guid.Empty).WithMessage("Responsible ID cannot be empty");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Responsible name is required")
                .MaximumLength(100).WithMessage("Responsible name cannot exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Valid email address is required")
                .MaximumLength(150).WithMessage("Email cannot exceed 150 characters");
        }
    }
}