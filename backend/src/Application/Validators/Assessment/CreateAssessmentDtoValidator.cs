using System;
using FluentValidation;
using Application.DTOs.Assessment;

namespace Application.Validators.Assessment
{
    public class CreateAssessmentDtoValidator : AbstractValidator<CreateAssessmentDto>
    {
        public CreateAssessmentDtoValidator()
        {
            RuleFor(x => x.TechnicalId)
                .NotEmpty().WithMessage("Technical ID is required")
                .NotEqual(Guid.Empty).WithMessage("Technical ID cannot be empty");

            RuleFor(x => x.DirectorId)
                .NotEmpty().WithMessage("Director ID is required")
                .NotEqual(Guid.Empty).WithMessage("Director ID cannot be empty");

            RuleFor(x => x.Score)
                .NotEmpty().WithMessage("Score is required")
                .InclusiveBetween(0, 100).WithMessage("Score must be between 0 and 100");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Comment is required")
                .MaximumLength(500).WithMessage("Comment cannot exceed 500 characters");
        }
    }
}