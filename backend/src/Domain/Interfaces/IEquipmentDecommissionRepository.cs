using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEquipmentDecommissionRepository : IRepository<EquipmentDecommission>
    {
        /// <summary>
        /// Filtra descomisiones usando Dynamic LINQ
        /// </summary>
        /// <param name="query">Consulta en formato Dynamic LINQ (ej: "EquipmentId == Guid('...') && DestinyTypeId == 1")</param>
        Task<IEnumerable<EquipmentDecommission>> FilterAsync(string query, CancellationToken cancellationToken = default);
    }
}