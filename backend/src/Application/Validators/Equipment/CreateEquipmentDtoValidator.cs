using System;
using FluentValidation;
using Application.DTOs.Equipment;

namespace Application.Validators.Equipment
{
    /// <summary>
    /// Validator for CreateEquipmentDto. Performs basic and existence validations for equipment creation.
    /// </summary>
    public class CreateEquipmentDtoValidator : AbstractValidator<CreateEquipmentDto>
    {
        private readonly Domain.Interfaces.IEquipmentTypeRepository _equipmentTypeRepo;
        private readonly Domain.Interfaces.IDepartmentRepository _departmentRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateEquipmentDtoValidator"/> class.
        /// </summary>
        /// <param name="equipmentTypeRepo">The equipment type repository for existence checks.</param>
        /// <param name="departmentRepo">The department repository for existence checks.</param>
        public CreateEquipmentDtoValidator(
            Domain.Interfaces.IEquipmentTypeRepository equipmentTypeRepo,
            Domain.Interfaces.IDepartmentRepository departmentRepo)
        {
            _equipmentTypeRepo = equipmentTypeRepo;
            _departmentRepo = departmentRepo;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Equipment name is required.")
                .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");

            RuleFor(x => x.AcquisitionDate)
                .LessThanOrEqualTo(_ => DateTime.UtcNow)
                .WithMessage("Acquisition date cannot be in the future.");

            RuleFor(x => x.EquipmentTypeId)
                .NotEqual(Guid.Empty)
                .WithMessage("Equipment type ID is required.")
                .MustAsync(EquipmentTypeExists)
                .WithMessage("The specified equipment type does not exist.");

            When(x => x.DepartmentId.HasValue, () =>
            {
                RuleFor(x => x.DepartmentId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Department ID cannot be empty if provided.")
                    .MustAsync(async (id, ct) => id == null || await _departmentRepo.GetByIdAsync(id.Value, ct) != null)
                    .WithMessage("The specified department does not exist.");
            });

            RuleFor(x => x.StateId)
                .GreaterThan(0)
                .WithMessage("State ID must be greater than 0.");

            RuleFor(x => x.LocationTypeId)
                .GreaterThan(0)
                .WithMessage("Location type ID must be greater than 0.");
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