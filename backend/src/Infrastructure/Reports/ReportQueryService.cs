using Application.DTOs.ReportResult;
using Application.Interfaces.Services;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Reports
{

    public class ReportQueryService : IReportQueryService
    {
        private readonly AppDbContext _context;

        public ReportQueryService(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// REPORT 1 - Decommissioned equipment last year.
        /// Lists all equipment decommissioned in the last year, including decommission cause,
        /// final destination and recipient name (if assigned).
        /// </summary>
        /// <returns>
        /// IEnumerable of <see cref="EquipmentDecommissionLastYearDto"/> ordered by decommission date (newest first).
        /// </returns>
        public async Task<IEnumerable<EquipmentDecommissionLastYearDto>> GetEquipmentDecommissionLastYearAsync()
        {
            var oneYearAgo = DateTime.UtcNow.AddYears(-1);

            var query =
                from decomm in _context.EquipmentDecommissions
                where decomm.DecommissionDate >= oneYearAgo

                join equipment in _context.Equipments
                    on decomm.EquipmentId equals equipment.Id

                // LEFT JOIN receiver (may not exist)
                join receiver in _context.Responsibles
                    on decomm.RecipientId equals receiver.Id into receivers
                from receiver in receivers.DefaultIfEmpty()

                // LEFT JOIN department (only for DestinyTypeId == 1)
                join department in _context.Departments
                    on decomm.DepartmentId equals department.Id into departments
                from department in departments.DefaultIfEmpty()

                select new EquipmentDecommissionLastYearDto
                {
                    EquipmentName = equipment.Name,
                    DecommissionCause = decomm.Reason,
                    FinalDestination =
                        decomm.DestinyTypeId == 1 && department != null
                            ? department.Name
                            : GetDestinyName(decomm.DestinyTypeId),
                    ReceiverName = receiver != null ? receiver.Name : "No recipient assigned",
                    DecommissionDate = decomm.DecommissionDate
                };

            return await query
                .AsNoTracking()
                .OrderByDescending(x => x.DecommissionDate)
                .ToListAsync();
        }
        
        /// <summary>
        /// REPORT 2 - Equipment maintenance history.
        /// Gets the maintenance history for a specific equipment, classified by type and date,
        /// including technicians who performed the interventions.
        /// </summary>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns>
        /// IEnumerable of <see cref="EquipmentMaintenanceHistoryDto"/> ordered by maintenance date (newest first).
        /// </returns>
        public async Task<IEnumerable<EquipmentMaintenanceHistoryDto>> GetEquipmentMaintenanceHistoryAsync(Guid equipmentId)
        {
            var query =
                from m in _context.Maintenances
                where m.EquipmentId == equipmentId
                join eq in _context.Equipments
                    on m.EquipmentId equals eq.Id
                join tech in _context.Technicals
                    on m.TechnicalId equals tech.Id
                select new EquipmentMaintenanceHistoryDto
                {
                    MaintenanceId = m.Id,
                    MaintenanceDate = m.MaintenanceDate,
                    MaintenanceType = GetMaintenanceTypeName(m.MaintenanceTypeId),
                    EquipmentName = eq.Name,
                    TechnicalName = tech.Name
                };

            return await query
                .AsNoTracking()
                .OrderByDescending(x => x.MaintenanceDate)
                .ToListAsync();
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
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// A list of <see cref="TechnicianPerformanceCorrelationDto"/> representing
        /// the 5 technicians with the worst cost/longevity/performance correlation
        /// for equipment decommissioned due to "fallo técnico irreparable".
        /// </returns>
        public async Task<IEnumerable<TechnicianPerformanceCorrelationDto>> GetTechnicianMaintenanceCorrelationAsync()
        {
            // PHASE 1: Irreparable equipment (reason = "fallo técnico irreparable")
            var irreparableEquipments =
                from d in _context.EquipmentDecommissions
                where d.Reason.ToLower() == "fallo técnico irreparable"
                select new
                {
                    d.EquipmentId,
                    d.TechnicalId,
                    d.DecommissionDate
                };

            // PHASE 2: Equipment longevity (from acquisition to decommission)
            var equipmentLongevity =
                from e in _context.Equipments
                join d in irreparableEquipments
                    on e.Id equals d.EquipmentId
                select new
                {
                    d.TechnicalId,
                    EquipmentId = e.Id,
                    LongevityDays = EF.Functions.DateDiffDay(
                        e.AcquisitionDate,
                        d.DecommissionDate)
                };

            // PHASE 3: Total maintenance cost per technician + equipment BEFORE decommission
            var maintenanceCosts =
                from m in _context.Maintenances
                join d in irreparableEquipments
                    on m.EquipmentId equals d.EquipmentId
                where m.MaintenanceDate < d.DecommissionDate
                group m by new { m.TechnicalId, m.EquipmentId } into g
                select new
                {
                    g.Key.TechnicalId,
                    g.Key.EquipmentId,
                    TotalCost = g.Sum(x => x.Cost)
                };

            // PHASE 4: Average technician performance score (assessments)
            var technicianPerformance =
                from a in _context.Assessments
                group a by a.TechnicalId into g
                select new
                {
                    TechnicalId = g.Key,
                    AvgScore = g.Average(x => x.Score.Value)
                };

            // PHASE 5: Final join, aggregation by technician + equipment type, ordering and TOP 5
            var query =
                from lon in equipmentLongevity
                join cost in maintenanceCosts
                    on new { lon.TechnicalId, lon.EquipmentId }
                    equals new { cost.TechnicalId, cost.EquipmentId }
                join perf in technicianPerformance
                    on lon.TechnicalId equals perf.TechnicalId
                join tech in _context.Technicals
                    on lon.TechnicalId equals tech.Id
                join eq in _context.Equipments
                    on lon.EquipmentId equals eq.Id
                join type in _context.EquipmentTypes
                    on eq.EquipmentTypeId equals type.Id
                group new { lon, cost, perf, type } by new
                {
                    tech.Id,
                    tech.Name,
                    EquipmentTypeName = type.Name,
                    perf.AvgScore
                }
                into g
                // Aggregated metrics per technician + equipment type
                let totalCost = g.Sum(x => x.cost.TotalCost)
                let avgLongevity = g.Average(x => (double)x.lon.LongevityDays)
                // Ordering: worst correlation = higher cost, lower longevity, lower rating
                orderby totalCost descending,
                        avgLongevity ascending,
                        g.Key.AvgScore ascending
                select new TechnicianPerformanceCorrelationDto
                {
                    TechnicianId = g.Key.Id,
                    TechnicianName = g.Key.Name,
                    EquipmentTypeName = g.Key.EquipmentTypeName,
                    AveragePerformanceScore = g.Key.AvgScore,
                    TotalMaintenanceCost = totalCost,
                    AverageEquipmentLongevityDays = avgLongevity
                };

            // Read-only query and TOP 5 from the database
            var list = await query
                .AsNoTracking()
                .Take(5)
                .ToListAsync();

            return list;
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
            var technicianPerformance =
                from a in _context.Assessments
                group a by a.TechnicalId into g
                select new
                {
                    TechnicalId = g.Key,
                    AvgScore = g.Average(x => x.Score.Value)
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
                4 => "Calibration",
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
