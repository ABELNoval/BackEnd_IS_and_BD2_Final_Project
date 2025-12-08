using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Application.DTOs.ReportRequest;
using System.Text.Json;

namespace Infrastructure.Reports
{
    public class PdfReportGenerator
    {
        public PdfReportGenerator()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<byte[]> Generate(ReportRequestDto request)
        {
            var data = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(
                request.Data,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? new List<Dictionary<string, object>>();

            var pdf = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Margin(40);
                    page.Size(PageSizes.A4);

                    // Encabezado
                    page.Header()
                        .PaddingBottom(20)
                        .AlignCenter()
                        .Text($"Reporte: {GetReportTitle(request.ReportType)}")
                        .FontSize(20)
                        .Bold();

                    // Contenido
                    page.Content().Table(table =>
                    {
                        // Obtener columnas si hay datos
                        var columns = data.Count > 0 ? data[0].Keys.ToList() : new List<string>();

                        // Si no hay columnas (porque no hay datos o la primera fila está vacía)
                        if (columns.Count == 0)
                        {
                            table.ColumnsDefinition(columnsDefinition => columnsDefinition.RelativeColumn());
                            table.Cell().Text("No hay datos para mostrar.");
                            return;
                        }

                        // Definir columnas
                        table.ColumnsDefinition(columnsDefinition =>
                        {
                            foreach (var _ in columns)
                                columnsDefinition.RelativeColumn();
                        });

                        // Encabezados
                        foreach (var col in columns)
                        {
                            table.Cell()
                                .Background(Colors.Grey.Lighten3)
                                .PaddingVertical(5)
                                .Text(GetColumnDisplayName(col, request.ReportType))
                                .Bold()
                                .FontSize(11);
                        }

                        // Filas
                        foreach (var row in data)
                        {
                            foreach (var col in columns)
                            {
                                table.Cell()
                                    .PaddingVertical(5)
                                    .Text(row[col]?.ToString() ?? "");
                            }
                        }
                    });

                    // Pie de página
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span($"Generado: {DateTime.Now:yyyy-MM-dd HH:mm}");
                            x.Span(" | ");
                            x.Span($"ID: {request.ReportId}");
                        });
                });
            });

            return pdf.GeneratePdf();
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