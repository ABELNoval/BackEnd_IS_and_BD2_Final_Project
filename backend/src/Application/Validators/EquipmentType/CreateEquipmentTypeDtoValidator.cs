using System;
using FluentValidation;
using Application.DTOs.EquipmentType;

namespace Application.Validators.EquipmentType
{
    /// <summary>
    /// Validator for CreateEquipmentTypeDto. Performs basic and uniqueness validations for equipment type creation.
    /// </summary>
    public class CreateEquipmentTypeDtoValidator : AbstractValidator<CreateEquipmentTypeDto>
    {
        private readonly Domain.Interfaces.IEquipmentTypeRepository _equipmentTypeRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateEquipmentTypeDtoValidator"/> class.
        /// </summary>
        /// <param name="equipmentTypeRepo">The equipment type repository for uniqueness checks.</param>
        public CreateEquipmentTypeDtoValidator(Domain.Interfaces.IEquipmentTypeRepository equipmentTypeRepo)
        {
            _equipmentTypeRepo = equipmentTypeRepo;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Equipment type name is required.")
                .MaximumLength(100).WithMessage("Equipment type name cannot exceed 100 characters.")
                .MustAsync(NameIsUnique).WithMessage("An equipment type with this name already exists.");
        }

        /// <summary>
        /// Checks if the equipment type name is unique in the database.
        /// </summary>
        private async System.Threading.Tasks.Task<bool> NameIsUnique(string name, System.Threading.CancellationToken ct)
        {
            var existing = await _equipmentTypeRepo.FilterAsync($"Name == \"{name}\"", ct);
            return existing == null || !existing.GetEnumerator().MoveNext();
        }
    }
}