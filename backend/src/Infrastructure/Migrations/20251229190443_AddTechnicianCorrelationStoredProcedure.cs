using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTechnicianCorrelationStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Stored Procedure for Report 4: Technician performance vs equipment longevity
            migrationBuilder.Sql(@"
CREATE PROCEDURE GetTechnicianMaintenanceCorrelation()
BEGIN
    -- CTE 1: Equipos con fallo técnico irreparable
    WITH IrreparableEquipments AS (
        SELECT 
            EquipmentId, 
            TechnicalId, 
            DecommissionDate
        FROM EquipmentDecommissions
        WHERE LOWER(Reason) = 'fallo técnico irreparable'
    ),
    -- CTE 2: Longevidad del equipo (días desde adquisición hasta baja)
    EquipmentLongevity AS (
        SELECT 
            ie.TechnicalId,
            e.Id AS EquipmentId,
            DATEDIFF(ie.DecommissionDate, e.AcquisitionDate) AS LongevityDays
        FROM Equipments e
        INNER JOIN IrreparableEquipments ie ON e.Id = ie.EquipmentId
    ),
    -- CTE 3: Costo total de mantenimiento por técnico + equipo ANTES de la baja
    MaintenanceCosts AS (
        SELECT 
            m.TechnicalId,
            m.EquipmentId,
            SUM(m.Cost) AS TotalCost
        FROM Maintenances m
        INNER JOIN IrreparableEquipments ie ON m.EquipmentId = ie.EquipmentId
        WHERE m.MaintenanceDate < ie.DecommissionDate
        GROUP BY m.TechnicalId, m.EquipmentId
    ),
    -- CTE 4: Score promedio por técnico
    TechnicianPerformance AS (
        SELECT 
            TechnicalId,
            AVG(Score) AS AvgScore
        FROM Assessments
        GROUP BY TechnicalId
    )
    -- Resultado final: JOIN de todas las CTEs, agrupación y TOP 5
    SELECT 
        tech.Id AS TechnicianId,
        tech.Name AS TechnicianName,
        et.Name AS EquipmentTypeName,
        tp.AvgScore AS AveragePerformanceScore,
        SUM(mc.TotalCost) AS TotalMaintenanceCost,
        AVG(el.LongevityDays) AS AverageEquipmentLongevityDays
    FROM EquipmentLongevity el
    INNER JOIN MaintenanceCosts mc 
        ON el.TechnicalId = mc.TechnicalId 
        AND el.EquipmentId = mc.EquipmentId
    INNER JOIN TechnicianPerformance tp 
        ON el.TechnicalId = tp.TechnicalId
    INNER JOIN Users tech 
        ON el.TechnicalId = tech.Id
    INNER JOIN Equipments eq 
        ON el.EquipmentId = eq.Id
    INNER JOIN EquipmentTypes et 
        ON eq.EquipmentTypeId = et.Id
    GROUP BY 
        tech.Id, 
        tech.Name, 
        et.Name, 
        tp.AvgScore
    ORDER BY 
        SUM(mc.TotalCost) DESC,
        AVG(el.LongevityDays) ASC,
        tp.AvgScore ASC
    LIMIT 5;
END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTechnicianMaintenanceCorrelation");
        }
    }
}
