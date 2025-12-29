using Application.DTOs.ReportRequest;
using Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
// Query usings for report endpoints
using Application.Reports.Queries.GetDecommissionLastYear;
using Application.Reports.Queries.GetMaintenanceHistory;
using Application.Reports.Queries.GetEquipmentTransferHistoryBetweenSections;
using Application.Reports.Queries.GetTechnicianMaintenanceCorrelation;
using Application.Reports.Queries.GetFrequentMaintenanceEquipment;
using Application.Reports.Queries.GetTechnicianPerformanceReport;
using Application.Reports.Queries.GetEquipmentSentToDepartment;

namespace WebApi.Controllers
{
    /// <summary>
    /// Controller for exporting various reports in different formats.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IReportService _reportService;
        private readonly ILogger<ReportController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportController"/> class.
        /// </summary>
        /// <param name="sender">The mediator sender for queries.</param>
        /// <param name="reportService">The report generation service.</param>
        /// <param name="logger">The logger instance.</param>
        public ReportController(
            ISender sender,
            IReportService reportService,
            ILogger<ReportController> logger)
        {
            _sender = sender;
            _reportService = reportService;
            _logger = logger;
        }

        /// <summary>
        /// Exports the report of equipment decommissioned in the last year.
        /// </summary>
        /// <param name="format">The export format (pdf, excel, word).</param>
        [HttpGet("export/decommission-last-year/{format}")]
        public async Task<IActionResult> ExportDecommissionLastYear(string format)
        {
            var data = await _sender.Send(new GetDecommissionLastYearQuery());
            return await GenerateReportFile("EquipmentDecommissionLastYear", format, data);
        }

        /// <summary>
        /// Exports the maintenance history report for a specific equipment.
        /// </summary>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <param name="format">The export format (pdf, excel, word).</param>
        [HttpGet("export/maintenance-history/{equipmentId}/{format}")]
        public async Task<IActionResult> ExportMaintenanceHistory(Guid equipmentId, string format)
        {
            var data = await _sender.Send(new GetMaintenanceHistoryQuery(equipmentId));
            return await GenerateReportFile("EquipmentMaintenanceHistory", format, data);
        }

        /// <summary>
        /// Exports the report of equipment transfers between sections.
        /// </summary>
        /// <param name="format">The export format (pdf, excel, word).</param>
        [HttpGet("export/equipment-transfers-between-sections/{format}")]
        public async Task<IActionResult> ExportEquipmentTransfersBetweenSections(string format)
        {
            var data = await _sender.Send(new GetEquipmentTransferHistoryBetweenSectionsQuery());
            return await GenerateReportFile("EquipmentTransfersBetweenSections", format, data);
        }

        /// <summary>
        /// Exports the report of worst technician maintenance correlation.
        /// </summary>
        /// <param name="format">The export format (pdf, excel, word).</param>
        [HttpGet("export/technician-correlation-worst/{format}")]
        public async Task<IActionResult> ExportTechnicianCorrelationWorst(string format)
        {
            var data = await _sender.Send(new GetTechnicianMaintenanceCorrelationQuery());
            return await GenerateReportFile("TechnicianCorrelationWorst", format, data);
        }

        /// <summary>
        /// Exports the report of equipment with frequent maintenance.
        /// </summary>
        /// <param name="format">The export format (pdf, excel, word).</param>
        [HttpGet("export/frequent-maintenance/{format}")]
        public async Task<IActionResult> ExportFrequentMaintenance(string format)
        {
            var data = await _sender.Send(new GetFrequentMaintenanceEquipmentQuery());
            return await GenerateReportFile("FrequentMaintenanceEquipment", format, data);
        }

        /// <summary>
        /// Exports the report of technician performance bonuses.
        /// </summary>
        /// <param name="format">The export format (pdf, excel, word).</param>
        [HttpGet("export/technician-bonus/{format}")]
        public async Task<IActionResult> ExportTechnicianBonus(string format)
        {
            var data = await _sender.Send(new GetTechnicianPerformanceReportQuery());
            return await GenerateReportFile("TechnicianPerformanceBonus", format, data);
        }

        /// <summary>
        /// Exports the report of equipment sent to a specific department.
        /// </summary>
        /// <param name="departmentId">The department identifier.</param>
        /// <param name="format">The export format (pdf, excel, word).</param>
        [HttpGet("export/equipment-sent-to-department/{departmentId}/{format}")]
        public async Task<IActionResult> ExportEquipmentSentToDepartment(Guid departmentId, string format)
        {
            var data = await _sender.Send(new GetEquipmentSentToDepartmentQuery(departmentId));
            return await GenerateReportFile($"EquipmentSentToDept_{departmentId}", format, data);
        }

        /// <summary>
        /// Generates the report file for download.
        /// </summary>
        /// <param name="reportType">The type of report.</param>
        /// <param name="format">The export format.</param>
        /// <param name="data">The report data.</param>
        /// <returns>The file result for download.</returns>
        private async Task<IActionResult> GenerateReportFile(string reportType, string format, object? data = null)
        {
            try
            {
                if (data == null)
                    return BadRequest($"Could not retrieve data for report: {reportType}");

                var request = new ReportRequestDto
                {
                    ReportId = Guid.NewGuid().ToString(),
                    ReportType = reportType,
                    GeneratedAt = DateTime.UtcNow,
                    Data = JsonSerializer.Serialize(data)
                };

                // NUEVO: un solo método para todos los formatos
                byte[] fileBytes = await _reportService.GenerateReportAsync(request, format);
                string contentType = GetContentType(format);
                string fileName = GetFileName(reportType, format);

                Response.Headers.Append("Content-Disposition", $"attachment; filename=\"{fileName}\"");
                return File(fileBytes, contentType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating report {ReportType} in format {Format}", reportType, format);
                return StatusCode(500, $"Internal error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the file name for the exported report.
        /// </summary>
        /// <param name="reportType">The type of report.</param>
        /// <param name="format">The export format.</param>
        /// <returns>The file name.</returns>
        private string GetFileName(string reportType, string format)
        {
            var reportNames = new Dictionary<string, string>
            {
                { "EquipmentDecommissionLastYear", "equipos-dados-de-baja" },
                { "EquipmentMaintenanceHistory", "historial-mantenimiento" },
                { "FrequentMaintenanceEquipment", "equipos-mantenimiento-frecuente" },
                { "TechnicianPerformanceBonus", "rendimiento-tecnicos-bonificaciones" },
                { "EquipmentTransfersBetweenSections", "traslados-secciones" },
                { "TechnicianCorrelationWorst", "tecnicos-correlacion-peor" },
                { "EquipmentSentToDepartment", "equipos-enviados-departamento" }
            };

            var extensions = new Dictionary<string, string>
            {
                { "pdf", "pdf" },
                { "excel", "xlsx" },
                { "word", "docx" }
            };

            var baseName = reportNames.ContainsKey(reportType) ? reportNames[reportType] : "reporte";
            var extension = extensions.ContainsKey(format.ToLower()) ? extensions[format.ToLower()] : format;

            return $"{baseName}-{DateTime.Now:yyyyMMdd}.{extension}";
        }
        // Nuevo método para obtener el contentType según el formato
        private string GetContentType(string format) => format.ToLower() switch
        {
            "pdf" => "application/pdf",
            "excel" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "word" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            _ => "application/octet-stream"
        };
    }
}
