using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEquipmentTypeRepository : IRepository<EquipmentType>
    { 
        /// <summary>
        /// Filtra tipos de equipo usando Dynamic LINQ
        /// </summary>
        /// <param name="query">Consulta en formato Dynamic LINQ (ej: "Name.Contains('Computer')")</param>
        Task<IEnumerable<EquipmentType>> FilterAsync(string query, CancellationToken cancellationToken = default);
    }
}