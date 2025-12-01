using System;
using FluentValidation;
using Application.DTOs.Transfer;

namespace Application.Validators.Transfer
{
    public class UpdateTransferDtoValidator : AbstractValidator<UpdateTransferDto>
    {
        public UpdateTransferDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Transfer ID is required")
                .NotEqual(Guid.Empty).WithMessage("Transfer ID cannot be empty");

            RuleFor(x => x.TransferDate)
                .NotEmpty().WithMessage("Transfer date is required")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Transfer date cannot be in the future");

            RuleFor(x => x.TargetDepartmentId)
                .NotEmpty().WithMessage("Target department ID is required")
                .NotEqual(Guid.Empty).WithMessage("Target department ID cannot be empty")
                .When(x => x.TargetDepartmentId != Guid.Empty);
        }
    }
}