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

        // ----------------------------------------------------------------------
        //  🔥  Reporte 1: Equipos dados de baja en el último año
        // ----------------------------------------------------------------------
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
                    Department = department != null ? department.Name : "Sin departamento",

                    // 👇 AQUÍ VA EL CAMBIO IMPORTANTE
                    DaysInUse = EF.Functions.DateDiffDay(
                        equipment.AcquisitionDate,
                        decommission.DecommissionDate
                    )
                };

            return await query.ToListAsync();
        }


        // ----------------------------------------------------------------------
        //  🔥  Reporte 2: Historial de mantenimientos de un equipo específico
        // ----------------------------------------------------------------------
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

         // ----------------------------------------------------------------------
        //  🔥  Reporte 5: Equipos con +3 mantenimientos en el último año - CORREGIDO
        // ----------------------------------------------------------------------
        public async Task<IEnumerable<FrequentMaintenanceEquipmentDto>>
            GetFrequentMaintenanceEquipmentAsync()
        {
            var oneYearAgo = DateTime.UtcNow.AddYears(-1);

            // Subconsulta para equipos con más de 3 mantenimientos
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

            // Obtener IDs de equipos frecuentes
            var equipmentIds = frequentList.Select(f => f.EquipmentId).ToList();

            // Consulta principal para obtener datos detallados
            var detailedQuery = 
                from eq in _context.Equipments
                where equipmentIds.Contains(eq.Id)
                join equipmentType in _context.EquipmentTypes on eq.EquipmentTypeId equals equipmentType.Id
                join department in _context.Departments on eq.DepartmentId equals department.Id
                join section in _context.Sections on department.SectionId equals section.Id
                join responsible in _context.Responsibles on section.ResponsibleId equals responsible.Id
                select new
                {
                    Equipment = eq,
                    EquipmentTypeName = equipmentType.Name,
                    Department = department,
                    Responsible = responsible
                };

            var detailedList = await detailedQuery.ToListAsync();

            // Combinar los datos
            var result = from f in frequentList
                         join d in detailedList on f.EquipmentId equals d.Equipment.Id
                         select new FrequentMaintenanceEquipmentDto
                         {
                             EquipmentName = d.Equipment.Name,
                             EquipmentType = d.EquipmentTypeName, // CORREGIDO: Ahora es el nombre, no el ID
                             State = GetStateName(d.Equipment.StateId),
                             AcquisitionDate = d.Equipment.AcquisitionDate,
                             Department = d.Department.Name,
                             DepartmentResponsible = d.Responsible.Name,
                             MaintenanceCountLastYear = f.Count,
                             LastMaintenanceDate = f.LastDate,
                             TotalMaintenanceCost = f.TotalCost,
                             Recommendation = "REEMPLAZO RECOMENDADO",
                             DaysSinceAcquisition = (int)(DateTime.UtcNow - d.Equipment.AcquisitionDate).TotalDays,
                             MonthlyMaintenanceFrequency = Math.Round(f.Count / 12.0m, 2)
                         };

            return result.ToList();
        }

        // ----------------------------------------------------------------------
        //  🔥  Reporte 6: Rendimiento técnicos para bonificaciones
        // ----------------------------------------------------------------------
        public async Task<IEnumerable<TechnicianPerformanceBonusDto>>
            GetTechnicianPerformanceBonusAsync()
        {
            var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);

            var query =
                from tech in _context.Technicals
                join m in _context.Maintenances
                    on tech.Id equals m.TechnicalId into mGroup
                from mg in mGroup.DefaultIfEmpty()
                join a in _context.Assessments
                    on tech.Id equals a.TechnicalId into aGroup
                from ag in aGroup.DefaultIfEmpty()
                where mg == null || mg.MaintenanceDate >= sixMonthsAgo
                group new { mg, ag } by new
                {
                    tech.Id,
                    tech.Name,
                    tech.Specialty,
                    tech.Experience,
                    tech.Email
                }
                into g
                select new TechnicianPerformanceBonusDto
                {
                    TechnicalName = g.Key.Name,
                    Speciality = g.Key.Specialty,
                    Experience = g.Key.Experience,
                    Email = g.Key.Email.Value,
                    TotalInterventions = g.Count(x => x.mg != null),
                    TotalInterventionCost = g.Sum(x => x.mg != null ? x.mg.Cost : 0),
                    TotalRatings = g.Count(x => x.ag != null),
                    AverageRating = g.Where(x => x.ag != null).Average(x => x.ag!.Score.Value),
                    LastRatingDate = g.Where(x => x.ag != null)
                                      .Max(x => x.ag!.AssessmentDate),
                    BonusScore =
                        (g.Count(x => x.mg != null) * 0.4m) +
                        (g.Where(x => x.ag != null).Average(x => x.ag!.Score.Value) * 0.6m),
                    BonusRecommendation =
                        ((g.Count(x => x.mg != null) * 0.4m) +
                         (g.Where(x => x.ag != null).Average(x => x.ag!.Score.Value) * 0.6m)) > 7
                            ? "BONIFICAR"
                            : "MANTENER",
                    DaysWithoutIntervention =
                        g.All(x => x.mg == null)
                            ? 999
                            : (int)(DateTime.UtcNow -
                                g.Where(x => x.mg != null)
                                 .Max(x => x.mg!.MaintenanceDate)
                            ).TotalDays
                };

            return await query
                .OrderByDescending(x => x.BonusScore)
                .ToListAsync();
        }

        // ----------------------------------------------------------------------
        //  🔧 Métodos auxiliares para enums (solución temporal)
        // ----------------------------------------------------------------------
        private static string GetStateName(int id) =>
            id switch
            {
                2 => "UnderMaintenance",
                1 => "Operative",
                3 => "Decommissioned",
                4 => "Disposed",
                _ => "Desconocido"
            };

        private static string GetMaintenanceTypeName(int id) =>
            id switch
            {
                1 => "Preventive",
                2 => "Corrective",
                3 => "Predective",
                4 => "Emergency",
                _ => "Desconocido"
            };

        private static string GetDestinyName(int id) =>
            id switch
            {
                1 => "Department",
                2 => "Disposal",
                3 => "Warehouse",
                _ => "Desconocido"
            };
    }
}
