using System;
using FluentValidation;
using Application.DTOs.Transfer;

namespace Application.Validators.Transfer
{
    public class CreateTransferDtoValidator : AbstractValidator<CreateTransferDto>
    {
        public CreateTransferDtoValidator()
        {
            RuleFor(x => x.EquipmentId)
                .NotEmpty().WithMessage("Equipment ID is required")
                .NotEqual(Guid.Empty).WithMessage("Equipment ID cannot be empty");

            RuleFor(x => x.SourceDepartmentId)
                .NotEmpty().WithMessage("Source department ID is required")
                .NotEqual(Guid.Empty).WithMessage("Source department ID cannot be empty");

            RuleFor(x => x.TargetDepartmentId)
                .NotEmpty().WithMessage("Target department ID is required")
                .NotEqual(Guid.Empty).WithMessage("Target department ID cannot be empty")
                .NotEqual(x => x.SourceDepartmentId).WithMessage("Target department cannot be the same as source department");

            RuleFor(x => x.TransferDate)
                .NotEmpty().WithMessage("Transfer date is required")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Transfer date cannot be in the future");
        }
    }
}