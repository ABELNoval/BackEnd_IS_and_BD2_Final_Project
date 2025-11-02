using Domain.Entities;
namespace Application.DTOs.Transfet
{
    public class TransferDto
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int OriginDepartmentId { get; set; }
        public string OriginDepartmentName { get; set; } = string.Empty;
        public string OriginSectionName { get; set; } = string.Empty;
        public int DestinyDepartmentId { get; set; }
        public string DestinyDepartmentName { get; set; } = string.Empty;
        public string DestinySectionName { get; set; } = string.Empty;
        public int EquimentId { get; set; }
        public string EquimentName { get; set; } = string.Empty;
        public string EquipmentType { get; set; } = string.Empty;
        public string EquimentState { get; set; } = string.Empty;
    }
}