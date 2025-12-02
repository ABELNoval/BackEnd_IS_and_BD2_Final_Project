using FluentValidation;
using Application.DTOs.EquipmentType;

namespace Application.Validators.EquipmentType
{
    public class CreateEquipmentTypeDtoValidator : AbstractValidator<CreateEquipmentTypeDto>
    {
        public CreateEquipmentTypeDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Equipment type name is required")
                .MaximumLength(100).WithMessage("Equipment type name cannot exceed 100 characters");
        }
    }
}