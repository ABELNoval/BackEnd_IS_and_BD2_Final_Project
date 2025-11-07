using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IDirectorRepository : IRepository<Director>
    {
        // Buscar director por nombre (case-insensitive)
        Task<Director?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

        // Buscar director por email
        Task<Director?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        // Obtener todos los directores con paginado
        Task<IEnumerable<Director>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        // Verificar si un director existe por email
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}