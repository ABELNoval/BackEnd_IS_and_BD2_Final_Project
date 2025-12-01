using FluentValidation;
using Application.DTOs.Maintenance;

namespace Application.Validators.Maintenance
{
    public class UpdateMaintenanceDtoValidator : AbstractValidator<UpdateMaintenanceDto>
    {
        public UpdateMaintenanceDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El ID del mantenimiento es requerido")
                .NotEqual(Guid.Empty).WithMessage("El ID del mantenimiento no puede estar vacío");

            RuleFor(x => x.MaintenanceDate)
                .NotEmpty().WithMessage("La fecha de mantenimiento es requerida")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La fecha de mantenimiento no puede ser futura");

            RuleFor(x => x.MaintenanceTypeId)
                .NotEmpty().WithMessage("El tipo de mantenimiento es requerido")
                .InclusiveBetween(1, 4).WithMessage("El tipo de mantenimiento debe ser válido (1: Preventivo, 2: Correctivo, 3: Predictivo, 4: Emergencia)");

            RuleFor(x => x.Cost)
                .GreaterThanOrEqualTo(0).WithMessage("El costo no puede ser negativo")
                .LessThanOrEqualTo(1000000).WithMessage("El costo no puede exceder 1,000,000");
        }
    }
}