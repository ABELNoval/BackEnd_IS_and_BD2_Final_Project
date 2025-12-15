using Application.DTOs.ReportResult;
using Application.Interfaces.Services;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Reports
{
        
    public class ReportQueryService : IReportQueryService
    {

        /// <summary>
        /// Gets all equipment transfers between different sections, including transfer date, origin, destination, sender, and receiver information.
        /// </summary>
        /// <returns>List of equipment transfers between different sections.</returns>
        private readonly AppDbContext _context;

        public ReportQueryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EquipmentTransferBetweenSectionsDto>> GetEquipmentTransferHistoryBetweenSectionsAsync()
        {
            var query = from transfer in _context.Transfers
                        join equipment in _context.Equipments on transfer.EquipmentId equals equipment.Id
                        join eqType in _context.EquipmentTypes on equipment.EquipmentTypeId equals eqType.Id
                        join sourceDept in _context.Departments on transfer.SourceDepartmentId equals sourceDept.Id
                        join sourceSection in _context.Sections on sourceDept.SectionId equals sourceSection.Id
                        join targetDept in _context.Departments on transfer.TargetDepartmentId equals targetDept.Id
                        join targetSection in _context.Sections on targetDept.SectionId equals targetSection.Id
                        join sender in _context.Responsibles on transfer.ResponsibleId equals sender.Id
                        where sourceSection.Id != targetSection.Id
                        select new EquipmentTransferBetweenSectionsDto
                        {
                            EquipmentId = equipment.Id,
                            EquipmentName = equipment.Name,
                            EquipmentType = eqType.Name,
                            TransferDate = transfer.TransferDate,
                            SourceSection = sourceSection.Name,
                            SourceDepartment = sourceDept.Name,
                            TargetSection = targetSection.Name,
                            TargetDepartment = targetDept.Name,
                            SenderName = sender.Name,
                            SenderEmail = sender.Email.Value,
                        };

            return await query
                .AsNoTracking()
                .OrderByDescending(x => x.TransferDate)
                .ToListAsync();
        }

        /// <summary>
        /// REPORT 4 - CRITICAL: Correlation between technicians and irreparable equipment longevity.
        /// Identifies the 5 technicians with the worst performance based on:
        /// 1. High maintenance cost on failed equipment
        /// 2. Low longevity (equipment lasts little)
        /// 3. Equipment decommissioned due to "irreparable technical failure"
        /// 4. Low average rating from supervisors
        /// 5. Type of equipment they specialize in
        /// </summary>
        /// <returns>List of the 5 worst-performing technicians by correlation score.</returns>
        public async Task<IEnumerable<TechnicianPerformanceCorrelationDto>> GetTechnicianMaintenanceCorrelationAsync()
        {
            try
            {
                // PHASE 1: Get irreparable equipment IDs
                var irreparableEquipmentIds = await _context.EquipmentDecommissions
                    .AsNoTracking()
                    .Where(d => d.Reason.ToLower().Contains("irreparable") || d.Reason.ToLower().Contains("fallo técnico"))
                    .Select(d => d.EquipmentId)
                    .ToListAsync();

                if (!irreparableEquipmentIds.Any())
                {
                    // No irreparable equipment found
                    return new List<TechnicianPerformanceCorrelationDto>();
                }

                // PHASE 2: Maintenance costs per irreparable equipment
                var maintenanceCosts = await _context.Maintenances
                    .Where(m => irreparableEquipmentIds.Contains(m.EquipmentId))
                    .GroupBy(m => m.EquipmentId)
                    .Select(g => new
                    {
                        EquipmentId = g.Key,
                        TotalCost = g.Sum(x => x.Cost),
                        MaintenanceCount = g.Count()
                    })
                    .AsNoTracking()
                    .ToListAsync();

                var equipmentWithCosts = maintenanceCosts.Select(m => m.EquipmentId).ToList();

                // PHASE 3: Longevity and type of irreparable equipment
                var equipmentDetails = await (from eq in _context.Equipments
                                                where equipmentWithCosts.Contains(eq.Id)
                                                join decomm in _context.EquipmentDecommissions on eq.Id equals decomm.EquipmentId
                                                join eqType in _context.EquipmentTypes on eq.EquipmentTypeId equals eqType.Id
                                                select new
                                                {
                                                    Equipment = eq,
                                                    Decommission = decomm,
                                                    EquipmentTypeName = eqType.Name,
                                                    DaysInUse = EF.Functions.DateDiffDay(eq.AcquisitionDate, decomm.DecommissionDate)
                                                })
                                                .AsNoTracking()
                                                .ToListAsync();

                // PHASE 4: Technicians who maintained irreparable equipment
                var techniciansWithEquipment = await (from m in _context.Maintenances
                                                        where irreparableEquipmentIds.Contains(m.EquipmentId)
                                                        join tech in _context.Technicals on m.TechnicalId equals tech.Id
                                                        select new { tech.Id, tech.Name, tech.Specialty, m.EquipmentId })
                                                        .AsNoTracking()
                                                        .Distinct()
                                                        .ToListAsync();

                // PHASE 5: Average ratings per technician
                var assessments = await _context.Assessments
                    .GroupBy(a => a.TechnicalId)
                    .Select(g => new
                    {
                        TechnicalId = g.Key,
                        AverageRating = g.Average(x => x.Score.Value),
                        AssessmentCount = g.Count()
                    })
                    .AsNoTracking()
                    .ToListAsync();

                // PHASE 6: Analytical calculations per technician
                var results = new List<TechnicianPerformanceCorrelationDto>();
                var technicianGroups = techniciansWithEquipment.GroupBy(x => x.Id).ToList();

                foreach (var techGroup in technicianGroups)
                {
                    var techId = techGroup.Key;
                    var techName = techGroup.First().Name;
                    var techSpecialty = techGroup.First().Specialty;

                    // Irreparable equipment maintained by this technician
                    var techEquipmentIds = techGroup.Select(x => x.EquipmentId).Distinct().ToList();
                    var techEquipments = equipmentDetails.Where(e => techEquipmentIds.Contains(e.Equipment.Id)).ToList();
                    var techCosts = maintenanceCosts.Where(c => techEquipmentIds.Contains(c.EquipmentId)).ToList();

                    decimal totalCost = techCosts.Sum(c => c.TotalCost);
                    double avgLongevityDays = techEquipments.Count > 0 ? techEquipments.Average(e => e.DaysInUse) : 0;

                    // Average rating
                    var techAssessment = assessments.FirstOrDefault(a => a.TechnicalId == techId);
                    decimal avgRating = techAssessment?.AverageRating ?? 0;

                    // Correlation score: 100 / (cost per day)
                    decimal correlationScore = 0;
                    if (avgLongevityDays > 0 && totalCost > 0)
                    {
                        decimal costPerDay = totalCost / (decimal)avgLongevityDays;
                        correlationScore = 100m / costPerDay;
                    }

                    // Specialization: most maintained equipment type
                    var specialization = techEquipments
                        .GroupBy(e => e.EquipmentTypeName)
                        .OrderByDescending(g => g.Count())
                        .FirstOrDefault()?.Key ?? "Not specialized";

                    results.Add(new TechnicianPerformanceCorrelationDto
                    {
                        Rank = 0, // Assigned in phase 7
                        TechnicalName = techName,
                        Specialty = techSpecialty,
                        AverageRating = Math.Round(avgRating, 2),
                        IrreparableEquipmentCount = techEquipments.Count,
                        TotalMaintenanceCost = Math.Round(totalCost, 2),
                        AverageCostPerEquipment = techEquipments.Any() ? Math.Round(totalCost / techEquipments.Count, 2) : 0,
                        AverageLongevityDays = Math.Round(avgLongevityDays, 0),
                        CorrelationScore = Math.Round(correlationScore, 2),
                        EquipmentSpecialization = specialization
                    });
                }

                // PHASE 7: Top 5 worst technicians by correlation score
                var top5Worst = results
                    .OrderBy(r => r.CorrelationScore)
                    .Take(5)
                    .Select((item, index) => new TechnicianPerformanceCorrelationDto
                    {
                        Rank = index + 1,
                        TechnicalName = item.TechnicalName,
                        Specialty = item.Specialty,
                        AverageRating = item.AverageRating,
                        IrreparableEquipmentCount = item.IrreparableEquipmentCount,
                        TotalMaintenanceCost = item.TotalMaintenanceCost,
                        AverageCostPerEquipment = item.AverageCostPerEquipment,
                        AverageLongevityDays = item.AverageLongevityDays,
                        CorrelationScore = item.CorrelationScore,
                        EquipmentSpecialization = item.EquipmentSpecialization
                    })
                    .ToList();

                return top5Worst;
            }
            catch (Exception ex)
            {
                // Log error (no emojis or AI-like marks)
                Console.WriteLine($"ERROR in GetTechnicianMaintenanceCorrelationAsync: {ex.Message}");
                return new List<TechnicianPerformanceCorrelationDto>();
            }
        }
    

        /// <summary>
        /// Gets equipment decommissioned in the last year.
        /// </summary>
        /// <returns>List of equipment decommissioned in the last year.</returns>
        public async Task<IEnumerable<EquipmentDecommissionLastYearDto>> GetEquipmentDecommissionLastYearAsync()
        {
            var oneYearAgo = DateTime.UtcNow.AddYears(-1);

            var query =
                from decommission in _context.EquipmentDecommissions
                where decommission.DecommissionDate >= oneYearAgo
                join equipment in _context.Equipments
                    on decommission.EquipmentId equals equipment.Id
                join technical in _context.Technicals
                    on decommission.TechnicalId equals technical.Id
                join department in _context.Departments
                    on equipment.DepartmentId equals department.Id into deptGroup
                from department in deptGroup.DefaultIfEmpty()
                select new EquipmentDecommissionLastYearDto
                {
                    EquipmentName = equipment.Name,
                    AcquisitionDate = equipment.AcquisitionDate,
                    State = GetStateName(equipment.StateId),
                    DecommissionDate = decommission.DecommissionDate,
                    Reason = decommission.Reason,
                    DestinyType = GetDestinyName(decommission.DestinyTypeId),
                    TechnicalName = technical.Name,
                    TechnicalSpeciality = technical.Specialty,
                    TechnicalEmail = technical.Email.Value,
                    Department = department != null ? department.Name : "No department",

                    // 👇 AQUÍ VA EL CAMBIO IMPORTANTE
                    DaysInUse = EF.Functions.DateDiffDay(
                        equipment.AcquisitionDate,
                        decommission.DecommissionDate
                    )
                };

            return await query.ToListAsync();
        }

        /// <summary>
        /// Gets the maintenance history for a specific equipment.
        /// </summary>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns>List of maintenance history records for the equipment.</returns>
        public async Task<IEnumerable<EquipmentMaintenanceHistoryDto>>
            GetEquipmentMaintenanceHistoryAsync(Guid equipmentId)
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
                    Cost = m.Cost,
                    EquipmentName = eq.Name,
                    EquipmentState = GetStateName(eq.StateId),
                    TechnicalName = tech.Name,
                    TechnicalSpeciality = tech.Specialty,
                    TechnicalExperience = tech.Experience,
                    TechnicalEmail = tech.Email.Value,
                    DaysFromAcquisition = (int)(m.MaintenanceDate - eq.AcquisitionDate).TotalDays
                };

            return await query
                .OrderByDescending(x => x.MaintenanceDate)
                .ToListAsync();
        }

        /// <summary>
        /// Gets equipment with more than 3 maintenances in the last year.
        /// </summary>
        /// <returns>List of equipment with frequent maintenance in the last year.</returns>
        public async Task<IEnumerable<FrequentMaintenanceEquipmentDto>>
            GetFrequentMaintenanceEquipmentAsync()
        {
            var oneYearAgo = DateTime.UtcNow.AddYears(-1);

            // Subquery for equipment with more than 3 maintenances
            var frequentQuery = 
                from m in _context.Maintenances
                where m.MaintenanceDate >= oneYearAgo
                group m by m.EquipmentId into g
                where g.Count() > 3
                select new
                {
                    EquipmentId = g.Key,
                    Count = g.Count(),
                    LastDate = g.Max(x => x.MaintenanceDate),
                    TotalCost = g.Sum(x => x.Cost)
                };

            var frequentList = await frequentQuery.ToListAsync();

            if (!frequentList.Any())
                return new List<FrequentMaintenanceEquipmentDto>();

            // Get IDs of frequent equipment
            var equipmentIds = frequentList.Select(f => f.EquipmentId).ToList();

            // Main query to get detailed data
            var detailedQuery = 
                from eq in _context.Equipments
                where equipmentIds.Contains(eq.Id)
                join equipmentType in _context.EquipmentTypes on eq.EquipmentTypeId equals equipmentType.Id
                join department in _context.Departments on eq.DepartmentId equals department.Id
                join section in _context.Sections on department.SectionId equals section.Id
                select new
                {
                    Equipment = eq,
                    EquipmentTypeName = equipmentType.Name,
                    Department = department,
                };

            var detailedList = await detailedQuery.ToListAsync();

            // Combine the data
            var result = from f in frequentList
                         join d in detailedList on f.EquipmentId equals d.Equipment.Id
                         select new FrequentMaintenanceEquipmentDto
                         {
                             EquipmentName = d.Equipment.Name,
                             EquipmentType = d.EquipmentTypeName,
                             State = GetStateName(d.Equipment.StateId),
                             AcquisitionDate = d.Equipment.AcquisitionDate,
                             Department = d.Department.Name,
                             MaintenanceCountLastYear = f.Count,
                             LastMaintenanceDate = f.LastDate,
                             TotalMaintenanceCost = f.TotalCost,
                             Recommendation = "REPLACEMENT RECOMMENDED",
                             DaysSinceAcquisition = (int)(DateTime.UtcNow - d.Equipment.AcquisitionDate).TotalDays,
                             MonthlyMaintenanceFrequency = Math.Round(f.Count / 12.0m, 2)
                         };

            return result.ToList();
        }

        /// <summary>
        /// Gets technician performance for bonuses in the last 6 months.
        /// </summary>
        /// <returns>List of technician performance bonus data.</returns>
        public async Task<IEnumerable<TechnicianPerformanceBonusDto>> GetTechnicianPerformanceBonusAsync()
        {
            var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);
            
            try
            {
                // Get all required data separately
                var technicians = await _context.Technicals.ToListAsync();
                var maintenances = await _context.Maintenances
                    .Where(m => m.MaintenanceDate >= sixMonthsAgo)
                    .ToListAsync();
                var assessments = await _context.Assessments.ToListAsync();

                var results = new List<TechnicianPerformanceBonusDto>();

                foreach (var tech in technicians)
                {
                    var techMaintenances = maintenances
                        .Where(m => m.TechnicalId == tech.Id)
                        .ToList();
                        
                    var techAssessments = assessments
                        .Where(a => a.TechnicalId == tech.Id)
                        .ToList();
                    
                    // Only process technicians with at least one intervention or rating
                    if (!techMaintenances.Any() && !techAssessments.Any())
                        continue;
                    
                    var totalInterventions = techMaintenances.Count;
                    var totalInterventionCost = techMaintenances.Sum(m => m.Cost);
                    var totalRatings = techAssessments.Count;
                    
                    // Calculate average rating (safe against division by zero)
                    decimal averageRating = 0;
                    if (totalRatings > 0)
                    {
                        averageRating = techAssessments.Average(a => a.Score.Value);
                    }
                    
                    // Last rating date (nullable)
                    DateTime? lastRatingDate = null;
                    if (techAssessments.Any())
                    {
                        lastRatingDate = techAssessments.Max(a => a.AssessmentDate);
                    }
                    
                    // Calculate bonus score
                    decimal bonusScore = (totalInterventions * 0.4m) + (averageRating * 0.6m);
                    
                    // Determine recommendation
                    string bonusRecommendation = bonusScore > 7 ? "BONUS" : "KEEP";
                    
                    // Calculate days without intervention
                    int daysWithoutIntervention = 999;
                    if (techMaintenances.Any())
                    {
                        var lastMaintenance = techMaintenances.Max(m => m.MaintenanceDate);
                        daysWithoutIntervention = (int)(DateTime.UtcNow - lastMaintenance).TotalDays;
                    }
                    
                    results.Add(new TechnicianPerformanceBonusDto
                    {
                        TechnicalName = tech.Name,
                        Speciality = tech.Specialty,
                        Experience = tech.Experience,
                        Email = tech.Email.Value,
                        TotalInterventions = totalInterventions,
                        TotalInterventionCost = totalInterventionCost,
                        TotalRatings = totalRatings,
                        AverageRating = Math.Round(averageRating, 2),
                        LastRatingDate = lastRatingDate,
                        BonusScore = Math.Round(bonusScore, 2),
                        BonusRecommendation = bonusRecommendation,
                        DaysWithoutIntervention = daysWithoutIntervention
                    });
                }
                
                return results
                    .OrderByDescending(r => r.BonusScore)
                    .ThenByDescending(r => r.TotalInterventions)
                    .ToList();
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error in GetTechnicianPerformanceBonusAsync: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                
                // Return an empty list in case of error to avoid breaking the flow
                return new List<TechnicianPerformanceBonusDto>();
            }
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
        /// Gets the maintenance type name for a given type id.
        /// </summary>
        private static string GetMaintenanceTypeName(int id) =>
            id switch
            {
                1 => "Preventive",
                2 => "Corrective",
                3 => "Predective",
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
        /// Gets the list of equipment sent to a specific department.
        /// </summary>
        /// <param name="targetDepartmentId">The target department identifier.</param>
        /// <returns>List of equipment sent to the specified department.</returns>
        public async Task<IEnumerable<EquipmentSentToDepartmentDto>> GetEquipmentSentToDepartmentAsync(Guid targetDepartmentId)
        {

            var query =
                from decomm in _context.EquipmentDecommissions
                where decomm.DepartmentId == targetDepartmentId && decomm.DestinyTypeId == 1 // 1 = Department
                join equipment in _context.Equipments on decomm.EquipmentId equals equipment.Id
                join equipmentType in _context.EquipmentTypes on equipment.EquipmentTypeId equals equipmentType.Id
                join technical in _context.Technicals on decomm.TechnicalId equals technical.Id
                join sourceDept in _context.Departments on equipment.DepartmentId equals sourceDept.Id
                join sourceSection in _context.Sections on sourceDept.SectionId equals sourceSection.Id
                join targetDept in _context.Departments on decomm.DepartmentId equals targetDept.Id
                join recipient in _context.Responsibles on decomm.RecipientId equals recipient.Id
                select new EquipmentSentToDepartmentDto
                {
                    EquipmentName = equipment.Name,
                    EquipmentType = equipmentType.Name,
                    SendDate = decomm.DecommissionDate,
                    Reason = decomm.Reason,
                    SenderName = technical.Name,
                    SenderEmail = technical.Email.Value,
                    SenderDepartment = sourceDept.Name,
                    SenderCompany = sourceSection.Name,
                    ReceiverName = recipient.Name,
                    ReceiverEmail = recipient.Email.Value,
                    ReceiverDepartment = targetDept.Name,
                    EquipmentState = GetStateName(equipment.StateId),
                    IsDefective = decomm.Reason.Contains("defect", StringComparison.OrdinalIgnoreCase) ||
                                 decomm.Reason.Contains("fail", StringComparison.OrdinalIgnoreCase)
                };

            return await query
                .AsNoTracking()
                .OrderByDescending(x => x.SendDate)
                .ToListAsync();
        }
    }
}
