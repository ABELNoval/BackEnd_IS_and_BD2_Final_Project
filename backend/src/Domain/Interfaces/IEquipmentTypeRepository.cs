using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEquipmentTypeRepository : IRepository<EquipmentType>
    {
        // ✅ MÉTODOS DE FILTRADO DINÁMICO ESPECÍFICOS PARA EQUIPMENTTYPE
        
        /// <summary>
        /// Filtra tipos de equipo usando Dynamic LINQ
        /// </summary>
        /// <param name="query">Consulta en formato Dynamic LINQ (ej: "Name.Contains('Computer')")</param>
        Task<IEnumerable<EquipmentType>> FilterAsync(string query, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Busca tipos de equipo por nombre
        /// </summary>
        Task<IEnumerable<EquipmentType>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Verifica si existe un tipo de equipo con el mismo nombre
        /// </summary>
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene tipos de equipo con conteo de equipos asociados
        /// </summary>
        Task<IEnumerable<EquipmentType>> GetWithEquipmentCountAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene tipos de equipo sin equipos asignados
        /// </summary>
        Task<IEnumerable<EquipmentType>> GetTypesWithoutEquipmentAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene tipos de equipo ordenados por nombre
        /// </summary>
        Task<IEnumerable<EquipmentType>> GetAllOrderedByNameAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene tipos de equipo por IDs
        /// </summary>
        Task<IEnumerable<EquipmentType>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    }
}