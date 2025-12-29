using Application.DTOs.ReportRequest;
using Application.Reports.Generators;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Reports.Generators
{
    /// <summary>
    /// PDF report generator implementation.
    /// </summary>
    public class PdfReportGenerator : IReportGenerator
    {
        public string SupportedFormat => "pdf";

        public PdfReportGenerator()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<byte[]> GenerateAsync(ReportRequestDto request)
        {
            // ...existing logic from previous PdfReportGenerator.Generate...
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
                    page.Header()
                        .PaddingBottom(20)
                        .AlignCenter()
                        .Text($"Report: {request.ReportType}")
                        .FontSize(20)
                        .Bold();
                    page.Content().Table(table =>
                    {
                        var columns = data.Count > 0 ? data[0].Keys.ToList() : new List<string>();
                        if (columns.Count == 0)
                        {
                            table.ColumnsDefinition(columnsDefinition => columnsDefinition.RelativeColumn());
                            table.Cell().Text("No data to display.");
                            return;
                        }
                        table.ColumnsDefinition(columnsDefinition =>
                        {
                            foreach (var _ in columns)
                                columnsDefinition.RelativeColumn();
                        });
                        foreach (var col in columns)
                        {
                            table.Cell()
                                .Background(Colors.Grey.Lighten3)
                                .PaddingVertical(5)
                                .Text(col)
                                .Bold()
                                .FontSize(11);
                        }
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
                });
            });
            var bytes = pdf.GeneratePdf();
            return await Task.FromResult(bytes);
        }
    }
}
