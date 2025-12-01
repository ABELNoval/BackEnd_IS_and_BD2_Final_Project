using FluentValidation;
using Application.DTOs.EquipmentDecommission;

namespace Application.Validators.EquipmentDecommission
{
    public class UpdateEquipmentDecommissionDtoValidator : AbstractValidator<UpdateEquipmentDecommissionDto>
    {
        public UpdateEquipmentDecommissionDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Decommission ID is required")
                .NotEqual(Guid.Empty).WithMessage("Decommission ID cannot be empty");

            RuleFor(x => x.DecommissionDate)
                .NotEmpty().WithMessage("Decommission date is required")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Decommission date cannot be in the future");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Decommission reason is required")
                .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters")
                .Must(reason => reason.ToLower().Contains("technical failure") || 
                               reason.ToLower().Contains("irreparable failure") ||
                               reason.ToLower().Contains("obsolescence") ||
                               reason.ToLower().Contains("other"))
                .WithMessage("Reason must include valid causes like 'technical failure', 'irreparable failure', 'obsolescence' or 'other'");
        }
    }
}