namespace Application.DTOs.ReportResult
{
    public class EquipmentDecommissionLastYearDto
    {
        // Reporte 1: Equipos dados de baja en el último año
        public string EquipmentName { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public string State { get; set; } // Estado del equipo
        public DateTime DecommissionDate { get; set; }
        public string Reason { get; set; } // Causa de la baja
        public string DestinyType { get; set; } // Destino final
        public string TechnicalName { get; set; } // Persona que hace la baja
        public string TechnicalSpeciality { get; set; }
        public string TechnicalEmail { get; set; }
        public string Department { get; set; } // Departamento origen
        public int DaysInUse { get; set; } // Días desde adquisición hasta baja
    }
}