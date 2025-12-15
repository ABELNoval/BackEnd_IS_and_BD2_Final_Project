using Application.DTOs.ReportRequest;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportQueryService _queryService;
        private readonly IReportService _reportService;
        private readonly ILogger<ReportController> _logger;

        public ReportController(
            IReportQueryService queryService,
            IReportService reportService,
            ILogger<ReportController> logger)
        {
            _queryService = queryService;
            _reportService = reportService;
            _logger = logger;
        }

        // ----------------------------------------------------------------------
        // ENDPOINTS DE EXPORTACIÓN DIRECTA
        // ----------------------------------------------------------------------
        
        //1
        [HttpGet("export/decommission-last-year/{format}")]
        public async Task<IActionResult> ExportDecommissionLastYear(string format)
        {
            var data = await _queryService.GetEquipmentDecommissionLastYearAsync();
            return await GenerateReportFile("EquipmentDecommissionLastYear", format, data);
        }

        //2
        [HttpGet("export/maintenance-history/{equipmentId}/{format}")]
        public async Task<IActionResult> ExportMaintenanceHistory(Guid equipmentId, string format)
        {
            var data = await _queryService.GetEquipmentMaintenanceHistoryAsync(equipmentId);
            return await GenerateReportFile("EquipmentMaintenanceHistory", format, data);
        }

        //5
        [HttpGet("export/frequent-maintenance/{format}")]
        public async Task<IActionResult> ExportFrequentMaintenance(string format)
        {
            var data = await _queryService.GetFrequentMaintenanceEquipmentAsync();
            return await GenerateReportFile("FrequentMaintenanceEquipment", format, data);
        }

        //6
        [HttpGet("export/technician-bonus/{format}")]
        public async Task<IActionResult> ExportTechnicianBonus(string format)
        {
            var data = await _queryService.GetTechnicianPerformanceBonusAsync();
            return await GenerateReportFile("TechnicianPerformanceBonus", format, data);
        }

        // 3
        [HttpGet("export/equipment-transfers-between-sections/{format}")]
        public async Task<IActionResult> ExportEquipmentTransfersHistoryBetweenSections(string format)
        {
            try
            {
                var data = await _queryService.GetEquipmentTransferHistoryBetweenSectionsAsync();
                return await GenerateReportFile("EquipmentTransfersBetweenSections", format, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error export traslados");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // 4
        [HttpGet("export/technician-correlation-worst/{format}")]
        public async Task<IActionResult> ExportTechnicianCorrelationWorst(string format)
        {
            try
            {
                var data = await _queryService.GetTechnicianMaintenanceCorrelationAsync();
                return await GenerateReportFile("TechnicianCorrelationWorst", format, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error export correlación");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // 7
        [HttpGet("export/equipment-sent-to-department/{departmentId}/{format}")]
        public async Task<IActionResult> ExportEquipmentSentToDepartment(Guid departmentId, string format)
        {
            try
            {
                var data = await _queryService.GetEquipmentSentToDepartmentAsync(departmentId);
                return await GenerateReportFile($"EquipmentSentToDept_{departmentId}", format, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error export enviados");
                return StatusCode(500, new { error = ex.Message });
            }
        }

 
        // MÉTODO AUXILIAR PARA GENERAR ARCHIVOS (EXISTENTE)
        private async Task<IActionResult> GenerateReportFile(string reportType, string format, object? data = null)
        {
            try
            {
                if (data == null)
                {
                    return BadRequest($"No se pudo obtener datos para el reporte: {reportType}");
                }

                // Construir el DTO de solicitud
                var request = new ReportRequestDto
                {
                    ReportId = Guid.NewGuid().ToString(),
                    ReportType = reportType,
                    GeneratedAt = DateTime.UtcNow,
                    Data = JsonSerializer.Serialize(data)
                };

                byte[] fileBytes;
                string contentType;
                string fileName = GetFileName(reportType, format);

                switch (format.ToLower())
                {
                    case "pdf":
                        fileBytes = await _reportService.GeneratePdfReport(request);
                        contentType = "application/pdf";
                        break;

                    case "excel":
                        fileBytes = await _reportService.GenerateExcelReport(request);
                        contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        break;

                    case "word":
                        fileBytes = await _reportService.GenerateWordReport(request);
                        contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;

                    default:
                        return BadRequest($"Formato no soportado: {format}");
                }

                // HEADERS CRÍTICOS para descarga correcta
                Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{fileName}\"");
                return File(fileBytes, contentType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando reporte {ReportType} en formato {Format}", reportType, format);
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

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
                // For department-specific exported file names we use the base name plus dept id at call site
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
    }
}
