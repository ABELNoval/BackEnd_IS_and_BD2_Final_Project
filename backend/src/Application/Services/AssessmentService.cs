using Application.DTOs.Assessment;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class AssessmentService : IAssessmentService
    {
        private readonly IAssessmentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AssessmentService(
            IAssessmentRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Crear evaluación
        public async Task<AssessmentDTO> CreateAsync(CreateAssessmentDto dto, CancellationToken cancellationToken = default)
        {
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
            var list = await _repository.GetByTechnicalIdAsync(technicalId);
            return _mapper.Map<IEnumerable<AssessmentDTO>>(list);
        }

        // Obtener por director
        public async Task<IEnumerable<AssessmentDTO>> GetByDirectorIdAsync(Guid directorId, CancellationToken cancellationToken = default)
        {
            var list = await _repository.GetByDirectorIdAsync(directorId);
            return _mapper.Map<IEnumerable<AssessmentDTO>>(list);
        }

        // Obtener por rango de fechas
        public async Task<IEnumerable<AssessmentDTO>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var list = await _repository.GetByDateRangeAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<AssessmentDTO>>(list);
        }
    }
}
