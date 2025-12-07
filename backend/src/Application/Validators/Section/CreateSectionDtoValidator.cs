using FluentValidation;
using Application.DTOs.Section;

namespace Application.Validators.Section
{
    public class CreateSectionDtoValidator : AbstractValidator<CreateSectionDto>
    {
        public CreateSectionDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Section name is required")
                .MaximumLength(100).WithMessage("Section name cannot exceed 100 characters");

            RuleFor(x => x.ResponsibleId)
                .NotEmpty().WithMessage("Responsible ID is required");
        }
    }
}