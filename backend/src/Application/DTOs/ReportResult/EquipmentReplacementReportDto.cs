using System;

namespace Application.DTOs.ReportResult
{
    /// <summary>
    /// Read-model for REPORT 5: equipment with more than three maintenances
    /// in the last year that must be replaced by policy.
    /// </summary>
    public class EquipmentReplacementReportDto
    {
        /// <summary>
        /// Unique identifier of the equipment.
        /// </summary>
        public Guid EquipmentId { get; set; }

        /// <summary>
        /// Human-readable equipment name.
        /// </summary>
        public string EquipmentName { get; set; } = string.Empty;

        /// <summary>
        /// Name of the equipment type.
        /// </summary>
        public string EquipmentTypeName { get; set; } = string.Empty;

        /// <summary>
        /// Total number of maintenances in the last 12 months.
        /// </summary>
        public int MaintenanceCountLastYear { get; set; }

        /// <summary>
        /// Date of the last maintenance performed.
        /// </summary>
        public DateTime? LastMaintenanceDate { get; set; }
    }
}
