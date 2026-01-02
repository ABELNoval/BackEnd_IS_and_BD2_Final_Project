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
    /// PDF report generator implementation with professional formatting.
    /// </summary>
    public class PdfReportGenerator : IReportGenerator
    {
        public string SupportedFormat => "pdf";

        // Professional color scheme
        private static readonly string PrimaryColor = "#1E3A5F";      // Dark blue
        private static readonly string HeaderBgColor = "#2C5282";     // Medium blue
        private static readonly string HeaderTextColor = "#FFFFFF";   // White
        private static readonly string RowEvenColor = "#F7FAFC";      // Light gray-blue
        private static readonly string RowOddColor = "#FFFFFF";       // White
        private static readonly string BorderColor = "#CBD5E0";       // Gray border
        private static readonly string TextColor = "#2D3748";         // Dark gray text

        public PdfReportGenerator()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<byte[]> GenerateAsync(ReportRequestDto request)
        {
            var data = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(
                request.Data,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? new List<Dictionary<string, object>>();

            var pdf = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4.Landscape()); // Landscape for better table display
                    
                    // Header with logo area and title
                    page.Header().Container().Column(column =>
                    {
                        // Title bar
                        column.Item()
                            .Background(Color.FromHex(PrimaryColor))
                            .Padding(15)
                            .Row(row =>
                            {
                                row.RelativeItem()
                                    .AlignLeft()
                                    .Text("Equipment Management System")
                                    .FontSize(10)
                                    .FontColor(Colors.White);
                                    
                                row.RelativeItem()
                                    .AlignRight()
                                    .Text($"Generated: {request.GeneratedAt:yyyy-MM-dd HH:mm}")
                                    .FontSize(10)
                                    .FontColor(Colors.White);
                            });
                        
                        // Report title
                        column.Item()
                            .PaddingTop(15)
                            .PaddingBottom(10)
                            .AlignCenter()
                            .Text(FormatReportTitle(request.ReportType))
                            .FontSize(22)
                            .Bold()
                            .FontColor(Color.FromHex(PrimaryColor));
                        
                        // Report ID
                        column.Item()
                            .PaddingBottom(15)
                            .AlignCenter()
                            .Text($"Report ID: {request.ReportId}")
                            .FontSize(9)
                            .FontColor(Colors.Grey.Medium);
                        
                        // Separator line
                        column.Item()
                            .LineHorizontal(2)
                            .LineColor(Color.FromHex(HeaderBgColor));
                    });

                    // Main content with table
                    page.Content().PaddingTop(15).Element(container =>
                    {
                        if (data.Count == 0)
                        {
                            container.AlignCenter().AlignMiddle()
                                .Text("No data available for this report.")
                                .FontSize(14)
                                .FontColor(Colors.Grey.Medium);
                            return;
                        }

                        container.Table(table =>
                        {
                            var columns = data[0].Keys.ToList();
                            
                            // Define columns with proper sizing
                            table.ColumnsDefinition(columnsDefinition =>
                            {
                                foreach (var col in columns)
                                {
                                    // Give more space to certain columns
                                    if (col.Contains("Name") || col.Contains("Reason") || col.Contains("Description"))
                                        columnsDefinition.RelativeColumn(2);
                                    else if (col.Contains("Date") || col.Contains("Email"))
                                        columnsDefinition.RelativeColumn(1.5f);
                                    else
                                        columnsDefinition.RelativeColumn(1);
                                }
                            });

                            // Header row
                            table.Header(header =>
                            {
                                foreach (var col in columns)
                                {
                                    header.Cell()
                                        .Background(Color.FromHex(HeaderBgColor))
                                        .Border(1)
                                        .BorderColor(Color.FromHex(BorderColor))
                                        .Padding(8)
                                        .AlignCenter()
                                        .AlignMiddle()
                                        .Text(FormatColumnHeader(col))
                                        .Bold()
                                        .FontSize(10)
                                        .FontColor(Color.FromHex(HeaderTextColor));
                                }
                            });

                            // Data rows with alternating colors
                            int rowIndex = 0;
                            foreach (var row in data)
                            {
                                var bgColor = rowIndex % 2 == 0 
                                    ? Color.FromHex(RowEvenColor) 
                                    : Color.FromHex(RowOddColor);
                                
                                foreach (var col in columns)
                                {
                                    var cellValue = row.ContainsKey(col) ? row[col]?.ToString() ?? "" : "";
                                    
                                    table.Cell()
                                        .Background(bgColor)
                                        .Border(0.5f)
                                        .BorderColor(Color.FromHex(BorderColor))
                                        .Padding(6)
                                        .AlignMiddle()
                                        .Text(FormatCellValue(cellValue, col))
                                        .FontSize(9)
                                        .FontColor(Color.FromHex(TextColor));
                                }
                                rowIndex++;
                            }
                        });
                    });

                    // Footer
                    page.Footer().Container().Column(column =>
                    {
                        column.Item()
                            .LineHorizontal(1)
                            .LineColor(Color.FromHex(BorderColor));
                        
                        column.Item()
                            .PaddingTop(8)
                            .Row(row =>
                            {
                                row.RelativeItem()
                                    .AlignLeft()
                                    .Text($"Total Records: {data.Count}")
                                    .FontSize(9)
                                    .FontColor(Colors.Grey.Medium);
                                
                                row.RelativeItem()
                                    .AlignCenter()
                                    .Text(text =>
                                    {
                                        text.DefaultTextStyle(x => x.FontSize(9).FontColor(Colors.Grey.Medium));
                                        text.Span("Page ");
                                        text.CurrentPageNumber();
                                        text.Span(" of ");
                                        text.TotalPages();
                                    });
                                
                                row.RelativeItem()
                                    .AlignRight()
                                    .Text("Equipment Management System © 2026")
                                    .FontSize(9)
                                    .FontColor(Colors.Grey.Medium);
                            });
                    });
                });
            });
            
            var bytes = pdf.GeneratePdf();
            return await Task.FromResult(bytes);
        }

        /// <summary>
        /// Formats the report type into a readable title.
        /// </summary>
        private static string FormatReportTitle(string reportType)
        {
            if (string.IsNullOrEmpty(reportType)) return "Report";
            
            // Split camelCase or PascalCase and add spaces
            var result = System.Text.RegularExpressions.Regex.Replace(
                reportType, 
                "([a-z])([A-Z])", 
                "$1 $2"
            );
            
            // Replace underscores and hyphens with spaces
            result = result.Replace("_", " ").Replace("-", " ");
            
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.ToLower());
        }

        /// <summary>
        /// Formats column headers into readable text.
        /// </summary>
        private static string FormatColumnHeader(string column)
        {
            if (string.IsNullOrEmpty(column)) return "";
            
            // Split camelCase/PascalCase
            var result = System.Text.RegularExpressions.Regex.Replace(
                column, 
                "([a-z])([A-Z])", 
                "$1 $2"
            );
            
            // Remove Id suffix for cleaner headers
            if (result.EndsWith(" Id"))
                result = result.Substring(0, result.Length - 3);
            
            return result;
        }

        /// <summary>
        /// Formats cell values for display.
        /// </summary>
        private static string FormatCellValue(string value, string column)
        {
            if (string.IsNullOrEmpty(value)) return "-";
            
            // Format dates
            if (column.Contains("Date") && DateTime.TryParse(value, out var date))
            {
                return date.ToString("MMM dd, yyyy");
            }
            
            // Format currency/costs
            if (column.Contains("Cost") && decimal.TryParse(value, out var cost))
            {
                return cost.ToString("C2");
            }
            
            // Truncate long text
            if (value.Length > 50)
            {
                return value.Substring(0, 47) + "...";
            }
            
            return value;
        }
    }
}
