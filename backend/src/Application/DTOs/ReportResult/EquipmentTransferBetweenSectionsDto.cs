using System;

namespace Application.DTOs.ReportResult
{
    /// <summary>
    /// Read-model for REPORT 3: equipment transfers between different sections.
    /// Contains transfer details including origin, destination, sender and recipient.
    /// </summary>
    public class EquipmentTransferBetweenSectionsDto
    {
        /// <summary>
        /// Equipment name.
        /// </summary>
        public string EquipmentName { get; set; } = string.Empty;

        /// <summary>
        /// Transfer date.
        /// </summary>
        public DateTime TransferDate { get; set; }

        /// <summary>
        /// Source section name (origin).
        /// </summary>
        public string SourceSection { get; set; } = string.Empty;

        /// <summary>
        /// Target section name (destination).
        /// </summary>
        public string TargetSection { get; set; } = string.Empty;

        /// <summary>
        /// Name of the person who sends the equipment.
        /// </summary>
        public string SenderName { get; set; } = string.Empty;

        /// <summary>
        /// Name of the person who receives the equipment.
        /// </summary>
        public string RecipientName { get; set; } = string.Empty;
    }
}
