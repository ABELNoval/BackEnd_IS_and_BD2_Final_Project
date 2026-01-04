using System;

namespace Application.DTOs.ReportResult
{
    /// <summary>
    /// Read-model for REPORT 1: decommissioned equipment last year.
    /// Contains equipment details, decommission cause, final destination and recipient.
    /// </summary>
    public class EquipmentDecommissionLastYearDto
    {
        /// <summary>
        /// Equipment name.
        /// </summary>
        public string EquipmentName { get; set; } = string.Empty;

        /// <summary>
        /// Cause of the equipment decommission.
        /// </summary>
        public string DecommissionCause { get; set; } = string.Empty;

        /// <summary>
        /// Final destination (department name or destiny type).
        /// </summary>
        public string FinalDestination { get; set; } = string.Empty;

        /// <summary>
        /// Name of the recipient person (or "No recipient assigned").
        /// </summary>
        public string ReceiverName { get; set; } = string.Empty;

        /// <summary>
        /// Decommission date.
        /// </summary>
        public DateTime DecommissionDate { get; set; }
    }
}