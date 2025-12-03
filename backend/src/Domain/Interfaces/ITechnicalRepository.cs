using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITechnicalRepository : IRepository<Technical>
    {
        /// <summary>
        /// Filtra técnicos usando Dynamic LINQ
        /// </summary>
        /// <param name="query">Consulta en formato Dynamic LINQ (ej: "Name.Contains('John') && Experience > 5")</param>
        Task<IEnumerable<Technical>> FilterAsync(string query, CancellationToken cancellationToken = default);
    }
}