using Application.DTOs.Assessment;

namespace Application.Interfaces.Services
{
    public interface IAssessmentService
    {
        // Crear una evaluación
        Task<AssessmentDTO> CreateAsync(CreateAssessmentDto dto, CancellationToken cancellationToken = default);

        // Actualizar una evaluación
        Task<AssessmentDTO?> UpdateAsync(UpdateAssessmentDto dto, CancellationToken cancellationToken = default);

        // Eliminar una evaluación
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        // Obtener una evaluación por Id
        Task<AssessmentDTO?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        // Obtener todas las evaluaciones
        Task<IEnumerable<AssessmentDTO>> GetAllAsync(CancellationToken cancellationToken = default);

        // Obtener evaluaciones por técnico
        Task<IEnumerable<AssessmentDTO>> GetByTechnicalIdAsync(Guid technicalId, CancellationToken cancellationToken = default);

        // Obtener evaluaciones por director
        Task<IEnumerable<AssessmentDTO>> GetByDirectorIdAsync(Guid directorId, CancellationToken cancellationToken = default);

        // Obtener evaluaciones por rango de fechas
        Task<IEnumerable<AssessmentDTO>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}
