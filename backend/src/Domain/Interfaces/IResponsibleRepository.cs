using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IResponsibleRepository : IRepository<Responsible>
    {
        /// <summary>
        /// Filtra responsables usando Dynamic LINQ
        /// </summary>
        Task<IEnumerable<Responsible>> FilterAsync(string query, CancellationToken cancellationToken = default);
    }
}