using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixMaintenanceHistorySP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the old SP and recreate with correct JOIN to Users table
            // Technical inherits from User (TPT), so Name and Email are in Users table
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetEquipmentMaintenanceHistory");
            
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
        u.Name AS TechnicalName,
        t.Specialty AS TechnicalSpeciality,
        t.Experience AS TechnicalExperience,
        u.Email AS TechnicalEmail,
        DATEDIFF(m.MaintenanceDate, e.AcquisitionDate) AS DaysFromAcquisition
    FROM Maintenances m
    INNER JOIN Equipments e ON m.EquipmentId = e.Id
    INNER JOIN Technicals t ON m.TechnicalId = t.Id
    INNER JOIN Users u ON t.Id = u.Id
    LEFT JOIN Departments d ON e.DepartmentId = d.Id
    WHERE m.EquipmentId = p_EquipmentId
    ORDER BY m.MaintenanceDate DESC;
END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Restore the old (incorrect) SP
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetEquipmentMaintenanceHistory");
            
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
    }
}
