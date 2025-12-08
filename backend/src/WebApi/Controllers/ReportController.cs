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
        // 🔥 ENDPOINTS GET PARA DATOS JSON (para mostrar en tabla)
        // ----------------------------------------------------------------------
        [HttpGet("decommission-last-year")]
        public async Task<IActionResult> GetDecommissionLastYear()
        {
            try
            {
                var result = await _queryService.GetEquipmentDecommissionLastYearAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetDecommissionLastYear");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("maintenance-history/{equipmentId}")]
        public async Task<IActionResult> GetMaintenanceHistory(Guid equipmentId)
        {
            try
            {
                var result = await _queryService.GetEquipmentMaintenanceHistoryAsync(equipmentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetMaintenanceHistory");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("frequent-maintenance")]
        public async Task<IActionResult> GetFrequentMaintenance()
        {
            try
            {
                var result = await _queryService.GetFrequentMaintenanceEquipmentAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetFrequentMaintenance");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("technician-bonus")]
        public async Task<IActionResult> GetTechnicianBonus()
        {
            try
            {
                var result = await _queryService.GetTechnicianPerformanceBonusAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetTechnicianBonus");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // ----------------------------------------------------------------------
        // 🔥 ENDPOINTS DE EXPORTACIÓN DIRECTA (NUEVOS - PARA FRONTEND)
        // ----------------------------------------------------------------------

        [HttpGet("export/decommission-last-year/{format}")]
        public async Task<IActionResult> ExportDecommissionLastYear(string format)
        {
            return await GenerateReportFile("EquipmentDecommissionLastYear", format, null);
        }

        [HttpGet("export/maintenance-history/{equipmentId}/{format}")]
        public async Task<IActionResult> ExportMaintenanceHistory(Guid equipmentId, string format)
        {
            var data = await _queryService.GetEquipmentMaintenanceHistoryAsync(equipmentId);
            return await GenerateReportFile("EquipmentMaintenanceHistory", format, data);
        }

        [HttpGet("export/frequent-maintenance/{format}")]
        public async Task<IActionResult> ExportFrequentMaintenance(string format)
        {
            return await GenerateReportFile("FrequentMaintenanceEquipment", format, null);
        }

        [HttpGet("export/technician-bonus/{format}")]
        public async Task<IActionResult> ExportTechnicianBonus(string format)
        {
            return await GenerateReportFile("TechnicianPerformanceBonus", format, null);
        }

        // ----------------------------------------------------------------------
        // 🔥 MÉTODO AUXILIAR PARA GENERAR ARCHIVOS
        // ----------------------------------------------------------------------
        private async Task<IActionResult> GenerateReportFile(string reportType, string format, object? data = null)
        {
            try
            {
                // Si no nos pasan datos, los obtenemos del servicio
                if (data == null)
                {
                    data = reportType switch
                    {
                        "EquipmentDecommissionLastYear" => await _queryService.GetEquipmentDecommissionLastYearAsync(),
                        "FrequentMaintenanceEquipment" => await _queryService.GetFrequentMaintenanceEquipmentAsync(),
                        "TechnicianPerformanceBonus" => await _queryService.GetTechnicianPerformanceBonusAsync(),
                        _ => null
                    };
                }

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
                { "TechnicianPerformanceBonus", "rendimiento-tecnicos-bonificaciones" }
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

        // ----------------------------------------------------------------------
        // 🔥 ENDPOINTS POST ORIGINALES (mantener para compatibilidad)
        // ----------------------------------------------------------------------
        [HttpPost("export/pdf")]
        public async Task<IActionResult> ExportPdf([FromBody] ReportRequestDto request)
        {
            var bytes = await _reportService.GeneratePdfReport(request);
            return File(bytes, "application/pdf", $"{request.ReportType}.pdf");
        }

        [HttpPost("export/excel")]
        public async Task<IActionResult> ExportExcel([FromBody] ReportRequestDto request)
        {
            var bytes = await _reportService.GenerateExcelReport(request);
            return File(bytes, 
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                        $"{request.ReportType}.xlsx");
        }

        [HttpPost("export/word")]
        public async Task<IActionResult> ExportWord([FromBody] ReportRequestDto request)
        {
            var bytes = await _reportService.GenerateWordReport(request);
            return File(bytes, 
                        "application/vnd.openxmlformats-officedocument.wordprocessingml.document", 
                        $"{request.ReportType}.docx");
        }
    }
}