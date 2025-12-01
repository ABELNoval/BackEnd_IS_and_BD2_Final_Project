using FluentValidation;
using Application.DTOs.Technical;

namespace Application.Validators.Technical
{
    public class CreateTechnicalDtoValidator : AbstractValidator<CreateTechnicalDto>
    {
        public CreateTechnicalDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del técnico es requerido")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es requerido")
                .EmailAddress().WithMessage("Debe ser una dirección de email válida")
                .MaximumLength(150).WithMessage("El email no puede exceder 150 caracteres");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es requerida")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres");

            RuleFor(x => x.Experience)
                .GreaterThanOrEqualTo(0).WithMessage("La experiencia no puede ser negativa")
                .LessThanOrEqualTo(50).WithMessage("La experiencia no puede exceder 50 años");

            RuleFor(x => x.Specialty)
                .NotEmpty().WithMessage("La especialidad es requerida")
                .MaximumLength(100).WithMessage("La especialidad no puede exceder 100 caracteres")
                .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage("La especialidad solo puede contener letras y espacios");
        }
    }
}