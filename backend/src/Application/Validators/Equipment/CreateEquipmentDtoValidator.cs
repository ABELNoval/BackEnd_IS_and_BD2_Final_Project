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
        private readonly Domain.Interfaces.ITechnicalRepository _technicalRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateEquipmentDtoValidator"/> class.
        /// </summary>
        /// <param name="equipmentTypeRepo">The equipment type repository for existence checks.</param>
        /// <param name="departmentRepo">The department repository for existence checks.</param>
        /// <param name="technicalRepo">The technical repository for existence checks.</param>
        public CreateEquipmentDtoValidator(
            Domain.Interfaces.IEquipmentTypeRepository equipmentTypeRepo,
            Domain.Interfaces.IDepartmentRepository departmentRepo,
            Domain.Interfaces.ITechnicalRepository technicalRepo)
        {
            _equipmentTypeRepo = equipmentTypeRepo;
            _departmentRepo = departmentRepo;
            _technicalRepo = technicalRepo;

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

            RuleFor(x => x.TechnicalId)
                .NotEqual(Guid.Empty)
                .WithMessage("Technical ID is required for initial maintenance.")
                .MustAsync(TechnicalExists)
                .WithMessage("The specified technical does not exist.");
        }

        /// <summary>
        /// Checks if the equipment type exists in the database.
        /// </summary>
        private async System.Threading.Tasks.Task<bool> EquipmentTypeExists(System.Guid id, System.Threading.CancellationToken ct)
        {
            return await _equipmentTypeRepo.GetByIdAsync(id, ct) != null;
        }

        /// <summary>
        /// Checks if the technical exists in the database.
        /// </summary>
        private async System.Threading.Tasks.Task<bool> TechnicalExists(System.Guid id, System.Threading.CancellationToken ct)
        {
            return await _technicalRepo.GetByIdAsync(id, ct) != null;
        }
    }
}