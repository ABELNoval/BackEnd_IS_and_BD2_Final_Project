namespace Application.DTOs.ReportResult
{
    public class EquipmentTransfersDto
    {
        // Reporte 3: Equipos trasladados entre secciones
        public Guid TransferId { get; set; }
        public DateTime TransferDate { get; set; }
        public string Reason { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentType { get; set; }
        public string EquipmentState { get; set; }
        public string SourceDepartment { get; set; }
        public string SourceSection { get; set; }
        public string SourceResponsible { get; set; }
        public string TargetDepartment { get; set; }
        public string TargetSection { get; set; }
        public string TargetResponsible { get; set; }
        public string TransferResponsible { get; set; }
        public string TransferResponsibleEmail { get; set; }
        public int DaysAtSource { get; set; } // Días en el origen
    }
}