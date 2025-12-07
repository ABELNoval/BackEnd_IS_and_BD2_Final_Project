using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITransferRepository : IRepository<Transfer>
    {  
        /// <summary>
        /// Filtra transferencias usando Dynamic LINQ
        /// </summary>
        /// <param name="query">Consulta en formato Dynamic LINQ (ej: "EquipmentId == Guid('...') && TransferDate > DateTime(2023, 1, 1)")</param>
        Task<IEnumerable<Transfer>> FilterAsync(string query, CancellationToken cancellationToken = default);
    }
}