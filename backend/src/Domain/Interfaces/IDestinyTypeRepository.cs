using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Enumerations;

namespace Domain.Interfaces
{
    public interface IDestinyTypeRepository : IRepository<DestinyType>
    {
        /// <summary>
        /// Filtra tipos de destino usando Dynamic LINQ
        /// </summary>
        /// <param name="query">Consulta en formato Dynamic LINQ (ej: "Name.Contains('Warehouse')")</param>
        Task<IEnumerable<DestinyType>> FilterAsync(string query, CancellationToken cancellationToken = default);
    }
}