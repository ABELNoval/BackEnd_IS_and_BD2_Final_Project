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
        
        /// <summary>
        /// Filtra transferencias con paginación
        /// </summary>
        Task<(IEnumerable<Transfer> Data, int Total, int Pages)> FilterPaginatedAsync(
            string query,
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Obtiene transferencias por ID de equipo
        /// </summary>
        Task<IEnumerable<Transfer>> GetByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene transferencias por ID de departamento origen
        /// </summary>
        Task<IEnumerable<Transfer>> GetBySourceDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene transferencias por ID de departamento destino
        /// </summary>
        Task<IEnumerable<Transfer>> GetByTargetDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene transferencias por ID del responsable
        /// </summary>
        Task<IEnumerable<Transfer>> GetByResponsibleIdAsync(Guid responsibleId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene transferencias por rango de fechas
        /// </summary>
        Task<IEnumerable<Transfer>> GetByDateRangeAsync(
            DateTime startDate, 
            DateTime endDate, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Obtiene transferencias que involucran un departamento (como origen o destino)
        /// </summary>
        Task<IEnumerable<Transfer>> GetTransfersInvolvingDepartmentAsync(Guid departmentId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene el historial completo de transferencias de un equipo
        /// </summary>
        Task<IEnumerable<Transfer>> GetEquipmentTransferHistoryAsync(Guid equipmentId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene el conteo de transferencias por departamento (como origen)
        /// </summary>
        Task<Dictionary<Guid, int>> GetOutgoingTransferCountByDepartmentAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene el conteo de transferencias por departamento (como destino)
        /// </summary>
        Task<Dictionary<Guid, int>> GetIncomingTransferCountByDepartmentAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene la última transferencia de un equipo
        /// </summary>
        Task<Transfer?> GetLatestTransferForEquipmentAsync(Guid equipmentId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene transferencias recientes (últimos 30 días)
        /// </summary>
        Task<IEnumerable<Transfer>> GetRecentTransfersAsync(int days = 30, CancellationToken cancellationToken = default);
    }
}