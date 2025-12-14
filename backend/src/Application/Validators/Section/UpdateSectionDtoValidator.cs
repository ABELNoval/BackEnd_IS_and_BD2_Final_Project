using FluentValidation;
using Application.DTOs.Section;

namespace Application.Validators.Section
{
    public class UpdateSectionDtoValidator : AbstractValidator<UpdateSectionDto>
    {
        public UpdateSectionDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Section ID is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Section name is required")
                .MaximumLength(100).WithMessage("Section name cannot exceed 100 characters");
        }
    }
}