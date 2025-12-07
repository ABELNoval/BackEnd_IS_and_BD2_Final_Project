using FluentValidation;
using Application.DTOs.Department;

namespace Application.Validators.Department
{
    public class UpdateDepartmentDtoValidator : AbstractValidator<UpdateDepartmentDto>
    {
        public UpdateDepartmentDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Department ID is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Department name is required")
                .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters");

            RuleFor(x => x.SectionId)
                .NotEmpty().WithMessage("Section ID is required");
        }
    }
}