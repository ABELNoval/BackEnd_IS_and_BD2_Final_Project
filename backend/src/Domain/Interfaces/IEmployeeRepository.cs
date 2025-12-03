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
        /// Filtra empleados con paginación
        /// </summary>
        Task<(IEnumerable<Employee> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query,
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Busca empleados por nombre
        /// </summary>
        Task<IEnumerable<Employee>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene empleados por rol (RoleId)
        /// </summary>
        Task<IEnumerable<Employee>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene empleados que no tienen departamento asignado como responsables
        /// </summary>
        Task<IEnumerable<Employee>> GetEmployeesWithoutDepartmentResponsibilityAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene empleados con email de un dominio específico
        /// </summary>
        Task<IEnumerable<Employee>> GetByEmailDomainAsync(string domain, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Verifica si existe un empleado con el mismo email
        /// </summary>
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene empleados creados en un rango de fechas
        /// </summary>
        Task<IEnumerable<Employee>> GetByCreationDateRangeAsync(
            DateTime startDate, 
            DateTime endDate, 
            CancellationToken cancellationToken = default);
    }
}