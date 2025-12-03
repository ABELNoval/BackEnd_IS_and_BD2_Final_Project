using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEquipmentRepository : IRepository<Equipment>
    {
        /// <summary>
        /// Filtra equipos usando Dynamic LINQ
        /// </summary>
        /// <param name="query">Consulta en formato Dynamic LINQ (ej: "Name.Contains('Laptop') && StateId == 1")</param>
        Task<IEnumerable<Equipment>> FilterAsync(string query, CancellationToken cancellationToken = default);
    }
}