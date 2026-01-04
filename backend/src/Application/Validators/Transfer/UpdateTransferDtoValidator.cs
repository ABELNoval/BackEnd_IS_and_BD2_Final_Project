using System;
using FluentValidation;
using Application.DTOs.Transfer;

namespace Application.Validators.Transfer
{
    /// <summary>
    /// Validator for UpdateTransferDto. Performs existence and business rule validations for transfer update.
    /// </summary>
    public class UpdateTransferDtoValidator : AbstractValidator<UpdateTransferDto>
    {
        private readonly Domain.Interfaces.ITransferRepository _transferRepo;
        private readonly Domain.Interfaces.IDepartmentRepository _departmentRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTransferDtoValidator"/> class.
        /// </summary>
        /// <param name="transferRepo">The transfer repository for existence checks.</param>
        /// <param name="departmentRepo">The department repository for existence checks.</param>
        public UpdateTransferDtoValidator(
            Domain.Interfaces.ITransferRepository transferRepo,
            Domain.Interfaces.IDepartmentRepository departmentRepo)
        {
            _transferRepo = transferRepo;
            _departmentRepo = departmentRepo;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Transfer ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Transfer ID cannot be empty.")
                .MustAsync(TransferExists).WithMessage("The specified transfer does not exist.");

            RuleFor(x => x.TransferDate)
                .NotEmpty().WithMessage("Transfer date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Transfer date cannot be in the future.");

            RuleFor(x => x.TargetDepartmentId)
                .NotEmpty().WithMessage("Target department ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Target department ID cannot be empty.")
                .MustAsync(DepartmentExists).WithMessage("The specified target department does not exist.");
        }

        /// <summary>
        /// Checks if the transfer exists in the database.
        /// </summary>
        private async System.Threading.Tasks.Task<bool> TransferExists(System.Guid id, System.Threading.CancellationToken ct)
        {
            return await _transferRepo.GetByIdAsync(id, ct) != null;
        }

        /// <summary>
        /// Checks if the department exists in the database.
        /// </summary>
        private async System.Threading.Tasks.Task<bool> DepartmentExists(System.Guid id, System.Threading.CancellationToken ct)
        {
            return await _departmentRepo.GetByIdAsync(id, ct) != null;
        }
    }
}