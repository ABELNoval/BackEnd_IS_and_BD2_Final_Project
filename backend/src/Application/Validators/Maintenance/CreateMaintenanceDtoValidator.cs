
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Application.DTOs.Maintenance;
using Domain.Interfaces;
using Domain.Enumerations;

namespace Application.Validators.Maintenance
{
    /// <summary>
    /// Validator for <see cref="CreateMaintenanceDto"/>.
    /// Ensures all required fields are present and valid, and checks existence of related entities asynchronously.
    /// </summary>
    public class CreateMaintenanceDtoValidator : AbstractValidator<CreateMaintenanceDto>
    {
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly ITechnicalRepository _technicalRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateMaintenanceDtoValidator"/> class.
        /// </summary>
        /// <param name="equipmentRepository">Repository for equipment existence checks.</param>
        /// <param name="technicalRepository">Repository for technical existence checks.</param>
        public CreateMaintenanceDtoValidator(
            IEquipmentRepository equipmentRepository,
            ITechnicalRepository technicalRepository)
        {
            _equipmentRepository = equipmentRepository;
            _technicalRepository = technicalRepository;

            RuleFor(x => x.EquipmentId)
                .NotEmpty().WithMessage("Equipment ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Equipment ID cannot be empty.")
                .MustAsync(EquipmentExistsAsync).WithMessage("Equipment with the specified ID does not exist.")
                .CustomAsync(ValidateEquipmentStateAsync);

            RuleFor(x => x.TechnicalId)
                .NotEmpty().WithMessage("Technical ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Technical ID cannot be empty.")
                .MustAsync(TechnicalExistsAsync).WithMessage("Technical with the specified ID does not exist.");

            // RuleFor(x => x.MaintenanceDate)
            //     .NotEmpty().WithMessage("Maintenance date is required.")
            //     .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Maintenance date cannot be in the future.");

            RuleFor(x => x.MaintenanceTypeId)
                .NotEmpty().WithMessage("Maintenance type is required.")
                .MustAsync(MaintenanceTypeIsValidAsync).WithMessage("Maintenance type must be valid (1: Preventive, 2: Corrective, 3: Predictive, 4: Emergency).");

            RuleFor(x => x.Cost)
                .GreaterThanOrEqualTo(0).WithMessage("Cost cannot be negative.")
                .LessThanOrEqualTo(1000000).WithMessage("Cost cannot exceed 1,000,000.");
        }

        /// <summary>
        /// Checks asynchronously if the equipment exists.
        /// </summary>
        private async Task<bool> EquipmentExistsAsync(Guid equipmentId, CancellationToken cancellationToken)
        {
            if (equipmentId == Guid.Empty) return false;
            var equipment = await _equipmentRepository.GetByIdAsync(equipmentId, cancellationToken);
            return equipment != null;
        }

        /// <summary>
        /// Checks asynchronously if the technical exists.
        /// </summary>
        private async Task<bool> TechnicalExistsAsync(Guid technicalId, CancellationToken cancellationToken)
        {
            if (technicalId == Guid.Empty) return false;
            var technical = await _technicalRepository.GetByIdAsync(technicalId, cancellationToken);
            return technical != null;
        }

        /// <summary>
        /// Checks asynchronously if the maintenance type is valid.
        /// </summary>
        private Task<bool> MaintenanceTypeIsValidAsync(int maintenanceTypeId, CancellationToken cancellationToken)
        {
            return Task.FromResult(Domain.Enumerations.MaintenanceType.FromId(maintenanceTypeId) != null);
        }

        /// <summary>
        /// Validates the state of the equipment to ensure it can receive maintenance.
        /// </summary>
        private async Task ValidateEquipmentStateAsync(Guid equipmentId, ValidationContext<CreateMaintenanceDto> context, CancellationToken cancellationToken)
        {
            if (equipmentId == Guid.Empty) return;

            var equipment = await _equipmentRepository.GetByIdAsync(equipmentId, cancellationToken);
            if (equipment == null) return; // Handled by EquipmentExistsAsync

            if (equipment.StateId == EquipmentState.UnderMaintenance.Id)
            {
                context.AddFailure("EquipmentId", "Equipment is already under maintenance.");
            }

            if (equipment.StateId == EquipmentState.Decommissioned.Id || equipment.LocationTypeId == LocationType.Warehouse.Id)
            {
                context.AddFailure("EquipmentId", "Equipment is decommissioned (in warehouse) and cannot receive maintenance.");
            }

            if (equipment.StateId == EquipmentState.Disposed.Id || equipment.LocationTypeId == LocationType.Disposal.Id)
            {
                context.AddFailure("EquipmentId", "Equipment is disposed and cannot receive maintenance.");
            }
        }
    }
}