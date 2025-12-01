using FluentValidation;
using Application.DTOs.EquipmentDecommission;

namespace Application.Validators.EquipmentDecommission
{
    public class CreateEquipmentDecommissionDtoValidator : AbstractValidator<CreateEquipmentDecommissionDto>
    {
        public CreateEquipmentDecommissionDtoValidator()
        {
            RuleFor(x => x.EquipmentId)
                .NotEmpty().WithMessage("El ID del equipo es requerido")
                .NotEqual(Guid.Empty).WithMessage("El ID del equipo no puede estar vacío");

            RuleFor(x => x.TechnicalId)
                .NotEmpty().WithMessage("El ID del técnico es requerido")
                .NotEqual(Guid.Empty).WithMessage("El ID del técnico no puede estar vacío");

            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage("El ID del departamento es requerido")
                .NotEqual(Guid.Empty).WithMessage("El ID del departamento no puede estar vacío");

            RuleFor(x => x.DestinyTypeId)
                .NotEmpty().WithMessage("El tipo de destino es requerido")
                .InclusiveBetween(1, 3).WithMessage("El tipo de destino debe ser válido (1: Departamento, 2: Desecho, 3: Almacén)");

            RuleFor(x => x.RecipientId)
                .NotEmpty().WithMessage("El ID del receptor es requerido")
                .NotEqual(Guid.Empty).WithMessage("El ID del receptor no puede estar vacío");

            RuleFor(x => x.DecommissionDate)
                .NotEmpty().WithMessage("La fecha de baja es requerida")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La fecha de baja no puede ser futura");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("La razón de la baja es requerida")
                .MaximumLength(500).WithMessage("La razón no puede exceder 500 caracteres")
                .Must(reason => reason.ToLower().Contains("fallo técnico irreparable") || 
                               reason.ToLower().Contains("obsolescencia") ||
                               reason.ToLower().Contains("otras"))
                .WithMessage("La razón debe incluir causas válidas como 'fallo técnico irreparable', 'obsolescencia' u 'otras'");
        }
    }
}