using Application.DTOs.ReportResult;

namespace Application.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for read-only analytical report queries.
    /// </summary>
    public interface IReportQueriesRepository
    {
        /// <summary>
        /// REPORT 1 - Decommissioned equipment last year.
        /// Lists all equipment decommissioned in the last year, including decommission cause,
        /// final destination and recipient name (if assigned).
        /// </summary>
        /// <returns>
        /// IEnumerable of <see cref="EquipmentDecommissionLastYearDto"/> ordered by decommission date (newest first).
        /// </returns>
        Task<IEnumerable<EquipmentDecommissionLastYearDto>> GetEquipmentDecommissionLastYearAsync();

        /// <summary>
        /// REPORT 2 - Equipment maintenance history.
        /// Gets the maintenance history for a specific equipment, classified by type and date,
        /// including technicians who performed the interventions.
        /// </summary>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns>
        /// IEnumerable of <see cref="EquipmentMaintenanceHistoryDto"/> ordered by maintenance date (newest first).
        /// </returns>
        Task<IEnumerable<EquipmentMaintenanceHistoryDto>> GetEquipmentMaintenanceHistoryAsync(Guid equipmentId);

        /// <summary>
        /// REPORT 3 - Equipment transfers between different sections.
        /// Lists equipment transferred between sections with dates, origin, destination,
        /// sender and recipient names.
        /// </summary>
        /// <returns>
        /// IEnumerable of <see cref="EquipmentTransferBetweenSectionsDto"/> ordered by transfer date (newest first).
        /// </returns>
        Task<IEnumerable<EquipmentTransferBetweenSectionsDto>> GetEquipmentTransferHistoryBetweenSectionsAsync();

        /// <summary>
        /// REPORT 4 - Technician performance vs equipment longevity (irreparable failures).
        /// Identifies the top 5 technicians with the worst correlation between:
        /// - High maintenance cost on equipment that ends in "fallo técnico irreparable"
        /// - Low equipment longevity (from acquisition to decommission)
        /// - Poor supervisor assessment scores.
        /// Grouped by technician and equipment type for analytical reporting.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TechnicianPerformanceCorrelationDto"/> representing
        /// the 5 technicians with the worst cost/longevity/performance correlation
        /// for equipment decommissioned due to "fallo técnico irreparable".
        /// </returns>
        Task<IEnumerable<TechnicianPerformanceCorrelationDto>> GetTechnicianMaintenanceCorrelationAsync();

        /// <summary>
        /// REPORT 5 - Frequent maintenance equipment.
        /// Retrieves equipment that has received more than three maintenances
        /// in the last year and therefore must be replaced according to policy.
        /// </summary>
        /// <returns>
        /// A list of <see cref="EquipmentReplacementReportDto"/> representing
        /// equipment with more than three maintenances in the last 12 months,
        /// including maintenance count and last maintenance date.
        /// </returns>
        Task<IEnumerable<EquipmentReplacementReportDto>> GetFrequentMaintenanceEquipmentAsync();

        /// <summary>
        /// REPORT 6 - Technician performance comparison for salary review.
        /// Compares technicians based on supervisor ratings and total interventions performed.
        /// </summary>
        /// <returns>
        /// List of technicians ordered by performance score, then total interventions.
        /// </returns>
        Task<List<TechnicianPerformanceReportDto>> GetTechnicianPerformanceReportAsync();

        /// <summary>
        /// REPORT 7 - Equipment sent to a specific department.
        /// Returns all equipment that has arrived at the target department,
        /// including sender, receiver, destination department and sending company.
        /// Combines decommissions (DestinyType = 1) and internal transfers.
        /// </summary>
        /// <param name="targetDepartmentId">Target department identifier.</param>
        /// <returns>
        /// List of <see cref="EquipmentSentToDepartmentDto"/> ordered by sent date (newest first).
        /// </returns>
        Task<IEnumerable<EquipmentSentToDepartmentDto>> GetEquipmentSentToDepartmentAsync(Guid targetDepartmentId);
    }
}
