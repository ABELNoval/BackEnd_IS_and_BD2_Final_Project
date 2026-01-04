namespace Application.DTOs.ReportResult
{
    public class EquipmentMaintenanceHistoryDto
    {
        // Reporte 2: Historial de mantenimientos de un equipo específico
        public Guid MaintenanceId { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string MaintenanceType { get; set; } // Tipo de mantenimiento
        public decimal Cost { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentState { get; set; }
        public string Department { get; set; }
        public string TechnicalName { get; set; }
        public string TechnicalSpeciality { get; set; }
        public int TechnicalExperience { get; set; }
        public string TechnicalEmail { get; set; }
        public int DaysFromAcquisition { get; set; } // Días desde adquisición
    }
}