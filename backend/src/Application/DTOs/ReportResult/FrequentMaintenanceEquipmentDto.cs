namespace Application.DTOs.ReportResult
{
    public class FrequentMaintenanceEquipmentDto
    {
        // Reporte 5: Equipos con más de 3 mantenimientos en el último año
        public string EquipmentName { get; set; }
        public string EquipmentType { get; set; }
        public string State { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public string Department { get; set; }
        public string DepartmentResponsible { get; set; }
        public int MaintenanceCountLastYear { get; set; } // Mantenimientos últimos 12 meses
        public DateTime LastMaintenanceDate { get; set; }
        public decimal TotalMaintenanceCost { get; set; }
        public string Recommendation { get; set; } // "REEMPLAZO RECOMENDADO"
        public int DaysSinceAcquisition { get; set; }
        public decimal MonthlyMaintenanceFrequency { get; set; } // Mantenimientos por mes
    }
}