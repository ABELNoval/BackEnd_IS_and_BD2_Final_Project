namespace Application.DTOs.ReportRequest
{
    public class ReportRequestDto
    {
        public string ReportId { get; set; }
        public string ReportType { get; set; } // Nombre del reporte
        public string Data { get; set; } // JSON serializado de los resultados
        public DateTime GeneratedAt { get; set; }
        public object Parameters { get; set; } // Parámetros adicionales como IDs
    }
}