using Application.DTOs.ReportRequest;
using Application.Reports.Generators;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace Infrastructure.Reports.Generators
{
    /// <summary>
    /// Word report generator implementation.
    /// </summary>
    public class WordReportGenerator : IReportGenerator
    {
        public string SupportedFormat => "word";

        public async Task<byte[]> GenerateAsync(ReportRequestDto request)
        {
            using var memoryStream = new MemoryStream();
            using (var wordDocument = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document))
            {
                var mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                var body = mainPart.Document.AppendChild(new Body());
                var titleParagraph = body.AppendChild(new Paragraph());
                var titleRun = titleParagraph.AppendChild(new Run());
                titleRun.AppendChild(new Text($"Report: {request.ReportType}"));
                titleRun.RunProperties = new RunProperties(
                    new Bold(),
                    new FontSize { Val = "28" }
                );
                titleParagraph.ParagraphProperties = new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }
                );
                var infoParagraph = body.AppendChild(new Paragraph());
                var infoRun = infoParagraph.AppendChild(new Run());
                infoRun.AppendChild(new Text($"ID: {request.ReportId} | Generated: {request.GeneratedAt:yyyy-MM-dd HH:mm}"));
                infoParagraph.ParagraphProperties = new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }
                );
                body.AppendChild(new Paragraph(new Run(new Text(""))));
                var data = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(
                    request.Data,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
                if (data == null || data.Count == 0)
                {
                    var noDataParagraph = body.AppendChild(new Paragraph());
                    var noDataRun = noDataParagraph.AppendChild(new Run());
                    noDataRun.AppendChild(new Text("No data to display."));
                    return memoryStream.ToArray();
                }
                var table = new Table();
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
                var headers = data[0].Keys.ToList();
                var headerRow = new TableRow();
                foreach (var header in headers)
                {
                    var cell = new TableCell(new Paragraph(new Run(new Text(header))));
                    cell.TableCellProperties = new TableCellProperties(new Shading { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D3D3D3" });
                    headerRow.AppendChild(cell);
                }
                table.AppendChild(headerRow);
                foreach (var rowData in data)
                {
                    var row = new TableRow();
                    foreach (var header in headers)
                    {
                        var value = rowData[header]?.ToString() ?? string.Empty;
                        var cell = new TableCell(new Paragraph(new Run(new Text(value))));
                        row.AppendChild(cell);
                    }
                    table.AppendChild(row);
                }
                body.AppendChild(table);
                return memoryStream.ToArray();
            }
        }
    }
}
