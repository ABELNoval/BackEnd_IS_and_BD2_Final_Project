using Application.DTOs.Assessment;
using Application.Interfaces.Services;
using Application.Validators.Assessment;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using Application.Exceptions;

namespace Application.Services
{
    public class AssessmentService : IAssessmentService
    {
        private readonly ITechnicalRepository _technicalRepository;
        private readonly IAssessmentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAssessmentDto> _createValidator;
        private readonly IValidator<UpdateAssessmentDto> _updateValidator;

        public AssessmentService(
            ITechnicalRepository technicalRepository,
            IAssessmentRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateAssessmentDto> createValidator,
            IValidator<UpdateAssessmentDto> updateValidator)
        {
            _technicalRepository = technicalRepository;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        /// <summary>
        /// Creates a new assessment by calling Technical.AddAssessment().
        /// </summary>
        /// <param name="dto">The CreateAssessmentDto to validate and create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created AssessmentDTO.</returns>
        public async Task<AssessmentDTO> CreateAsync(CreateAssessmentDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors);

            var technical = await _technicalRepository.GetByIdAsync(dto.TechnicalId, cancellationToken);
            if (technical == null)
                throw new EntityNotFoundException(nameof(Technical), dto.TechnicalId);

            technical.AddAssessment(
                dto.DirectorId,
                dto.Score,
                dto.Comment);

            var assessment = technical.Assessments.Last();

            await _repository.CreateAsync(assessment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<AssessmentDTO>(assessment);
        }

        /// <summary>
        /// Updates an existing assessment after validating the DTO.
        /// </summary>
        /// <param name="dto">The UpdateAssessmentDto to validate and update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated AssessmentDTO, or throws EntityNotFoundException if not found.</returns>
        public async Task<AssessmentDTO?> UpdateAsync(UpdateAssessmentDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Application.Exceptions.ValidationException(validationResult.Errors);

            var existing = await _repository.GetByIdAsync(dto.Id, cancellationToken);
            if (existing == null)
                throw new EntityNotFoundException(nameof(Assessment), dto.Id);

            existing.UpdateScore(dto.Score);
            existing.UpdateComment(dto.Comment);

            await _repository.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<AssessmentDTO>(existing);
        }

        /// <summary>
        /// Deletes an assessment by ID.
        /// </summary>
        /// <param name="id">The assessment ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if deleted, otherwise throws EntityNotFoundException.</returns>
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                throw new EntityNotFoundException(nameof(Assessment), id);

            await _repository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Gets an assessment by ID.
        /// </summary>
        /// <param name="id">The assessment ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The AssessmentDTO if found, otherwise throws EntityNotFoundException.</returns>
        public async Task<AssessmentDTO?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                throw new EntityNotFoundException(nameof(Assessment), id);
            return _mapper.Map<AssessmentDTO>(entity);
        }

        public async Task<IEnumerable<AssessmentDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var list = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<AssessmentDTO>>(list);
        }

        public async Task<IEnumerable<AssessmentDTO>> FilterAsync(string query, CancellationToken cancellationToken = default)
        {
            var entities = await _repository.FilterAsync(query, cancellationToken);
            return _mapper.Map<IEnumerable<AssessmentDTO>>(entities);
        }
    }
}