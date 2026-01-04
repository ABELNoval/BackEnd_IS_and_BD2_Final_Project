
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Application.DTOs.Assessment;
using Domain.Interfaces;

namespace Application.Validators.Assessment
{
    /// <summary>
    /// Validator for <see cref="CreateAssessmentDto"/>.
    /// Ensures all required fields are present and valid, and checks existence of related technical and director asynchronously.
    /// </summary>
    public class CreateAssessmentDtoValidator : AbstractValidator<CreateAssessmentDto>
    {
        private readonly ITechnicalRepository _technicalRepository;
        private readonly IDirectorRepository _directorRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAssessmentDtoValidator"/> class.
        /// </summary>
        /// <param name="technicalRepository">Repository for technical existence checks.</param>
        /// <param name="directorRepository">Repository for director existence checks.</param>
        public CreateAssessmentDtoValidator(ITechnicalRepository technicalRepository, IDirectorRepository directorRepository)
        {
            _technicalRepository = technicalRepository;
            _directorRepository = directorRepository;

            RuleFor(x => x.TechnicalId)
                .NotEmpty().WithMessage("Technical ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Technical ID cannot be empty.")
                .MustAsync(TechnicalExistsAsync).WithMessage("Technical with the specified ID does not exist.");

            RuleFor(x => x.DirectorId)
                .NotEmpty().WithMessage("Director ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Director ID cannot be empty.")
                .MustAsync(DirectorExistsAsync).WithMessage("Director with the specified ID does not exist.");

            RuleFor(x => x.Score)
                .NotEmpty().WithMessage("Score is required.")
                .InclusiveBetween(0, 100).WithMessage("Score must be between 0 and 100.");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Comment is required.")
                .MaximumLength(500).WithMessage("Comment cannot exceed 500 characters.");
        }

        /// <summary>
        /// Checks asynchronously if the technical exists.
        /// </summary>
        private async Task<bool> TechnicalExistsAsync(Guid technicalId, CancellationToken cancellationToken)
        {
            if (technicalId == Guid.Empty) return false;
            var technical = await _technicalRepository.GetByIdAsync(technicalId, cancellationToken);
            return technical != null;
        }

        /// <summary>
        /// Checks asynchronously if the director exists.
        /// </summary>
        private async Task<bool> DirectorExistsAsync(Guid directorId, CancellationToken cancellationToken)
        {
            if (directorId == Guid.Empty) return false;
            var director = await _directorRepository.GetByIdAsync(directorId, cancellationToken);
            return director != null;
        }
    }
}