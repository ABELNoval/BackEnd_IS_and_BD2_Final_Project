using System;
using FluentValidation;
using Application.DTOs.Transfer;

namespace Application.Validators.Transfer
{
    /// <summary>
    /// Validator for CreateTransferDto. Performs basic, existence, and cross-entity validations for transfer creation.
    /// </summary>
    public class CreateTransferDtoValidator : AbstractValidator<CreateTransferDto>
    {
        private readonly Domain.Interfaces.IEquipmentRepository _equipmentRepo;
        private readonly Domain.Interfaces.IDepartmentRepository _departmentRepo;
        private readonly Domain.Interfaces.IResponsibleRepository _responsibleRepo;
        private readonly Domain.Interfaces.ITechnicalRepository _technicalRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTransferDtoValidator"/> class.
        /// </summary>
        /// <param name="equipmentRepo">The equipment repository for existence and state checks.</param>
        /// <param name="departmentRepo">The department repository for existence checks.</param>
        /// <param name="responsibleRepo">The responsible repository for existence checks.</param>
        /// <param name="technicalRepo">The technical repository for recipient existence checks.</param>
        public CreateTransferDtoValidator(
            Domain.Interfaces.IEquipmentRepository equipmentRepo,
            Domain.Interfaces.IDepartmentRepository departmentRepo,
            Domain.Interfaces.IResponsibleRepository responsibleRepo,
            Domain.Interfaces.ITechnicalRepository technicalRepo)
        {
            _equipmentRepo = equipmentRepo;
            _departmentRepo = departmentRepo;
            _responsibleRepo = responsibleRepo;
            _technicalRepo = technicalRepo;

            // Basic validations
            RuleFor(x => x.EquipmentId)
                .NotEmpty().WithMessage("Equipment ID is required.");

            RuleFor(x => x.SourceDepartmentId)
                .NotEmpty().WithMessage("Source department ID is required.");

            RuleFor(x => x.TargetDepartmentId)
                .NotEmpty().WithMessage("Target department ID is required.")
                .NotEqual(x => x.SourceDepartmentId).WithMessage("Target department cannot be the same as source department.");

            RuleFor(x => x.ResponsibleId)
                .NotEmpty().WithMessage("Responsible ID is required.");

            RuleFor(x => x.RecipientId)
                .NotEmpty().WithMessage("Recipient ID is required.");

            RuleFor(x => x.TransferDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Transfer date cannot be in the future.");

            // Existence validations
            RuleFor(x => x.EquipmentId)
                .MustAsync(EquipmentExists)
                .WithMessage("The specified equipment does not exist.");

            RuleFor(x => x.SourceDepartmentId)
                .MustAsync(DepartmentExists)
                .WithMessage("The specified source department does not exist.");

            RuleFor(x => x.TargetDepartmentId)
                .MustAsync(DepartmentExists)
                .WithMessage("The specified target department does not exist.");

            RuleFor(x => x.ResponsibleId)
                .MustAsync(ResponsibleExists)
                .WithMessage("The specified responsible does not exist.");

            RuleFor(x => x.RecipientId)
                .MustAsync(ResponsibleExistsForRecipient)
                .WithMessage("The specified recipient (responsible) does not exist.");

            // Cross-entity: Equipment is not disposed
            RuleFor(x => x.EquipmentId)
                .MustAsync(EquipmentIsNotDisposed)
                .WithMessage("Cannot transfer equipment that is disposed.");

            // Cross-entity: Equipment is in source department
            RuleFor(x => x)
                .MustAsync(EquipmentIsInSourceDepartment)
                .WithMessage("The equipment is not currently in the source department.");
        }

        /// <summary>
        /// Checks if the equipment exists in the database.
        /// </summary>
        /// <param name="id">The equipment ID.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if the equipment exists; otherwise, false.</returns>
        private async System.Threading.Tasks.Task<bool> EquipmentExists(Guid id, System.Threading.CancellationToken ct)
        {
            return await _equipmentRepo.GetByIdAsync(id, ct) != null;
        }

        /// <summary>
        /// Checks if the department exists in the database.
        /// </summary>
        /// <param name="id">The department ID.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if the department exists; otherwise, false.</returns>
        private async System.Threading.Tasks.Task<bool> DepartmentExists(Guid id, System.Threading.CancellationToken ct)
        {
            return await _departmentRepo.GetByIdAsync(id, ct) != null;
        }

        /// <summary>
        /// Checks if the responsible exists in the database.
        /// </summary>
        /// <param name="id">The responsible ID.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if the responsible exists; otherwise, false.</returns>
        private async System.Threading.Tasks.Task<bool> ResponsibleExists(Guid id, System.Threading.CancellationToken ct)
        {
            return await _responsibleRepo.GetByIdAsync(id, ct) != null;
        }

        /// <summary>
        /// Checks if the technical (recipient) exists in the database.
        /// </summary>
        /// <param name="id">The technical ID.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if the technical exists; otherwise, false.</returns>
        private async System.Threading.Tasks.Task<bool> TechnicalExists(Guid id, System.Threading.CancellationToken ct)
        {
            return await _technicalRepo.GetByIdAsync(id, ct) != null;
        }

        /// <summary>
        /// Checks if the responsible (recipient) exists in the database.
        /// </summary>
        /// <param name="id">The responsible ID.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if the responsible exists; otherwise, false.</returns>
        private async System.Threading.Tasks.Task<bool> ResponsibleExistsForRecipient(Guid id, System.Threading.CancellationToken ct)
        {
            return await _responsibleRepo.GetByIdAsync(id, ct) != null;
        }

        /// <summary>
        /// Checks if the equipment is not disposed.
        /// </summary>
        /// <param name="equipmentId">The equipment ID.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if the equipment is not disposed; otherwise, false.</returns>
        private async System.Threading.Tasks.Task<bool> EquipmentIsNotDisposed(Guid equipmentId, System.Threading.CancellationToken ct)
        {
            var equipment = await _equipmentRepo.GetByIdAsync(equipmentId, ct);
            // Adjust this check to match your domain model for disposed state
            return equipment != null && (!equipment.GetType().GetProperty("State")?.GetValue(equipment)?.ToString().Equals("Disposed") ?? true);
        }

        /// <summary>
        /// Checks if the equipment is currently in the source department.
        /// </summary>
        /// <param name="dto">The CreateTransferDto instance.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if the equipment is in the source department; otherwise, false.</returns>
        private async System.Threading.Tasks.Task<bool> EquipmentIsInSourceDepartment(CreateTransferDto dto, System.Threading.CancellationToken ct)
        {
            var equipment = await _equipmentRepo.GetByIdAsync(dto.EquipmentId, ct);
            return equipment != null && equipment.GetType().GetProperty("DepartmentId")?.GetValue(equipment) is Guid depId && depId == dto.SourceDepartmentId;
        }
    }
}