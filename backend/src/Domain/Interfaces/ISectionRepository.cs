using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISectionRepository : IRepository<Section>
    {
        /// <summary>
        /// Filtra secciones usando Dynamic LINQ
        /// </summary>
        /// <param name="query">Consulta en formato Dynamic LINQ (ej: "Name.Contains('IT')")</param>
        Task<IEnumerable<Section>> FilterAsync(string query, CancellationToken cancellationToken = default);
    }
}