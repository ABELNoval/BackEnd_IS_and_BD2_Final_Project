using Application.DTOs.ReportResult;
using Application.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Reports
{

    public class ReportQueriesRepository : IReportQueriesRepository
    {
        private readonly AppDbContext _context;

        public ReportQueriesRepository(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// REPORT 1 - Decommissioned equipment last year.
        /// Lists all equipment decommissioned in the last year, including decommission cause,
        /// final destination and recipient name (if assigned).
        /// Uses Stored Procedure for efficient database-side processing.
        /// </summary>
        /// <returns>
        /// IEnumerable of <see cref="EquipmentDecommissionLastYearDto"/> ordered by decommission date (newest first).
        /// </returns>
        public async Task<IEnumerable<EquipmentDecommissionLastYearDto>> GetEquipmentDecommissionLastYearAsync()
        {
            var result = await _context.Database
                .SqlQueryRaw<EquipmentDecommissionLastYearDto>("CALL GetEquipmentDecommissionLastYear()")
                .ToListAsync();

            return result;
        }
        
        /// <summary>
        /// REPORT 2 - Equipment maintenance history.
        /// Gets the maintenance history for a specific equipment, classified by type and date,
        /// including technicians who performed the interventions.
        /// Uses Stored Procedure for efficient database-side processing.
        /// </summary>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns>
        /// IEnumerable of <see cref="EquipmentMaintenanceHistoryDto"/> ordered by maintenance date (newest first).
        /// </returns>
        public async Task<IEnumerable<EquipmentMaintenanceHistoryDto>> GetEquipmentMaintenanceHistoryAsync(Guid equipmentId)
        {
            var result = await _context.Database
                .SqlQueryRaw<EquipmentMaintenanceHistoryDto>(
                    "CALL GetEquipmentMaintenanceHistory({0})",
                    equipmentId.ToString())
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// REPORT 3 - Equipment transfers between different sections.
        /// Lists equipment transferred between sections with dates, origin, destination,
        /// sender and recipient names.
        /// </summary>
        /// <returns>
        /// IEnumerable of <see cref="EquipmentTransferBetweenSectionsDto"/> ordered by transfer date (newest first).
        /// </returns>
        public async Task<IEnumerable<EquipmentTransferBetweenSectionsDto>> GetEquipmentTransferHistoryBetweenSectionsAsync()
        {
            var query = from transfer in _context.Transfers
                        join equipment in _context.Equipments on transfer.EquipmentId equals equipment.Id
                        join sourceDept in _context.Departments on transfer.SourceDepartmentId equals sourceDept.Id
                        join sourceSection in _context.Sections on sourceDept.SectionId equals sourceSection.Id
                        join targetDept in _context.Departments on transfer.TargetDepartmentId equals targetDept.Id
                        join targetSection in _context.Sections on targetDept.SectionId equals targetSection.Id
                        join sender in _context.Responsibles on transfer.ResponsibleId equals sender.Id
                        join recipient in _context.Responsibles on transfer.RecipientId equals recipient.Id
                        where sourceSection.Id != targetSection.Id
                        select new EquipmentTransferBetweenSectionsDto
                        {
                            EquipmentName = equipment.Name,
                            TransferDate = transfer.TransferDate,
                            SourceSection = sourceSection.Name,
                            TargetSection = targetSection.Name,
                            SenderName = sender.Name,
                            RecipientName = recipient.Name
                        };

            return await query
                .AsNoTracking()
                .OrderByDescending(x => x.TransferDate)
                .ToListAsync();
        }

        /// <summary>
        /// REPORT 4 - Technician performance vs equipment longevity (irreparable failures).
        /// Identifies the top 5 technicians with the worst correlation between:
        /// - High maintenance cost on equipment that ends in "fallo técnico irreparable"
        /// - Low equipment longevity (from acquisition to decommission)
        /// - Poor supervisor assessment scores.
        /// Grouped by technician and equipment type for analytical reporting.
        /// Uses a Stored Procedure for complex SQL that EF Core cannot translate.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TechnicianPerformanceCorrelationDto"/> representing
        /// the 5 technicians with the worst cost/longevity/performance correlation
        /// for equipment decommissioned due to "fallo técnico irreparable".
        /// </returns>
        public async Task<IEnumerable<TechnicianPerformanceCorrelationDto>> GetTechnicianMaintenanceCorrelationAsync()
        {
            var result = await _context.Database
                .SqlQueryRaw<TechnicianPerformanceCorrelationDto>("CALL GetTechnicianMaintenanceCorrelation()")
                .ToListAsync();

            return result;
        }



        /// <summary>
        /// REPORT 5 - Frequent maintenance equipment.
        /// Retrieves equipment that has received more than three maintenances
        /// in the last year and therefore must be replaced according to policy.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// A list of <see cref="EquipmentReplacementReportDto"/> representing
        /// equipment with more than three maintenances in the last 12 months,
        /// including maintenance count and last maintenance date.
        /// </returns>
        public async Task<IEnumerable<EquipmentReplacementReportDto>> GetFrequentMaintenanceEquipmentAsync()
        {
            var oneYearAgo = DateTime.UtcNow.AddYears(-1);

            // PHASE 1: Filter maintenances in the last 12 months
            var recentMaintenances =
                from m in _context.Maintenances
                where m.MaintenanceDate >= oneYearAgo
                select m;

            // PHASE 2: Group by equipment and keep only those with > 3 maintenances
            var equipmentWithCounts =
                from m in recentMaintenances
                group m by m.EquipmentId into g
                where g.Count() > 3
                select new
                {
                    EquipmentId = g.Key,
                    MaintenanceCount = g.Count(),
                    LastMaintenanceDate = g.Max(x => x.MaintenanceDate)
                };

            // PHASE 3: Join with Equipment and EquipmentType to build the report DTO
            var query =
                from ec in equipmentWithCounts
                join e in _context.Equipments
                    on ec.EquipmentId equals e.Id
                join et in _context.EquipmentTypes
                    on e.EquipmentTypeId equals et.Id
                select new EquipmentReplacementReportDto
                {
                    EquipmentId = e.Id,
                    EquipmentName = e.Name,
                    EquipmentTypeName = et.Name,
                    MaintenanceCountLastYear = ec.MaintenanceCount,
                    LastMaintenanceDate = ec.LastMaintenanceDate
                };

            var list = await query
                .AsNoTracking()
                .ToListAsync();

            return list;
        }

        /// <summary>
        /// REPORT 6 - Technician performance comparison for salary review.
        /// Compares technicians based on supervisor ratings and total interventions performed.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// List of technicians ordered by performance score, then total interventions.
        /// </returns>
        public async Task<List<TechnicianPerformanceReportDto>> GetTechnicianPerformanceReportAsync()
        {
            // PHASE 1: Average performance score per technician
            // Using EF.Property to access the mapped column directly (bypasses Value Object)
            var technicianPerformance =
                from a in _context.Assessments
                group a by a.TechnicalId into g
                select new
                {
                    TechnicalId = g.Key,
                    AvgScore = g.Average(x => EF.Property<decimal>(x, "Score"))
                };

            // PHASE 2: Total interventions per technician
            var technicianInterventions =
                from m in _context.Maintenances
                group m by m.TechnicalId into g
                select new
                {
                    TechnicalId = g.Key,
                    TotalInterventions = g.Count()
                };

            // PHASE 3: Combine data and order by performance metrics
            var query =
                from perf in technicianPerformance
                join interv in technicianInterventions on perf.TechnicalId equals interv.TechnicalId
                join tech in _context.Technicals on perf.TechnicalId equals tech.Id
                orderby perf.AvgScore descending, interv.TotalInterventions descending
                select new TechnicianPerformanceReportDto
                {
                    TechnicianId = perf.TechnicalId,
                    TechnicianName = tech.Name,
                    AveragePerformanceScore = perf.AvgScore,
                    TotalInterventions = interv.TotalInterventions
                };

            var list = await query
                .AsNoTracking()
                .ToListAsync();

            return list;
        }

        /// <summary>
        /// Gets the state name for a given state id.
        /// </summary>
        private static string GetStateName(int id) =>
            id switch
            {
                2 => "UnderMaintenance",
                1 => "Operative",
                3 => "Decommissioned",
                4 => "Disposed",
                _ => "Unknown"
            };

        /// <summary>
        /// Gets the maintenance type name for a given maintenance type id.
        /// </summary>
        private static string GetMaintenanceTypeName(int id) =>
            id switch
            {
                1 => "Preventive",
                2 => "Corrective",
                3 => "Predictive",
                4 => "Emergency",
                _ => "Unknown"
            };

        /// <summary>
        /// Gets the destiny name for a given destiny id.
        /// </summary>
        private static string GetDestinyName(int id) =>
            id switch
            {
                1 => "Department",
                2 => "Disposal",
                3 => "Warehouse",
                _ => "Unknown"
            };

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
        public async Task<IEnumerable<EquipmentSentToDepartmentDto>>
            GetEquipmentSentToDepartmentAsync(Guid targetDepartmentId)
        {
            var decommissionsQuery =
                from decomm in _context.EquipmentDecommissions
                where decomm.DestinyTypeId == 1
                      && decomm.DepartmentId == targetDepartmentId

                join equipment in _context.Equipments
                    on decomm.EquipmentId equals equipment.Id

                join destinationDepartment in _context.Departments
                    on decomm.DepartmentId equals destinationDepartment.Id

                join sender in _context.Technicals
                    on decomm.TechnicalId equals sender.Id

                join receiver in _context.Responsibles
                    on decomm.RecipientId equals receiver.Id

                join section in _context.Sections
                    on destinationDepartment.SectionId equals section.Id

                select new EquipmentSentToDepartmentDto
                {
                    EquipmentName = equipment.Name,
                    SenderName = sender.Name,
                    ReceiverName = receiver.Name,
                    DestinationDepartment = destinationDepartment.Name,
                    SendingCompany = section.Name,
                    SentDate = decomm.DecommissionDate
                };

            var transfersQuery =
                from transfer in _context.Transfers
                where transfer.TargetDepartmentId == targetDepartmentId

                join equipment in _context.Equipments
                    on transfer.EquipmentId equals equipment.Id

                join destinationDepartment in _context.Departments
                    on transfer.TargetDepartmentId equals destinationDepartment.Id

                join sender in _context.Responsibles
                    on transfer.ResponsibleId equals sender.Id

                join receiver in _context.Responsibles
                    on transfer.RecipientId equals receiver.Id

                join section in _context.Sections
                    on destinationDepartment.SectionId equals section.Id

                select new EquipmentSentToDepartmentDto
                {
                    EquipmentName = equipment.Name,
                    SenderName = sender.Name,
                    ReceiverName = receiver.Name,
                    DestinationDepartment = destinationDepartment.Name,
                    SendingCompany = section.Name,
                    SentDate = transfer.TransferDate
                };

            var query = decommissionsQuery
                .Union(transfersQuery);

            return await query
                .AsNoTracking()
                .OrderByDescending(x => x.SentDate)
                .ToListAsync();
        }
    }
}
