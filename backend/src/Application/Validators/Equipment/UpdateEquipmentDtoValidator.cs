using FluentValidation;
using Application.DTOs.Equipment;

namespace Application.Validators.Equipment
{
    /// <summary>
    /// Validator for UpdateEquipmentDto. Performs existence and business rule validations for equipment update.
    /// </summary>
    public class UpdateEquipmentDtoValidator : AbstractValidator<UpdateEquipmentDto>
    {
        private readonly Domain.Interfaces.IEquipmentRepository _equipmentRepo;
        private readonly Domain.Interfaces.IEquipmentTypeRepository _equipmentTypeRepo;
        private readonly Domain.Interfaces.IDepartmentRepository _departmentRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateEquipmentDtoValidator"/> class.
        /// </summary>
        /// <param name="equipmentRepo">The equipment repository for existence checks.</param>
        /// <param name="equipmentTypeRepo">The equipment type repository for existence checks.</param>
        /// <param name="departmentRepo">The department repository for existence checks.</param>
        public UpdateEquipmentDtoValidator(
            Domain.Interfaces.IEquipmentRepository equipmentRepo,
            Domain.Interfaces.IEquipmentTypeRepository equipmentTypeRepo,
            Domain.Interfaces.IDepartmentRepository departmentRepo)
        {
            _equipmentRepo = equipmentRepo;
            _equipmentTypeRepo = equipmentTypeRepo;
            _departmentRepo = departmentRepo;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Equipment ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Equipment ID cannot be empty.")
                .MustAsync(EquipmentExists).WithMessage("The specified equipment does not exist.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Equipment name is required.")
                .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.")
                .Matches("^[a-zA-Z0-9 \\-\\.]+$").WithMessage("Name can only contain letters, numbers, spaces, hyphens and dots.");

            RuleFor(x => x.AcquisitionDate)
                .NotEmpty().WithMessage("Acquisition date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Acquisition date cannot be in the future.");

            RuleFor(x => x.EquipmentTypeId)
                .NotEmpty().WithMessage("Equipment type is required.")
                .NotEqual(Guid.Empty).WithMessage("Equipment type ID cannot be empty.")
                .MustAsync(EquipmentTypeExists).WithMessage("The specified equipment type does not exist.");

            When(x => x.DepartmentId.HasValue, () =>
            {
                RuleFor(x => x.DepartmentId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Department ID cannot be empty if provided.")
                    .MustAsync(async (id, ct) => id == null || await _departmentRepo.GetByIdAsync(id.Value, ct) != null)
                    .WithMessage("The specified department does not exist.");
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
        /// Checks if the equipment type exists in the database.
        /// </summary>
        private async System.Threading.Tasks.Task<bool> EquipmentTypeExists(System.Guid id, System.Threading.CancellationToken ct)
        {
            return await _equipmentTypeRepo.GetByIdAsync(id, ct) != null;
        }
    }
}