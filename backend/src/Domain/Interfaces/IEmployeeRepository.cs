using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        // Buscar employee por nombre (case-insensitive)
        Task<Employee?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

        // Buscar employee por email
        Task<Employee?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        // Obtener todos los employees con paginado
        Task<IEnumerable<Employee>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        // Verificar si un employee existe por email
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}