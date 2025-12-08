using Application.DTOs.ReportRequest;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.Text.Json;

namespace Infrastructure.Reports
{
    public class ExcelReportGenerator
    {
        public Task<byte[]> Generate(ReportRequestDto request)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Reporte");

            // Título
            sheet.Cells["A1"].Value = $"Reporte: {GetReportTitle(request.ReportType)}";
            sheet.Cells["A1"].Style.Font.Bold = true;
            sheet.Cells["A1"].Style.Font.Size = 18;

            sheet.Cells["A2"].Value = $"ID: {request.ReportId}";
            sheet.Cells["A3"].Value = $"Generado: {request.GeneratedAt:yyyy-MM-dd HH:mm}";
            sheet.Cells["A3"].Style.Font.Italic = true;

            // Deserializar datos
            var data = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(
                request.Data,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            // Si no hay datos
            if (data == null || data.Count == 0)
            {
                sheet.Cells["A5"].Value = "No hay datos disponibles para este reporte.";
                return Task.FromResult(package.GetAsByteArray());
            }

            // Escribir encabezados (fila 5)
            var headers = data[0].Keys.ToList();
            for (int i = 0; i < headers.Count; i++)
            {
                sheet.Cells[5, i + 1].Value = GetColumnDisplayName(headers[i], request.ReportType);
                sheet.Cells[5, i + 1].Style.Font.Bold = true;
                sheet.Cells[5, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                sheet.Cells[5, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            // Escribir datos
            int row = 6;
            foreach (var rowData in data)
            {
                int col = 1;
                foreach (var header in headers)
                {
                    var value = rowData[header];
                    
                    // Formatear fechas
                    if (value is JsonElement jsonElement)
                    {
                        if (jsonElement.ValueKind == JsonValueKind.String && 
                            DateTime.TryParse(jsonElement.GetString(), out DateTime dateValue))
                        {
                            sheet.Cells[row, col].Value = dateValue;
                            sheet.Cells[row, col].Style.Numberformat.Format = "dd/MM/yyyy";
                        }
                        else if (jsonElement.ValueKind == JsonValueKind.Number)
                        {
                            sheet.Cells[row, col].Value = jsonElement.GetDecimal();
                        }
                        else
                        {
                            sheet.Cells[row, col].Value = value?.ToString();
                        }
                    }
                    else
                    {
                        sheet.Cells[row, col].Value = value?.ToString();
                    }
                    col++;
                }
                row++;
            }

            // Formatear como tabla
            var range = sheet.Cells[5, 1, row - 1, headers.Count];
            var table = sheet.Tables.Add(range, "ReporteTable");
            table.TableStyle = TableStyles.Medium6;
            table.ShowHeader = true;
            table.ShowFilter = true;

            // Ajustar ancho de columnas
            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

            // Congelar encabezados
            sheet.View.FreezePanes(6, 1);

            return Task.FromResult(package.GetAsByteArray());
        }

        private string GetReportTitle(string reportType)
        {
            return reportType switch
            {
                "EquipmentDecommissionLastYear" => "Equipos dados de baja en el último año",
                "EquipmentMaintenanceHistory" => "Historial de mantenimientos del equipo",
                "EquipmentTransfers" => "Equipos trasladados entre secciones",
                "TechnicianPerformanceCorrelation" => "Correlación rendimiento técnicos vs longevidad equipos",
                "FrequentMaintenanceEquipment" => "Equipos con mantenimientos frecuentes (>3 en último año)",
                "TechnicianPerformanceBonus" => "Rendimiento técnicos para bonificaciones",
                "EquipmentToDepartment" => "Equipos enviados a departamento específico",
                _ => "Reporte General"
            };
        }

        private string GetColumnDisplayName(string columnName, string reportType)
        {
            var displayNames = new Dictionary<string, string>
            {
                { "EquipmentName", "Equipo" },
                { "DecommissionDate", "Fecha de baja" },
                { "Reason", "Motivo" },
                { "DestinyType", "Destino final" },
                { "TechnicalName", "Técnico responsable" },
                { "MaintenanceDate", "Fecha mantenimiento" },
                { "MaintenanceType", "Tipo de mantenimiento" },
                { "Cost", "Costo" },
                { "TechnicalSpeciality", "Especialidad" },
                { "TransferDate", "Fecha transferencia" },
                { "SourceDepartment", "Departamento origen" },
                { "TargetDepartment", "Departamento destino" },
                { "ResponsibleName", "Responsable" },
                { "AcquisitionDate", "Fecha de adquisición" },
                { "State", "Estado" },
                { "Department", "Departamento" },
                { "DaysInUse", "Días en uso" },
                { "TotalMaintenances", "Total de mantenimientos" },
                { "TotalMaintenanceCost", "Costo total mantenimientos" },
                { "AverageRating", "Valoración promedio" },
                { "CorrelationScore", "Puntaje correlación" },
                { "Recommendation", "Recomendación" },
                { "BonusScore", "Puntaje bonificación" },
                { "BonusRecommendation", "Recomendación bonificación" },
                { "SendingCompany", "Empresa que envía" },
                { "IsDefective", "Es defectuoso" }
            };

            return displayNames.ContainsKey(columnName) ? displayNames[columnName] : columnName;
        }
    }
}