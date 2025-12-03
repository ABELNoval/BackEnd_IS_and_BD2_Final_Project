using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IMaintenanceRepository : IRepository<Maintenance>
    {
        /// <summary>
        /// Filtra mantenimientos usando Dynamic LINQ
        /// </summary>
        /// <param name="query">Consulta en formato Dynamic LINQ (ej: "EquipmentId == Guid('...') && Cost > 1000")</param>
        Task<IEnumerable<Maintenance>> FilterAsync(string query, CancellationToken cancellationToken = default);
    }
}