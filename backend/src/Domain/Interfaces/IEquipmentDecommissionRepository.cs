using Domain.Entities;
using Domain.Enumerations;
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
        
        /// <summary>
        /// Filtra descomisiones con paginación
        /// </summary>
        Task<(IEnumerable<EquipmentDecommission> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query,
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Obtiene descomisiones por ID de equipo
        /// </summary>
        Task<IEnumerable<EquipmentDecommission>> GetByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene descomisiones por ID de técnico
        /// </summary>
        Task<IEnumerable<EquipmentDecommission>> GetByTechnicalIdAsync(Guid technicalId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene descomisiones por tipo de destino
        /// </summary>
        Task<IEnumerable<EquipmentDecommission>> GetByDestinyTypeIdAsync(int destinyTypeId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene descomisiones por rango de fechas
        /// </summary>
        Task<IEnumerable<EquipmentDecommission>> GetByDateRangeAsync(
            DateTime startDate, 
            DateTime endDate, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Obtiene descomisiones por ID de departamento
        /// </summary>
        Task<IEnumerable<EquipmentDecommission>> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene el conteo de descomisiones por tipo de destino
        /// </summary>
        Task<Dictionary<int, int>> GetCountByDestinyTypeAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene descomisiones con motivo que contiene texto específico
        /// </summary>
        Task<IEnumerable<EquipmentDecommission>> SearchByReasonAsync(string reason, CancellationToken cancellationToken = default);
    }
}