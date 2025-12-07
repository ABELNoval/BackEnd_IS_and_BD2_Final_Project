using FluentValidation;
using Application.DTOs.Department;

namespace Application.Validators.Department
{
    public class CreateDepartmentDtoValidator : AbstractValidator<CreateDepartmentDto>
    {
        public CreateDepartmentDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Department name is required")
                .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters");

            RuleFor(x => x.SectionId)
                .NotEmpty().WithMessage("Section ID is required");
        }
    }
}