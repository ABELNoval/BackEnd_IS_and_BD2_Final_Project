namespace Application.DTOs.ReportResult
{
    /// <summary>
    /// Read-model for REPORT 4: technician performance vs equipment longevity.
    /// Represents the aggregated correlation metrics per technician and equipment type.
    /// </summary>
    public class TechnicianPerformanceCorrelationDto
    {
        /// <summary>
        /// Unique identifier of the technician.
        /// </summary>
        public Guid TechnicianId { get; set; }

        /// <summary>
        /// Technician full name.
        /// </summary>
        public string TechnicianName { get; set; } = string.Empty;

        /// <summary>
        /// Average performance score given by supervisors (0-100).
        /// </summary>
        public decimal AveragePerformanceScore { get; set; }

        /// <summary>
        /// Total maintenance cost on equipment that ended as irreparable failure.
        /// </summary>
        public decimal TotalMaintenanceCost { get; set; }

        /// <summary>
        /// Average number of days the equipment remained operative before decommission.
        /// </summary>
        public double AverageEquipmentLongevityDays { get; set; }

        /// <summary>
        /// Name of the equipment type where this correlation is being measured.
        /// </summary>
        public string EquipmentTypeName { get; set; } = string.Empty;
    }
}