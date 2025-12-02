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

        // Obtener los resultados de filtrar
        Task<IEnumerable<AssessmentDTO>> FilterAsync(string query, CancellationToken cancellationToken = default);
    }
}