using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        /// <summary>
        /// Filtra departamentos usando Dynamic LINQ
        /// </summary>
        /// <param name="query">Consulta en formato Dynamic LINQ (ej: "Name.Contains('IT') && SectionId != null")</param>
        Task<IEnumerable<Department>> FilterAsync(string query, CancellationToken cancellationToken = default);
    }
}