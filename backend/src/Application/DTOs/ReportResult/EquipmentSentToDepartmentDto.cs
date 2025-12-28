using System;

namespace Application.DTOs.ReportResult
{
    /// <summary>
    /// Read-model for REPORT 7: equipment sent to specific department.
    /// Contains sender, receiver, destination department and sending company details.
    /// </summary>
    public class EquipmentSentToDepartmentDto
    {
        /// <summary>
        /// Equipment name.
        /// </summary>
        public string EquipmentName { get; set; } = string.Empty;

        /// <summary>
        /// Name of the person who sends the equipment.
        /// </summary>
        public string SenderName { get; set; } = string.Empty;

        /// <summary>
        /// Name of the person who receives the equipment.
        /// </summary>
        public string ReceiverName { get; set; } = string.Empty;

        /// <summary>
        /// Destination department name.
        /// </summary>
        public string DestinationDepartment { get; set; } = string.Empty;

        /// <summary>
        /// Sending company name (from section).
        /// </summary>
        public string SendingCompany { get; set; } = string.Empty;

        /// <summary>
        /// Date when equipment was sent.
        /// </summary>
        public DateTime SentDate { get; set; }
    }
}
