using System;

namespace Application.DTOs.ReportResult
{
    /// <summary>
    /// DTO for report of equipment sent to a specific department.
    /// </summary>
    public class EquipmentSentToDepartmentDto
    {
        /// <summary>
        /// Name of the equipment.
        /// </summary>
        public string EquipmentName { get; set; }
        /// <summary>
        /// Type of the equipment.
        /// </summary>
        public string EquipmentType { get; set; }
        /// <summary>
        /// Date when the equipment was sent.
        /// </summary>
        public DateTime SendDate { get; set; }
        /// <summary>
        /// Reason for sending the equipment.
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// Name of the sender (technician from source department).
        /// </summary>
        public string SenderName { get; set; }
        /// <summary>
        /// Email of the sender.
        /// </summary>
        public string SenderEmail { get; set; }
        /// <summary>
        /// Name of the sender's department.
        /// </summary>
        public string SenderDepartment { get; set; }
        /// <summary>
        /// Name of the sender's company/division (source section).
        /// </summary>
        public string SenderCompany { get; set; }
        /// <summary>
        /// Name of the receiver (responsible of target department).
        /// </summary>
        public string ReceiverName { get; set; }
        /// <summary>
        /// Email of the receiver.
        /// </summary>
        public string ReceiverEmail { get; set; }
        /// <summary>
        /// Name of the receiver's department.
        /// </summary>
        public string ReceiverDepartment { get; set; }
        /// <summary>
        /// Current state of the equipment.
        /// </summary>
        public string EquipmentState { get; set; }
        /// <summary>
        /// Indicates if the equipment is defective.
        /// </summary>
        public bool IsDefective { get; set; }
    }
}
