using FluentValidation;
using Application.DTOs.EquipmentDecommission;

namespace Application.Validators.EquipmentDecommission
{
    public class UpdateEquipmentDecommissionDtoValidator : AbstractValidator<UpdateEquipmentDecommissionDto>
    {
        public UpdateEquipmentDecommissionDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El ID de la baja técnica es requerido")
                .NotEqual(Guid.Empty).WithMessage("El ID de la baja técnica no puede estar vacío");

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