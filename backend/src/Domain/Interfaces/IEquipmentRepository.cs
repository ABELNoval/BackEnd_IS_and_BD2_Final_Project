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
        
        /// <summary>
        /// Filtra equipos con paginación
        /// </summary>
        Task<(IEnumerable<Equipment> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query,
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Obtiene equipos por ID de departamento
        /// </summary>
        Task<IEnumerable<Equipment>> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene equipos por ID de tipo de equipo
        /// </summary>
        Task<IEnumerable<Equipment>> GetByEquipmentTypeIdAsync(Guid equipmentTypeId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene equipos por estado
        /// </summary>
        Task<IEnumerable<Equipment>> GetByStateIdAsync(int stateId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene equipos por tipo de ubicación
        /// </summary>
        Task<IEnumerable<Equipment>> GetByLocationTypeIdAsync(int locationTypeId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene equipos que necesitan mantenimiento (basado en última fecha de mantenimiento)
        /// </summary>
        Task<IEnumerable<Equipment>> GetEquipmentNeedingMaintenanceAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene equipos operativos
        /// </summary>
        Task<IEnumerable<Equipment>> GetOperativeEquipmentAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene equipos descomisionados
        /// </summary>
        Task<IEnumerable<Equipment>> GetDecommissionedEquipmentAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene equipos dados de baja
        /// </summary>
        Task<IEnumerable<Equipment>> GetDisposedEquipmentAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene equipos por rango de fecha de adquisición
        /// </summary>
        Task<IEnumerable<Equipment>> GetByAcquisitionDateRangeAsync(
            DateTime startDate, 
            DateTime endDate, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Busca equipos por nombre
        /// </summary>
        Task<IEnumerable<Equipment>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene equipos con historial de transferencias
        /// </summary>
        Task<IEnumerable<Equipment>> GetEquipmentWithTransferHistoryAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene equipos con historial de mantenimientos
        /// </summary>
        Task<IEnumerable<Equipment>> GetEquipmentWithMaintenanceHistoryAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Verifica si un equipo tiene mantenimientos pendientes
        /// </summary>
        Task<bool> HasPendingMaintenanceAsync(Guid equipmentId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene el conteo de equipos por estado
        /// </summary>
        Task<Dictionary<int, int>> GetCountByStateAsync(CancellationToken cancellationToken = default);
    }
}