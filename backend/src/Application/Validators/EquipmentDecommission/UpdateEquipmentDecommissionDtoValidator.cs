using FluentValidation;
using Application.DTOs.EquipmentDecommission;

namespace Application.Validators.EquipmentDecommission
{
    /// <summary>
    /// Validator for UpdateEquipmentDecommissionDto. Performs existence and business rule validations for equipment decommission update.
    /// </summary>
    public class UpdateEquipmentDecommissionDtoValidator : AbstractValidator<UpdateEquipmentDecommissionDto>
    {
        private readonly Domain.Interfaces.IEquipmentDecommissionRepository _decommissionRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateEquipmentDecommissionDtoValidator"/> class.
        /// </summary>
        /// <param name="decommissionRepo">The equipment decommission repository for existence checks.</param>
        public UpdateEquipmentDecommissionDtoValidator(Domain.Interfaces.IEquipmentDecommissionRepository decommissionRepo)
        {
            _decommissionRepo = decommissionRepo;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Decommission ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Decommission ID cannot be empty.")
                .MustAsync(DecommissionExists).WithMessage("The specified decommission record does not exist.");

            RuleFor(x => x.DecommissionDate)
                .NotEmpty().WithMessage("Decommission date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Decommission date cannot be in the future.");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Decommission reason is required.")
                .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters.");
        }

        /// <summary>
        /// Checks if the equipment decommission record exists in the database.
        /// </summary>
        private async System.Threading.Tasks.Task<bool> DecommissionExists(System.Guid id, System.Threading.CancellationToken ct)
        {
            return await _decommissionRepo.GetByIdAsync(id, ct) != null;
        }
    }
}