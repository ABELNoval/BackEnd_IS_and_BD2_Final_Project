using FluentValidation;
using Application.DTOs.Director;

namespace Application.Validators.Director
{
    public class UpdateDirectorDtoValidator : AbstractValidator<UpdateDirectorDto>
    {
        public UpdateDirectorDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Director ID is required")
                .NotEqual(Guid.Empty).WithMessage("Director ID cannot be empty");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Director name is required")
                .MaximumLength(100).WithMessage("Director name cannot exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Valid email address is required")
                .MaximumLength(150).WithMessage("Email cannot exceed 150 characters");
        }
    }
}