using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IDirectorRepository : IRepository<Director>
    {
        /// <summary>
        /// Filtra directores usando Dynamic LINQ
        /// </summary>
        /// <param name="query">Consulta en formato Dynamic LINQ (ej: "Email.Value.Contains('@company.com')")</param>
        Task<IEnumerable<Director>> FilterAsync(string query, CancellationToken cancellationToken = default);
    }
}