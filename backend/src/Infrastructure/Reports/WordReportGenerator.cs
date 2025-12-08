using Application.DTOs.ReportRequest;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.Json;

namespace Infrastructure.Reports
{
    public class WordReportGenerator
    {
        public async Task<byte[]> Generate(ReportRequestDto request)
        {
            using var memoryStream = new MemoryStream();
            
            using (var wordDocument = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document))
            {
                var mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                var body = mainPart.Document.AppendChild(new Body());

                // Título
                var titleParagraph = body.AppendChild(new Paragraph());
                var titleRun = titleParagraph.AppendChild(new Run());
                titleRun.AppendChild(new Text($"Reporte: {GetReportTitle(request.ReportType)}"));
                titleRun.RunProperties = new RunProperties(
                    new Bold(),
                    new FontSize { Val = "28" }
                );
                titleParagraph.ParagraphProperties = new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }
                );

                // Información del reporte
                var infoParagraph = body.AppendChild(new Paragraph());
                var infoRun = infoParagraph.AppendChild(new Run());
                infoRun.AppendChild(new Text($"ID: {request.ReportId} | Generado: {request.GeneratedAt:yyyy-MM-dd HH:mm}"));
                infoParagraph.ParagraphProperties = new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }
                );

                // Espacio
                body.AppendChild(new Paragraph(new Run(new Text(""))));

                // Deserializar datos
                var data = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(
                    request.Data,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                // Si no hay datos
                if (data == null || data.Count == 0)
                {
                    var noDataParagraph = body.AppendChild(new Paragraph());
                    var noDataRun = noDataParagraph.AppendChild(new Run());
                    noDataRun.AppendChild(new Text("No hay datos para mostrar."));
                    return memoryStream.ToArray();
                }

                // Crear tabla
                var table = new Table();

                // Configurar bordes
                var tableProperties = new TableProperties(
                    new TableBorders(
                        new TopBorder { Val = BorderValues.Single, Size = 6 },
                        new BottomBorder { Val = BorderValues.Single, Size = 6 },
                        new LeftBorder { Val = BorderValues.Single, Size = 6 },
                        new RightBorder { Val = BorderValues.Single, Size = 6 },
                        new InsideHorizontalBorder { Val = BorderValues.Single, Size = 6 },
                        new InsideVerticalBorder { Val = BorderValues.Single, Size = 6 }
                    ),
                    new TableWidth { Width = "5000", Type = TableWidthUnitValues.Pct }
                );
                table.AppendChild(tableProperties);

                // Obtener encabezados
                var headers = data[0].Keys.ToList();

                // Crear fila de encabezados
                var headerRow = new TableRow();
                foreach (var header in headers)
                {
                    var cell = new TableCell();
                    var cellParagraph = new Paragraph();
                    var cellRun = new Run();
                    cellRun.AppendChild(new Text(GetColumnDisplayName(header, request.ReportType)));
                    cellRun.RunProperties = new RunProperties(new Bold());
                    cellParagraph.Append(cellRun);
                    cell.Append(cellParagraph);
                    cell.Append(new TableCellProperties(
                        new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center },
                        new Shading { Val = ShadingPatternValues.Clear, Fill = "D9D9D9" }
                    ));
                    headerRow.Append(cell);
                }
                table.Append(headerRow);

                // Crear filas de datos
                foreach (var rowData in data)
                {
                    var row = new TableRow();
                    foreach (var header in headers)
                    {
                        var cell = new TableCell();
                        var cellParagraph = new Paragraph();
                        var cellRun = new Run();
                        cellRun.AppendChild(new Text(rowData[header]?.ToString() ?? ""));
                        cellParagraph.Append(cellRun);
                        cell.Append(cellParagraph);
                        row.Append(cell);
                    }
                    table.Append(row);
                }

                body.AppendChild(table);
            }

            return memoryStream.ToArray();
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