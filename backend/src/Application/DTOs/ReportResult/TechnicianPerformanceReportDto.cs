using System;

namespace Application.DTOs.ReportResult
{
    /// <summary>
    /// Read-model for REPORT 6: raw technician performance data.
    /// Provides supervisor ratings and TOTAL intervention counts for HR salary decisions.
    /// </summary>
    public class TechnicianPerformanceReportDto
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
        /// Average performance score from supervisor assessments (0-100).
        /// </summary>
        public decimal AveragePerformanceScore { get; set; }

        /// <summary>
        /// TOTAL number of maintenance interventions performed (lifetime).
        /// </summary>
        public int TotalInterventions { get; set; }
    }
}
