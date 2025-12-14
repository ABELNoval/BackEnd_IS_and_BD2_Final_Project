using Application.DTOs.Assessment;
using Application.Interfaces.Services;
using Application.Validators.Assessment;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;

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
        /// Creates a new assessment by calling Technical.AddAssessment()
        /// </summary>
        public async Task<AssessmentDTO> CreateAsync(CreateAssessmentDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Get the aggregate root Technical
            var technical = await _technicalRepository.GetByIdAsync(dto.TechnicalId, cancellationToken);
            if (technical == null)
                throw new ValidationException($"Technical with ID {dto.TechnicalId} not found");

            // Technical creates and manages the Assessment entity
            technical.AddAssessment(
                dto.DirectorId,
                dto.Score,
                dto.Comment);

            var assessment = technical.Assessments.Last();

            await _repository.CreateAsync(assessment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Return the last assessment added
            return _mapper.Map<AssessmentDTO>(assessment);
        }

        // Actualizar evaluación
        public async Task<AssessmentDTO?> UpdateAsync(UpdateAssessmentDto dto, CancellationToken cancellationToken = default)
        {
            // Validar DTO usando FluentValidation
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existing = await _repository.GetByIdAsync(dto.Id, cancellationToken);

            if (existing == null)
                return null;

            existing.UpdateScore(dto.Score);
            existing.UpdateComment(dto.Comment);

            await _repository.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<AssessmentDTO>(existing);
        }

        // Eliminar evaluación
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                return false;

            await _repository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        // Obtener por Id
        public async Task<AssessmentDTO?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            return entity is null ? null : _mapper.Map<AssessmentDTO>(entity);
        }

        // Obtener todas
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