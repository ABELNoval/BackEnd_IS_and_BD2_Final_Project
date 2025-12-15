using System;

namespace Application.DTOs.ReportResult
{
    /// <summary>
    /// Equipment transferred between different sections, including transfer date, origin, destination, sender, and receiver information.
    /// </summary>
    public class EquipmentTransferBetweenSectionsDto
    {
        /// <summary>
        /// Equipment unique identifier.
        /// </summary>
        public Guid EquipmentId { get; set; }

        /// <summary>
        /// Equipment name.
        /// </summary>
        public string EquipmentName { get; set; } = string.Empty;

        /// <summary>
        /// Equipment type name.
        /// </summary>
        public string EquipmentType { get; set; } = string.Empty;

        /// <summary>
        /// Date of the transfer.
        /// </summary>
        public DateTime TransferDate { get; set; }

        /// <summary>
        /// Name of the source section.
        /// </summary>
        public string SourceSection { get; set; } = string.Empty;

        /// <summary>
        /// Name of the source department.
        /// </summary>
        public string SourceDepartment { get; set; } = string.Empty;

        /// <summary>
        /// Name of the target section.
        /// </summary>
        public string TargetSection { get; set; } = string.Empty;

        /// <summary>
        /// Name of the target department.
        /// </summary>
        public string TargetDepartment { get; set; } = string.Empty;

        /// <summary>
        /// Name of the person who sent the equipment.
        /// </summary>
        public string SenderName { get; set; } = string.Empty;

        /// <summary>
        /// Email of the person who sent the equipment.
        /// </summary>
        public string SenderEmail { get; set; } = string.Empty;
    }
}
