using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReportStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Stored Procedure for Report 1: Equipment decommissioned last year
            migrationBuilder.Sql(@"
CREATE PROCEDURE GetEquipmentDecommissionLastYear()
BEGIN
    SELECT 
        e.Name AS EquipmentName,
        ed.Reason AS DecommissionCause,
        CASE 
            WHEN ed.DestinyTypeId = 1 AND d.Name IS NOT NULL THEN d.Name
            WHEN ed.DestinyTypeId = 1 THEN 'Department'
            WHEN ed.DestinyTypeId = 2 THEN 'Disposal'
            WHEN ed.DestinyTypeId = 3 THEN 'Warehouse'
            ELSE 'Unknown'
        END AS FinalDestination,
        COALESCE(u.Name, 'No recipient assigned') AS ReceiverName,
        ed.DecommissionDate
    FROM EquipmentDecommissions ed
    INNER JOIN Equipments e ON ed.EquipmentId = e.Id
    LEFT JOIN Users u ON ed.RecipientId = u.Id
    LEFT JOIN Departments d ON ed.DepartmentId = d.Id
    WHERE ed.DecommissionDate >= DATE_SUB(UTC_TIMESTAMP(), INTERVAL 1 YEAR)
    ORDER BY ed.DecommissionDate DESC;
END
            ");

            // Stored Procedure for Report 2: Equipment maintenance history
            migrationBuilder.Sql(@"
CREATE PROCEDURE GetEquipmentMaintenanceHistory(IN p_EquipmentId CHAR(36))
BEGIN
    SELECT 
        m.Id AS MaintenanceId,
        m.MaintenanceDate,
        CASE m.MaintenanceTypeId
            WHEN 1 THEN 'Preventive'
            WHEN 2 THEN 'Corrective'
            WHEN 3 THEN 'Predictive'
            WHEN 4 THEN 'Emergency'
            ELSE 'Unknown'
        END AS MaintenanceType,
        m.Cost,
        e.Name AS EquipmentName,
        CASE e.StateId
            WHEN 1 THEN 'Operative'
            WHEN 2 THEN 'UnderMaintenance'
            WHEN 3 THEN 'Decommissioned'
            WHEN 4 THEN 'Disposed'
            ELSE 'Unknown'
        END AS EquipmentState,
        COALESCE(d.Name, 'N/A (Warehouse/Disposed)') AS Department,
        t.Name AS TechnicalName,
        t.Specialty AS TechnicalSpeciality,
        t.Experience AS TechnicalExperience,
        t.Email AS TechnicalEmail,
        DATEDIFF(m.MaintenanceDate, e.AcquisitionDate) AS DaysFromAcquisition
    FROM Maintenances m
    INNER JOIN Equipments e ON m.EquipmentId = e.Id
    INNER JOIN Technicals t ON m.TechnicalId = t.Id
    LEFT JOIN Departments d ON e.DepartmentId = d.Id
    WHERE m.EquipmentId = p_EquipmentId
    ORDER BY m.MaintenanceDate DESC;
END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetEquipmentDecommissionLastYear");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetEquipmentMaintenanceHistory");
        }
    }
}
