using Application.DTOs.ReportResult;

namespace Application.Interfaces.Services
{
    public interface IReportQueryService
    {
        // Reporte 1: Equipos dados de baja en el último año
        Task<IEnumerable<EquipmentDecommissionLastYearDto>> GetEquipmentDecommissionLastYearAsync();
        
        // Reporte 2: Historial de mantenimientos de un equipo específico
        Task<IEnumerable<EquipmentMaintenanceHistoryDto>> GetEquipmentMaintenanceHistoryAsync(Guid equipmentId);
        
        // Reporte 3: Equipos trasladados entre secciones
        //Task<IEnumerable<EquipmentTransfersDto>> GetEquipmentTransfersAsync();
        
        // Reporte 4: Correlación rendimiento técnicos vs longevidad equipos
        //Task<IEnumerable<TechnicianPerformanceCorrelationDto>> GetTechnicianPerformanceCorrelationAsync();
        
        // Reporte 5: Equipos con más de 3 mantenimientos en el último año
        Task<IEnumerable<FrequentMaintenanceEquipmentDto>> GetFrequentMaintenanceEquipmentAsync();
        
        // Reporte 6: Rendimiento técnicos para bonificaciones
        Task<IEnumerable<TechnicianPerformanceBonusDto>> GetTechnicianPerformanceBonusAsync();
        
        // Reporte 7: Equipos enviados a un departamento específico
        //Task<IEnumerable<EquipmentToDepartmentDto>> GetEquipmentToDepartmentAsync(string departmentId);
    }
}