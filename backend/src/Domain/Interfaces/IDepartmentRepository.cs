using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        // Buscar por nombre exacto (case-insensitive)
        Task<Department?> GetByNameAsync(string name);

        // Obtener departamentos por sección
        Task<IEnumerable<Department>> GetBySectionIdAsync(Guid sectionId);

        // Obtener departamentos por responsable
        Task<IEnumerable<Department>> GetByResponsibleIdAsync(Guid responsibleId);

        // Paginado básico
        Task<IEnumerable<Department>> GetAllPagedAsync(int pageNumber, int pageSize);
    }
}