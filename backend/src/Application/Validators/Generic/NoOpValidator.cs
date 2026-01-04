using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace Application.Validators.Generic
{
    public class NoOpValidator<T> : AbstractValidator<T>
    {
        public NoOpValidator()
        {
            // No validation rules; acts as a no-op validator
        }

        public override ValidationResult Validate(ValidationContext<T> context)
        {
            return new ValidationResult();
        }

        public override Task<ValidationResult> ValidateAsync(ValidationContext<T> context, CancellationToken cancellation = default)
        {
            return Task.FromResult(new ValidationResult());
        }
    }
}
