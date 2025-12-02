using FluentValidation;
using Application.DTOs.Maintenance;

namespace Application.Validators.Maintenance
{
    public class CreateMaintenanceDtoValidator : AbstractValidator<CreateMaintenanceDto>
    {
        public CreateMaintenanceDtoValidator()
        {
            RuleFor(x => x.EquipmentId)
                .NotEmpty().WithMessage("Equipment ID is required")
                .NotEqual(Guid.Empty).WithMessage("Equipment ID cannot be empty");

            RuleFor(x => x.TechnicalId)
                .NotEmpty().WithMessage("Technical ID is required")
                .NotEqual(Guid.Empty).WithMessage("Technical ID cannot be empty");

            RuleFor(x => x.MaintenanceDate)
                .NotEmpty().WithMessage("Maintenance date is required")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Maintenance date cannot be in the future");

            RuleFor(x => x.MaintenanceTypeId)
                .NotEmpty().WithMessage("Maintenance type is required")
                .InclusiveBetween(1, 4).WithMessage("Maintenance type must be valid (1: Preventive, 2: Corrective, 3: Predictive, 4: Emergency)");

            RuleFor(x => x.Cost)
                .GreaterThanOrEqualTo(0).WithMessage("Cost cannot be negative")
                .LessThanOrEqualTo(1000000).WithMessage("Cost cannot exceed 1,000,000");
        }
    }
}