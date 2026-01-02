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
    /// Word report generator implementation with professional formatting.
    /// </summary>
    public class WordReportGenerator : IReportGenerator
    {
        public string SupportedFormat => "word";

        // Professional color scheme (hex without #)
        private const string PrimaryColor = "1E3A5F";      // Dark blue
        private const string HeaderBgColor = "2C5282";     // Medium blue
        private const string HeaderTextColor = "FFFFFF";   // White
        private const string RowEvenColor = "F7FAFC";      // Light gray-blue
        private const string RowOddColor = "FFFFFF";       // White
        private const string BorderColor = "CBD5E0";       // Gray border
        private const string TextColor = "2D3748";         // Dark gray text

        public async Task<byte[]> GenerateAsync(ReportRequestDto request)
        {
            using var memoryStream = new MemoryStream();
            using (var wordDocument = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document))
            {
                var mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                var body = mainPart.Document.AppendChild(new Body());

                // Add page settings for landscape orientation
                var sectionProperties = new SectionProperties(
                    new PageSize { Width = 15840, Height = 12240, Orient = PageOrientationValues.Landscape },
                    new PageMargin { Top = 720, Right = 720, Bottom = 720, Left = 720, Header = 360, Footer = 360 }
                );

                // ===== HEADER SECTION =====
                // Title bar with dark background
                var headerTable = new Table();
                var headerTableProps = new TableProperties(
                    new TableWidth { Width = "5000", Type = TableWidthUnitValues.Pct },
                    new TableBorders(
                        new TopBorder { Val = BorderValues.None },
                        new BottomBorder { Val = BorderValues.None },
                        new LeftBorder { Val = BorderValues.None },
                        new RightBorder { Val = BorderValues.None }
                    )
                );
                headerTable.AppendChild(headerTableProps);

                var headerRow = new TableRow();
                
                // Left cell - System name
                var leftCell = CreateTableCell("Equipment Management System", HeaderTextColor, "18", false);
                leftCell.TableCellProperties = new TableCellProperties(
                    new Shading { Val = ShadingPatternValues.Clear, Color = "auto", Fill = PrimaryColor },
                    new TableCellWidth { Width = "2500", Type = TableWidthUnitValues.Pct },
                    new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
                );
                AddCellPadding(leftCell);
                headerRow.AppendChild(leftCell);

                // Right cell - Generation date
                var rightCell = CreateTableCell($"Generated: {request.GeneratedAt:yyyy-MM-dd HH:mm}", HeaderTextColor, "18", false);
                rightCell.TableCellProperties = new TableCellProperties(
                    new Shading { Val = ShadingPatternValues.Clear, Color = "auto", Fill = PrimaryColor },
                    new TableCellWidth { Width = "2500", Type = TableWidthUnitValues.Pct },
                    new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
                );
                AddCellPadding(rightCell);
                // Right align text in this cell
                var rightPara = rightCell.Descendants<Paragraph>().First();
                rightPara.ParagraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Right });
                headerRow.AppendChild(rightCell);

                headerTable.AppendChild(headerRow);
                body.AppendChild(headerTable);

                // Spacing after header
                body.AppendChild(CreateSpacingParagraph(200));

                // Report Title
                var titleParagraph = body.AppendChild(new Paragraph());
                var titleRun = titleParagraph.AppendChild(new Run());
                titleRun.AppendChild(new Text(FormatReportTitle(request.ReportType)));
                titleRun.RunProperties = new RunProperties(
                    new Bold(),
                    new FontSize { Val = "44" },
                    new Color { Val = PrimaryColor }
                );
                titleParagraph.ParagraphProperties = new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center },
                    new SpacingBetweenLines { After = "100" }
                );

                // Report ID
                var idParagraph = body.AppendChild(new Paragraph());
                var idRun = idParagraph.AppendChild(new Run());
                idRun.AppendChild(new Text($"Report ID: {request.ReportId}"));
                idRun.RunProperties = new RunProperties(
                    new FontSize { Val = "18" },
                    new Color { Val = "718096" }
                );
                idParagraph.ParagraphProperties = new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center },
                    new SpacingBetweenLines { After = "200" }
                );

                // Separator line
                var separatorPara = body.AppendChild(new Paragraph());
                separatorPara.ParagraphProperties = new ParagraphProperties(
                    new ParagraphBorders(
                        new BottomBorder { Val = BorderValues.Single, Size = 12, Color = HeaderBgColor }
                    ),
                    new SpacingBetweenLines { After = "300" }
                );

                // Parse data
                var data = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(
                    request.Data,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                if (data == null || data.Count == 0)
                {
                    var noDataParagraph = body.AppendChild(new Paragraph());
                    var noDataRun = noDataParagraph.AppendChild(new Run());
                    noDataRun.AppendChild(new Text("No data available for this report."));
                    noDataRun.RunProperties = new RunProperties(
                        new Italic(),
                        new FontSize { Val = "24" },
                        new Color { Val = "718096" }
                    );
                    noDataParagraph.ParagraphProperties = new ParagraphProperties(
                        new Justification { Val = JustificationValues.Center }
                    );
                    body.AppendChild(sectionProperties);
                    mainPart.Document.Save();
                    return memoryStream.ToArray();
                }

                // ===== DATA TABLE =====
                var table = new Table();
                var tableProperties = new TableProperties(
                    new TableStyle { Val = "TableGrid" },
                    new TableWidth { Width = "5000", Type = TableWidthUnitValues.Pct },
                    new TableBorders(
                        new TopBorder { Val = BorderValues.Single, Size = 8, Color = BorderColor },
                        new BottomBorder { Val = BorderValues.Single, Size = 8, Color = BorderColor },
                        new LeftBorder { Val = BorderValues.Single, Size = 8, Color = BorderColor },
                        new RightBorder { Val = BorderValues.Single, Size = 8, Color = BorderColor },
                        new InsideHorizontalBorder { Val = BorderValues.Single, Size = 4, Color = BorderColor },
                        new InsideVerticalBorder { Val = BorderValues.Single, Size = 4, Color = BorderColor }
                    ),
                    new TableLook { Val = "04A0" }
                );
                table.AppendChild(tableProperties);

                var headers = data[0].Keys.ToList();

                // Header row with styling
                var tableHeaderRow = new TableRow();
                tableHeaderRow.TableRowProperties = new TableRowProperties(
                    new TableRowHeight { Val = 400, HeightType = HeightRuleValues.AtLeast }
                );

                foreach (var header in headers)
                {
                    var cell = CreateTableCell(FormatColumnHeader(header), HeaderTextColor, "20", true);
                    cell.TableCellProperties = new TableCellProperties(
                        new Shading { Val = ShadingPatternValues.Clear, Color = "auto", Fill = HeaderBgColor },
                        new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
                    );
                    AddCellPadding(cell);
                    // Center align header text
                    var headerPara = cell.Descendants<Paragraph>().First();
                    headerPara.ParagraphProperties = new ParagraphProperties(
                        new Justification { Val = JustificationValues.Center }
                    );
                    tableHeaderRow.AppendChild(cell);
                }
                table.AppendChild(tableHeaderRow);

                // Data rows with alternating colors
                int rowIndex = 0;
                foreach (var rowData in data)
                {
                    var row = new TableRow();
                    row.TableRowProperties = new TableRowProperties(
                        new TableRowHeight { Val = 350, HeightType = HeightRuleValues.AtLeast }
                    );

                    var bgColor = rowIndex % 2 == 0 ? RowEvenColor : RowOddColor;

                    foreach (var header in headers)
                    {
                        var value = rowData.ContainsKey(header) ? rowData[header]?.ToString() ?? "" : "";
                        var formattedValue = FormatCellValue(value, header);
                        
                        var cell = CreateTableCell(formattedValue, TextColor, "18", false);
                        cell.TableCellProperties = new TableCellProperties(
                            new Shading { Val = ShadingPatternValues.Clear, Color = "auto", Fill = bgColor },
                            new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
                        );
                        AddCellPadding(cell);
                        row.AppendChild(cell);
                    }
                    table.AppendChild(row);
                    rowIndex++;
                }

                body.AppendChild(table);

                // ===== FOOTER SECTION =====
                body.AppendChild(CreateSpacingParagraph(300));

                // Footer separator
                var footerSeparator = body.AppendChild(new Paragraph());
                footerSeparator.ParagraphProperties = new ParagraphProperties(
                    new ParagraphBorders(
                        new TopBorder { Val = BorderValues.Single, Size = 4, Color = BorderColor }
                    ),
                    new SpacingBetweenLines { Before = "200" }
                );

                // Footer info table
                var footerTable = new Table();
                var footerTableProps = new TableProperties(
                    new TableWidth { Width = "5000", Type = TableWidthUnitValues.Pct },
                    new TableBorders(
                        new TopBorder { Val = BorderValues.None },
                        new BottomBorder { Val = BorderValues.None },
                        new LeftBorder { Val = BorderValues.None },
                        new RightBorder { Val = BorderValues.None }
                    )
                );
                footerTable.AppendChild(footerTableProps);

                var footerRow = new TableRow();

                // Total records
                var totalCell = CreateTableCell($"Total Records: {data.Count}", "718096", "18", false);
                totalCell.TableCellProperties = new TableCellProperties(
                    new TableCellWidth { Width = "3333", Type = TableWidthUnitValues.Pct }
                );
                footerRow.AppendChild(totalCell);

                // Empty center cell
                var centerCell = CreateTableCell("", "718096", "18", false);
                centerCell.TableCellProperties = new TableCellProperties(
                    new TableCellWidth { Width = "3333", Type = TableWidthUnitValues.Pct }
                );
                var centerPara = centerCell.Descendants<Paragraph>().First();
                centerPara.ParagraphProperties = new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }
                );
                footerRow.AppendChild(centerCell);

                // Copyright
                var copyrightCell = CreateTableCell("Equipment Management System © 2026", "718096", "18", false);
                copyrightCell.TableCellProperties = new TableCellProperties(
                    new TableCellWidth { Width = "3333", Type = TableWidthUnitValues.Pct }
                );
                var copyrightPara = copyrightCell.Descendants<Paragraph>().First();
                copyrightPara.ParagraphProperties = new ParagraphProperties(
                    new Justification { Val = JustificationValues.Right }
                );
                footerRow.AppendChild(copyrightCell);

                footerTable.AppendChild(footerRow);
                body.AppendChild(footerTable);

                // Add section properties (page layout)
                body.AppendChild(sectionProperties);

                mainPart.Document.Save();
            }
            return await Task.FromResult(memoryStream.ToArray());
        }

        /// <summary>
        /// Creates a table cell with specified text and styling.
        /// </summary>
        private static TableCell CreateTableCell(string text, string textColor, string fontSize, bool isBold)
        {
            var cell = new TableCell();
            var paragraph = new Paragraph();
            var run = new Run();
            
            var runProperties = new RunProperties(
                new FontSize { Val = fontSize },
                new Color { Val = textColor }
            );
            
            if (isBold)
            {
                runProperties.AppendChild(new Bold());
            }
            
            run.RunProperties = runProperties;
            run.AppendChild(new Text(text));
            paragraph.AppendChild(run);
            cell.AppendChild(paragraph);
            
            return cell;
        }

        /// <summary>
        /// Adds padding to a table cell.
        /// </summary>
        private static void AddCellPadding(TableCell cell)
        {
            if (cell.TableCellProperties == null)
                cell.TableCellProperties = new TableCellProperties();
            
            cell.TableCellProperties.AppendChild(new TableCellMargin(
                new TopMargin { Width = "80", Type = TableWidthUnitValues.Dxa },
                new BottomMargin { Width = "80", Type = TableWidthUnitValues.Dxa },
                new LeftMargin { Width = "120", Type = TableWidthUnitValues.Dxa },
                new RightMargin { Width = "120", Type = TableWidthUnitValues.Dxa }
            ));
        }

        /// <summary>
        /// Creates an empty paragraph for spacing.
        /// </summary>
        private static Paragraph CreateSpacingParagraph(int spacing)
        {
            return new Paragraph(
                new ParagraphProperties(
                    new SpacingBetweenLines { After = spacing.ToString() }
                )
            );
        }

        /// <summary>
        /// Formats the report type into a readable title.
        /// </summary>
        private static string FormatReportTitle(string reportType)
        {
            if (string.IsNullOrEmpty(reportType)) return "Report";
            
            var result = System.Text.RegularExpressions.Regex.Replace(
                reportType, 
                "([a-z])([A-Z])", 
                "$1 $2"
            );
            
            result = result.Replace("_", " ").Replace("-", " ");
            
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.ToLower());
        }

        /// <summary>
        /// Formats column headers into readable text.
        /// </summary>
        private static string FormatColumnHeader(string column)
        {
            if (string.IsNullOrEmpty(column)) return "";
            
            var result = System.Text.RegularExpressions.Regex.Replace(
                column, 
                "([a-z])([A-Z])", 
                "$1 $2"
            );
            
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
            
            if (column.Contains("Date") && DateTime.TryParse(value, out var date))
            {
                return date.ToString("MMM dd, yyyy");
            }
            
            if (column.Contains("Cost") && decimal.TryParse(value, out var cost))
            {
                return cost.ToString("C2");
            }
            
            if (value.Length > 50)
            {
                return value.Substring(0, 47) + "...";
            }
            
            return value;
        }
    }
}
