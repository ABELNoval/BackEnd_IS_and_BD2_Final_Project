
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
    /// Validator for <see cref="UpdateMaintenanceDto"/>.
    /// Ensures all required fields are present and valid, and checks existence of related entities asynchronously.
    /// </summary>
    public class UpdateMaintenanceDtoValidator : AbstractValidator<UpdateMaintenanceDto>
    {
        private readonly IMaintenanceRepository _maintenanceRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateMaintenanceDtoValidator"/> class.
        /// </summary>
        /// <param name="maintenanceRepository">Repository for maintenance existence checks.</param>
        public UpdateMaintenanceDtoValidator(IMaintenanceRepository maintenanceRepository)
        {
            _maintenanceRepository = maintenanceRepository;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Maintenance ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Maintenance ID cannot be empty.")
                .MustAsync(MaintenanceExistsAsync).WithMessage("Maintenance with the specified ID does not exist.");

            RuleFor(x => x.MaintenanceDate)
                .NotEmpty().WithMessage("Maintenance date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Maintenance date cannot be in the future.");

            RuleFor(x => x.MaintenanceTypeId)
                .NotEmpty().WithMessage("Maintenance type is required.")
                .MustAsync(MaintenanceTypeIsValidAsync).WithMessage("Maintenance type must be valid (1: Preventive, 2: Corrective, 3: Predictive, 4: Emergency).");

            RuleFor(x => x.Cost)
                .GreaterThanOrEqualTo(0).WithMessage("Cost cannot be negative.")
                .LessThanOrEqualTo(1000000).WithMessage("Cost cannot exceed 1,000,000.");
        }

        /// <summary>
        /// Checks asynchronously if the maintenance exists.
        /// </summary>
        private async Task<bool> MaintenanceExistsAsync(Guid maintenanceId, CancellationToken cancellationToken)
        {
            if (maintenanceId == Guid.Empty) return false;
            var maintenance = await _maintenanceRepository.GetByIdAsync(maintenanceId, cancellationToken);
            return maintenance != null;
        }

        /// <summary>
        /// Checks asynchronously if the maintenance type is valid.
        /// </summary>
        private Task<bool> MaintenanceTypeIsValidAsync(int maintenanceTypeId, CancellationToken cancellationToken)
        {
            return Task.FromResult(Domain.Enumerations.MaintenanceType.FromId(maintenanceTypeId) != null);
        }
    }
}