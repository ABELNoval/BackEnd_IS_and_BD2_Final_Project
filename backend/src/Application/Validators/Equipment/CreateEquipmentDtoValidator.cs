using FluentValidation;
using Application.DTOs.Equipment;

namespace Application.Validators.Equipment
{
    public class CreateEquipmentDtoValidator : AbstractValidator<CreateEquipmentDto>
    {
        public CreateEquipmentDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del equipo es requerido")
                .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres")
                .Matches("^[a-zA-Z0-9 \\-\\.]+$").WithMessage("El nombre solo puede contener letras, números, espacios, guiones y puntos");

            RuleFor(x => x.AcquisitionDate)
                .NotEmpty().WithMessage("La fecha de adquisición es requerida")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La fecha de adquisición no puede ser futura");

            RuleFor(x => x.EquipmentTypeId)
                .NotEmpty().WithMessage("El tipo de equipo es requerido")
                .NotEqual(Guid.Empty).WithMessage("El ID del tipo de equipo no puede estar vacío");

            RuleFor(x => x.DepartmentId)
                .Must(id => !id.HasValue || id.Value != Guid.Empty)
                .WithMessage("El ID del departamento no puede estar vacío si se proporciona");
        }
    }
}