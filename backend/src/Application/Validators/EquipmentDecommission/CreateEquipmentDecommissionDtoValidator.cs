using FluentValidation;
using Application.DTOs.EquipmentDecommission;

namespace Application.Validators.EquipmentDecommission
{
    public class CreateEquipmentDecommissionDtoValidator : AbstractValidator<CreateEquipmentDecommissionDto>
    {
        public CreateEquipmentDecommissionDtoValidator()
        {
            RuleFor(x => x.EquipmentId)
                .NotEmpty().WithMessage("Equipment ID is required")
                .NotEqual(Guid.Empty).WithMessage("Equipment ID cannot be empty");

            RuleFor(x => x.TechnicalId)
                .NotEmpty().WithMessage("Technical ID is required")
                .NotEqual(Guid.Empty).WithMessage("Technical ID cannot be empty");

            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage("Department ID is required")
                .NotEqual(Guid.Empty).WithMessage("Department ID cannot be empty");

            RuleFor(x => x.DestinyTypeId)
                .NotEmpty().WithMessage("Destiny type is required")
                .InclusiveBetween(1, 3).WithMessage("Destiny type must be valid (1: Department, 2: Disposal, 3: Warehouse)");

            RuleFor(x => x.DecommissionDate)
                .NotEmpty().WithMessage("Decommission date is required")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Decommission date cannot be in the future");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Decommission reason is required")
                .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters");
        }
    }
}