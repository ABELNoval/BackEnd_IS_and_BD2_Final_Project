using FluentValidation;
using Application.DTOs.EquipmentType;

namespace Application.Validators.EquipmentType
{
    /// <summary>
    /// Validator for UpdateEquipmentTypeDto. Performs existence and uniqueness validations for equipment type update.
    /// </summary>
    public class UpdateEquipmentTypeDtoValidator : AbstractValidator<UpdateEquipmentTypeDto>
    {
        private readonly Domain.Interfaces.IEquipmentTypeRepository _equipmentTypeRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateEquipmentTypeDtoValidator"/> class.
        /// </summary>
        /// <param name="equipmentTypeRepo">The equipment type repository for existence and uniqueness checks.</param>
        public UpdateEquipmentTypeDtoValidator(Domain.Interfaces.IEquipmentTypeRepository equipmentTypeRepo)
        {
            _equipmentTypeRepo = equipmentTypeRepo;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Equipment type ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Equipment type ID cannot be empty.")
                .MustAsync(EquipmentTypeExists).WithMessage("The specified equipment type does not exist.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Equipment type name is required.")
                .MaximumLength(100).WithMessage("Equipment type name cannot exceed 100 characters.")
                .MustAsync(NameIsUnique).WithMessage("An equipment type with this name already exists.");
        }

        /// <summary>
        /// Checks if the equipment type exists in the database.
        /// </summary>
        private async System.Threading.Tasks.Task<bool> EquipmentTypeExists(System.Guid id, System.Threading.CancellationToken ct)
        {
            return await _equipmentTypeRepo.GetByIdAsync(id, ct) != null;
        }

        /// <summary>
        /// Checks if the equipment type name is unique in the database (excluding the current ID).
        /// </summary>
        private async System.Threading.Tasks.Task<bool> NameIsUnique(UpdateEquipmentTypeDto dto, string name, System.Threading.CancellationToken ct)
        {
            var existing = await _equipmentTypeRepo.FilterAsync($"Name == \"{name}\" && Id != Guid(\"{dto.Id}\")", ct);
            return existing == null || !existing.GetEnumerator().MoveNext();
        }
    }
}