namespace Domain.Entities
{
    class DestinyType
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty; //deshecho, almacén o sección
        public List<TechnicalDowntime> TechnicalDowntimes { get; set; } = new List<TechnicalDowntime>()!;
    }
}