using FluentValidation;
using Application.DTOs.Maintenance;

namespace Application.Validators.Maintenance
{
    public class UpdateMaintenanceDtoValidator : AbstractValidator<UpdateMaintenanceDto>
    {
        public UpdateMaintenanceDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Maintenance ID is required")
                .NotEqual(Guid.Empty).WithMessage("Maintenance ID cannot be empty");

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