using FluentValidation;
using Application.DTOs.Department;

namespace Application.Validators.Department
{
    public class UpdateDepartmentDtoValidator : AbstractValidator<UpdateDepartmentDto>
    {
        public UpdateDepartmentDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Department ID is required")
                .NotEqual(Guid.Empty).WithMessage("Department ID cannot be empty");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Department name is required")
                .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters");

            RuleFor(x => x.SectionId)
                .NotEmpty().WithMessage("Section ID is required")
                .NotEqual(Guid.Empty).WithMessage("Section ID cannot be empty");

            RuleFor(x => x.ResponsibleId)
                .NotEmpty().WithMessage("Responsible ID is required")
                .NotEqual(Guid.Empty).WithMessage("Responsible ID cannot be empty");
        }
    }
}