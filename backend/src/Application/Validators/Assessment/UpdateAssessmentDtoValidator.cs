
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Application.DTOs.Assessment;
using Domain.Interfaces;

namespace Application.Validators.Assessment
{
    /// <summary>
    /// Validator for <see cref="UpdateAssessmentDto"/>.
    /// Ensures all required fields are present and valid, and checks existence of the assessment asynchronously.
    /// </summary>
    public class UpdateAssessmentDtoValidator : AbstractValidator<UpdateAssessmentDto>
    {
        private readonly IAssessmentRepository _assessmentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAssessmentDtoValidator"/> class.
        /// </summary>
        /// <param name="assessmentRepository">Repository for assessment existence checks.</param>
        public UpdateAssessmentDtoValidator(IAssessmentRepository assessmentRepository)
        {
            _assessmentRepository = assessmentRepository;

            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty).WithMessage("Assessment ID is required.")
                .MustAsync(AssessmentExistsAsync).WithMessage("Assessment with the specified ID does not exist.");

            RuleFor(x => x.Score)
                .InclusiveBetween(0, 100).WithMessage("Score must be between 0 and 100.");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Comment is required.")
                .MaximumLength(500).WithMessage("Comment cannot exceed 500 characters.");
        }

        /// <summary>
        /// Checks asynchronously if the assessment exists.
        /// </summary>
        private async Task<bool> AssessmentExistsAsync(Guid assessmentId, CancellationToken cancellationToken)
        {
            if (assessmentId == Guid.Empty) return false;
            var assessment = await _assessmentRepository.GetByIdAsync(assessmentId, cancellationToken);
            return assessment != null;
        }
    }
}