namespace Application.DTOs.ReportResult
{
    public class TechnicianPerformanceBonusDto
    {
        // Reporte 6: Rendimiento técnicos para bonificaciones
        public string TechnicalName { get; set; }
        public string Speciality { get; set; }
        public int Experience { get; set; }
        public string Email { get; set; }
        public int TotalInterventions { get; set; } // Intervenciones realizadas
        public decimal TotalInterventionCost { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalRatings { get; set; }
        public DateTime? LastRatingDate { get; set; }
        public decimal BonusScore { get; set; } // Puntaje para bonificación (0-10)
        public string BonusRecommendation { get; set; } // "BONIFICAR" o "MANTENER"
        public int DaysWithoutIntervention { get; set; } // Días sin intervención
    }
}