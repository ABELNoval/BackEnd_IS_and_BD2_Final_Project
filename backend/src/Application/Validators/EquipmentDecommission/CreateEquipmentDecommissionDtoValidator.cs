using System;
using FluentValidation;
using Application.DTOs.EquipmentDecommission;

namespace Application.Validators.EquipmentDecommission
{
    /// <summary>
    /// Validator for CreateEquipmentDecommissionDto. Performs basic, existence, and conditional validations for equipment decommission creation.
    /// </summary>
    public class CreateEquipmentDecommissionDtoValidator : AbstractValidator<CreateEquipmentDecommissionDto>
    {
        private readonly Domain.Interfaces.IEquipmentRepository _equipmentRepo;
        private readonly Domain.Interfaces.ITechnicalRepository _technicalRepo;
        private readonly Domain.Interfaces.IDepartmentRepository _departmentRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateEquipmentDecommissionDtoValidator"/> class.
        /// </summary>
        /// <param name="equipmentRepo">The equipment repository for existence checks.</param>
        /// <param name="technicalRepo">The technical repository for existence checks.</param>
        /// <param name="departmentRepo">The department repository for existence checks.</param>
        public CreateEquipmentDecommissionDtoValidator(
            Domain.Interfaces.IEquipmentRepository equipmentRepo,
            Domain.Interfaces.ITechnicalRepository technicalRepo,
            Domain.Interfaces.IDepartmentRepository departmentRepo)
        {
            _equipmentRepo = equipmentRepo;
            _technicalRepo = technicalRepo;
            _departmentRepo = departmentRepo;

            // Basic validations
            RuleFor(x => x.EquipmentId)
                .NotEmpty().WithMessage("Equipment ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Equipment ID cannot be empty.");

            RuleFor(x => x.TechnicalId)
                .NotEmpty().WithMessage("Technical ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Technical ID cannot be empty.");

            RuleFor(x => x.DestinyTypeId)
                .NotEmpty().WithMessage("Destiny type is required.")
                .InclusiveBetween(1, 3).WithMessage("Destiny type must be valid (1: Department, 2: Disposal, 3: Warehouse).");

            RuleFor(x => x.DecommissionDate)
                .NotEmpty().WithMessage("Decommission date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Decommission date cannot be in the future.");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Decommission reason is required.")
                .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters.");

            // Existence validations
            RuleFor(x => x.EquipmentId)
                .MustAsync(EquipmentExists)
                .WithMessage("The specified equipment does not exist.");

            RuleFor(x => x.TechnicalId)
                .MustAsync(TechnicalExists)
                .WithMessage("The specified technical does not exist.");

            // Conditional validations: DepartmentId and RecipientId only required for Department destiny (DestinyTypeId == 1)
            When(x => x.DestinyTypeId == 1, () =>
            {
                RuleFor(x => x.DepartmentId)
                    .NotEmpty().WithMessage("Department ID is required when destiny is Department.")
                    .NotEqual(Guid.Empty).WithMessage("Department ID cannot be empty when destiny is Department.")
                    .MustAsync(DepartmentExists)
                    .WithMessage("The specified department does not exist.");

                RuleFor(x => x.RecipientId)
                    .NotEmpty().WithMessage("Recipient ID is required when destiny is Department.")
                    .NotEqual(Guid.Empty).WithMessage("Recipient ID cannot be empty when destiny is Department.");
            });

            // For Disposal (2) and Warehouse (3), DepartmentId and RecipientId should be empty
            When(x => x.DestinyTypeId == 2 || x.DestinyTypeId == 3, () =>
            {
                RuleFor(x => x.DepartmentId)
                    .Equal(Guid.Empty).WithMessage("Department ID must be empty for Disposal or Warehouse destiny.");

                RuleFor(x => x.RecipientId)
                    .Equal(Guid.Empty).WithMessage("Recipient ID must be empty for Disposal or Warehouse destiny.");
            });
        }

        /// <summary>
        /// Checks if the equipment exists in the database.
        /// </summary>
        private async System.Threading.Tasks.Task<bool> EquipmentExists(System.Guid id, System.Threading.CancellationToken ct)
        {
            return await _equipmentRepo.GetByIdAsync(id, ct) != null;
        }

        /// <summary>
        /// Checks if the technical exists in the database.
        /// </summary>
        private async System.Threading.Tasks.Task<bool> TechnicalExists(System.Guid id, System.Threading.CancellationToken ct)
        {
            return await _technicalRepo.GetByIdAsync(id, ct) != null;
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