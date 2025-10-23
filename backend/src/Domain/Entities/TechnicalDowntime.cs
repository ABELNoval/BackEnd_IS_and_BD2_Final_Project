namespace Domain.Entities
{
    public class TechnicalDowntime
    {
        public int Id { get; set; }
        public string Cause { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public Technical Technical { get; set; } = null!;
        public Equipment Equipment { get; set; } = null!;
        public DestinyType DestinyType { get; set; } = null!;
        public Department Department{ get; set; } = null!;

    }
}