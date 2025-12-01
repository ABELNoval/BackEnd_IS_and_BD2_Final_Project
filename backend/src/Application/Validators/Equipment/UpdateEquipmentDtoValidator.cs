using FluentValidation;
using Application.DTOs.Equipment;

namespace Application.Validators.Equipment
{
    public class UpdateEquipmentDtoValidator : AbstractValidator<UpdateEquipmentDto>
    {
        public UpdateEquipmentDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Equipment ID is required")
                .NotEqual(Guid.Empty).WithMessage("Equipment ID cannot be empty");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Equipment name is required")
                .MaximumLength(200).WithMessage("Name cannot exceed 200 characters")
                .Matches("^[a-zA-Z0-9 \\-\\.]+$").WithMessage("Name can only contain letters, numbers, spaces, hyphens and dots");

            RuleFor(x => x.AcquisitionDate)
                .NotEmpty().WithMessage("Acquisition date is required")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Acquisition date cannot be in the future");

            RuleFor(x => x.EquipmentTypeId)
                .NotEmpty().WithMessage("Equipment type is required")
                .NotEqual(Guid.Empty).WithMessage("Equipment type ID cannot be empty");

            RuleFor(x => x.DepartmentId)
                .Must(id => !id.HasValue || id.Value != Guid.Empty)
                .WithMessage("Department ID cannot be empty if provided");
        }
    }
}