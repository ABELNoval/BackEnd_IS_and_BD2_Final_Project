using System;
using FluentValidation;
using Application.DTOs.User;

namespace Application.Validators.User
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("User name is required")
                .MaximumLength(100).WithMessage("User name cannot exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Valid email address is required")
                .MaximumLength(150).WithMessage("Email cannot exceed 150 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");

            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("Role is required")
                .InclusiveBetween(1, 6).WithMessage("Role must be a valid value (1-6)");
        }
    }
}