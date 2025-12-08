namespace Application.DTOs.ReportResult
{
    public class TechnicianPerformanceCorrelationDto
    {
        // Reporte 4: Correlación rendimiento técnicos vs longevidad equipos
        public string TechnicalName { get; set; }
        public string Speciality { get; set; }
        public int Experience { get; set; }
        public decimal AverageRating { get; set; } // Valoración promedio
        public int TotalRatings { get; set; }
        public int TotalMaintenances { get; set; }
        public decimal TotalMaintenanceCost { get; set; }
        public decimal AverageMaintenanceCost { get; set; }
        public int TotalDecommissionedEquipment { get; set; } // Equipos dados de baja
        public decimal CorrelationScore { get; set; } // Puntaje de correlación (alto costo + baja longevidad = malo)
        public string Recommendation { get; set; } // Recomendación
    }
}