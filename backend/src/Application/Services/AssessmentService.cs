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
        private readonly IAssessmentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAssessmentDto> _createValidator;
        private readonly IValidator<UpdateAssessmentDto> _updateValidator;

        public AssessmentService(
            IAssessmentRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateAssessmentDto> createValidator,
            IValidator<UpdateAssessmentDto> updateValidator)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        // Crear evaluación
        public async Task<AssessmentDTO> CreateAsync(CreateAssessmentDto dto, CancellationToken cancellationToken = default)
        {
            // Validar DTO usando FluentValidation
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = Assessment.Create(
                dto.TechnicalId,
                dto.DirectorId,
                dto.Score,
                dto.Comment
            );

            await _repository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<AssessmentDTO>(entity);
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

            _repository.Update(existing);
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

        // Obtener por técnico
        public async Task<IEnumerable<AssessmentDTO>> GetByTechnicalIdAsync(Guid technicalId, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (technicalId == Guid.Empty)
            {
                throw new ArgumentException("Technical ID cannot be empty", nameof(technicalId));
            }

            var list = await _repository.GetByTechnicalIdAsync(technicalId);
            return _mapper.Map<IEnumerable<AssessmentDTO>>(list);
        }

        // Obtener por director
        public async Task<IEnumerable<AssessmentDTO>> GetByDirectorIdAsync(Guid directorId, CancellationToken cancellationToken = default)
        {
            // Validación básica del ID
            if (directorId == Guid.Empty)
            {
                throw new ArgumentException("Director ID cannot be empty", nameof(directorId));
            }

            var list = await _repository.GetByDirectorIdAsync(directorId);
            return _mapper.Map<IEnumerable<AssessmentDTO>>(list);
        }

        // Obtener por rango de fechas
        public async Task<IEnumerable<AssessmentDTO>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            // Validación de fechas
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be later than end date", nameof(startDate));
            }

            var list = await _repository.GetByDateRangeAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<AssessmentDTO>>(list);
        }
    }
}