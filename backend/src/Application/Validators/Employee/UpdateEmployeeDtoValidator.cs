using FluentValidation;
using Application.DTOs.Employee;

namespace Application.Validators.Employee
{
    public class UpdateEmployeeDtoValidator : AbstractValidator<UpdateEmployeeDto>
    {
        public UpdateEmployeeDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Employee ID is required")
                .NotEqual(Guid.Empty).WithMessage("Employee ID cannot be empty");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Employee name is required")
                .MaximumLength(100).WithMessage("Employee name cannot exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Valid email address is required")
                .MaximumLength(150).WithMessage("Email cannot exceed 150 characters");
        }
    }
}