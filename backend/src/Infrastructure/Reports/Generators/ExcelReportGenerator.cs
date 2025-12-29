using Application.DTOs.ReportRequest;
using Application.Reports.Generators;
using OfficeOpenXml;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Infrastructure.Reports.Generators
{
    /// <summary>
    /// Excel report generator implementation.
    /// </summary>
    public class ExcelReportGenerator : IReportGenerator
    {
        public string SupportedFormat => "excel";

        public Task<byte[]> GenerateAsync(ReportRequestDto request)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Report");
            sheet.Cells["A1"].Value = $"Report: {request.ReportType}";
            sheet.Cells["A1"].Style.Font.Bold = true;
            sheet.Cells["A1"].Style.Font.Size = 18;
            sheet.Cells["A2"].Value = $"ID: {request.ReportId}";
            sheet.Cells["A3"].Value = $"Generated: {request.GeneratedAt:yyyy-MM-dd HH:mm}";
            sheet.Cells["A3"].Style.Font.Italic = true;
            var data = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(
                request.Data,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
            if (data == null || data.Count == 0)
            {
                sheet.Cells["A5"].Value = "No data available for this report.";
                return Task.FromResult(package.GetAsByteArray());
            }
            var headers = data[0].Keys.ToList();
            for (int i = 0; i < headers.Count; i++)
            {
                sheet.Cells[5, i + 1].Value = headers[i];
                sheet.Cells[5, i + 1].Style.Font.Bold = true;
                sheet.Cells[5, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                sheet.Cells[5, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }
            int row = 6;
            foreach (var rowData in data)
            {
                int col = 1;
                foreach (var header in headers)
                {
                    var value = rowData[header];
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
            return Task.FromResult(package.GetAsByteArray());
        }
    }
}
