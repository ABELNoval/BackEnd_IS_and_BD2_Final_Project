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
}