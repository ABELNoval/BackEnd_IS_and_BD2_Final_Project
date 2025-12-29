using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        /// <summary>
        /// Filtra empleados usando Dynamic LINQ
        /// </summary>
        /// <param name="query">Consulta en formato Dynamic LINQ (ej: "Name.Contains('John')")</param>
        Task<IEnumerable<Employee>> FilterAsync(string query, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets employees by a list of department IDs
        /// </summary>
        Task<IEnumerable<Employee>> GetByDepartmentIdsAsync(IEnumerable<Guid> departmentIds, CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene un empleado por su email
        /// </summary>
        Task<Employee?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}