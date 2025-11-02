namespace Application.DTOs.Transfer
{
    public class TransferDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        

        public int OriginDepartmentId { get; set; }
        public string OriginDepartmentName { get; set; } = string.Empty;
        public string OriginSectionName { get; set; } = string.Empty;
        
        
        public int DestinyDepartmentId { get; set; }
        public string DestinyDepartmentName { get; set; } = string.Empty;
        public string DestinySectionName { get; set; } = string.Empty;
        
        
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; } = string.Empty;
        public string EquipmentType { get; set; } = string.Empty;
        public string EquimentState { get; set; } = string.Empty;
    }
}