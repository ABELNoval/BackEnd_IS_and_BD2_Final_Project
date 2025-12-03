using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IMaintenanceRepository : IRepository<Maintenance>
    {
        // ✅ MÉTODOS DE FILTRADO DINÁMICO ESPECÍFICOS PARA MAINTENANCE
        
        /// <summary>
        /// Filtra mantenimientos usando Dynamic LINQ
        /// </summary>
        /// <param name="query">Consulta en formato Dynamic LINQ (ej: "EquipmentId == Guid('...') && Cost > 1000")</param>
        Task<IEnumerable<Maintenance>> FilterAsync(string query, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Filtra mantenimientos con paginación
        /// </summary>
        Task<(IEnumerable<Maintenance> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query,
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Obtiene mantenimientos por ID de equipo
        /// </summary>
        Task<IEnumerable<Maintenance>> GetByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene mantenimientos por ID de técnico
        /// </summary>
        Task<IEnumerable<Maintenance>> GetByTechnicalIdAsync(Guid technicalId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene mantenimientos por tipo de mantenimiento
        /// </summary>
        Task<IEnumerable<Maintenance>> GetByMaintenanceTypeIdAsync(int maintenanceTypeId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene mantenimientos por rango de fechas
        /// </summary>
        Task<IEnumerable<Maintenance>> GetByDateRangeAsync(
            DateTime startDate, 
            DateTime endDate, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Obtiene mantenimientos con costo mayor a un valor
        /// </summary>
        Task<IEnumerable<Maintenance>> GetByCostGreaterThanAsync(decimal minCost, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene el último mantenimiento de un equipo
        /// </summary>
        Task<Maintenance?> GetLatestMaintenanceForEquipmentAsync(Guid equipmentId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene el costo total de mantenimientos por equipo
        /// </summary>
        Task<Dictionary<Guid, decimal>> GetTotalMaintenanceCostByEquipmentAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene el conteo de mantenimientos por tipo
        /// </summary>
        Task<Dictionary<int, int>> GetCountByMaintenanceTypeAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene mantenimientos realizados por técnico en un período
        /// </summary>
        Task<IEnumerable<Maintenance>> GetMaintenancesByTechnicalInPeriodAsync(
            Guid technicalId, 
            DateTime startDate, 
            DateTime endDate, 
            CancellationToken cancellationToken = default);
    }
}