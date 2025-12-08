namespace Application.DTOs.ReportResult
{
    public class EquipmentToDepartmentDto
    {
        // Reporte 7: Equipos enviados a un departamento específico
        public Guid TransferId { get; set; }
        public DateTime TransferDate { get; set; }
        public string TransferReason { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentType { get; set; }
        public string EquipmentState { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public string SourceDepartment { get; set; }
        public string SourceResponsible { get; set; }
        public string SourceEmail { get; set; }
        public string TargetDepartment { get; set; }
        public string TargetResponsible { get; set; }
        public string TargetEmail { get; set; }
        public string SendingCompany { get; set; } // Empresa que envía
        public string ReceivingCondition { get; set; } // Condición al recibir
        public bool IsDefective { get; set; } // Es equipo defectuoso
        public int DaysInTransit { get; set; } // Días en tránsito
    }
}