namespace Application.DTOs.ReportResult
{
    /// <summary>
    /// DTO for technician performance correlation report (Report 4).
    /// </summary>
    public class TechnicianPerformanceCorrelationDto
    {
        /// <summary>
        /// Position in the ranking (1 = worst).
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// Technician's name.
        /// </summary>
        public string TechnicalName { get; set; } = string.Empty;

        /// <summary>
        /// Technician's specialty.
        /// </summary>
        public string Specialty { get; set; } = string.Empty;

        /// <summary>
        /// Average rating (0-100).
        /// </summary>
        public decimal AverageRating { get; set; }

        /// <summary>
        /// Number of irreparable equipment maintained.
        /// </summary>
        public int IrreparableEquipmentCount { get; set; }

        /// <summary>
        /// Total maintenance cost.
        /// </summary>
        public decimal TotalMaintenanceCost { get; set; }

        /// <summary>
        /// Average cost per equipment.
        /// </summary>
        public decimal AverageCostPerEquipment { get; set; }

        /// <summary>
        /// Average longevity in days.
        /// </summary>
        public double AverageLongevityDays { get; set; }

        /// <summary>
        /// Correlation score (lower = worse).
        /// </summary>
        public decimal CorrelationScore { get; set; }

        /// <summary>
        /// Most maintained equipment type.
        /// </summary>
        public string EquipmentSpecialization { get; set; } = string.Empty;
    }
}namespace Application.DTOs.ReportResult
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