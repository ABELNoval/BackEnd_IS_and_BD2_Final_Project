using FluentValidation;
using Application.DTOs.EquipmentType;

namespace Application.Validators.EquipmentType
{
    public class UpdateEquipmentTypeDtoValidator : AbstractValidator<UpdateEquipmentTypeDto>
    {
        public UpdateEquipmentTypeDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Equipment type ID is required")
                .NotEqual(Guid.Empty).WithMessage("Equipment type ID cannot be empty");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Equipment type name is required")
                .MaximumLength(100).WithMessage("Equipment type name cannot exceed 100 characters");
        }
    }
}